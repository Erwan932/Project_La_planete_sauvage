using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Textes du tutoriel")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Références")]
    public Dialogue dialogue;          // ton script Dialogue
    public GameObject spriteToShow;    // ton sprite à afficher
    public GameObject canvasObject;    // ton canvas/panel

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            // 🔹 Affiche le sprite
            if (spriteToShow != null)
                spriteToShow.SetActive(true);

            // 🔹 Affiche le canvas
            if (canvasObject != null)
                canvasObject.SetActive(true);

            // 🔹 Lance le dialogue
            if (dialogue != null)
                dialogue.StartNewDialogue(lines);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            // 🔹 Cache le sprite
            if (spriteToShow != null)
                spriteToShow.SetActive(false);

            // 🔹 Cache le canvas
            if (canvasObject != null)
                canvasObject.SetActive(false);

            // 🔹 Ferme le dialogue proprement
            if (dialogue != null)
                dialogue.ForceCloseDialogue();
        }
    }
}
