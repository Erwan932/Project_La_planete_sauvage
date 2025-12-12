using UnityEngine;
using UnityEngine.UI;

public class DialogueTriggermenu : MonoBehaviour
{
    [Header("Textes du tutoriel")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Références")]
    public Dialogue dialogue;          // Script Dialogue
    public GameObject spriteToShow;    // Sprite à afficher
    public GameObject canvasObject;    // Canvas / panel
    public Image imageToFlash;         // Image à faire clignoter
    public GameObject imageToHide;     // Image à cacher lors du trigger
    public string nameID;              // Identifiant unique du trigger

    [Header("Effet Zoom")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float zoomSpeed = 2f;

    [Header("Effet Flash Image")]
    public float flashSpeed = 5f;
    public float flashAlpha = 0.6f;

    private bool isEffectActive = false;
    private Vector3 originalScale;
    private Color originalImageColor;

    private void Start()
    {
        if (canvasObject != null)
            originalScale = canvasObject.transform.localScale;

        if (imageToFlash != null)
            originalImageColor = imageToFlash.color;

        if (!CheckpointData.savedStates.ContainsKey(nameID))
            CheckpointData.savedStates.Add(nameID, false);
    }

    private void Update()
    {
        if (!isEffectActive) return;

        // 🔹 Zoom / dézoom du canvas
        if (canvasObject != null)
        {
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * zoomSpeed) + 1f) / 2f);
            canvasObject.transform.localScale = originalScale * scale;
        }

        // 🔹 Clignotement de l'image
        if (imageToFlash != null)
        {
            float alpha = Mathf.Lerp(0f, flashAlpha, (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f);
            Color c = originalImageColor;
            c.a = alpha;
            imageToFlash.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (CheckpointData.savedStates.ContainsKey(nameID) && CheckpointData.savedStates[nameID] == true)
            return;

        if (coll.CompareTag("Player"))
        {
            // 🔹 Affiche le sprite à montrer
            if (spriteToShow != null)
                spriteToShow.SetActive(true);

            // 🔹 Cache l'image à supprimer
            if (imageToHide != null)
                imageToHide.SetActive(false);

            // 🔹 Affiche le canvas et active les effets
            if (canvasObject != null)
            {
                canvasObject.SetActive(true);
                isEffectActive = true;
            }

            // 🔹 Lance le dialogue
            if (dialogue != null)
                dialogue.StartNewDialogue(lines);

            CheckpointData.savedStates[nameID] = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;

        // 🔹 Cache le sprite
        if (spriteToShow != null)
            spriteToShow.SetActive(false);

        // 🔹 Arrête les effets
        isEffectActive = false;

        // 🔹 Remet le canvas et l’image à l’état normal
        if (canvasObject != null)
        {
            canvasObject.transform.localScale = originalScale;
            canvasObject.SetActive(true); // reste visible si nécessaire
        }

        if (imageToFlash != null)
            imageToFlash.color = originalImageColor;

        // 🔹 Remet l'image supprimée visible si besoin
        if (imageToHide != null)
            imageToHide.SetActive(true);

        // 🔹 Ferme le dialogue
        if (dialogue != null)
            dialogue.ForceCloseDialogue();
    }
}
