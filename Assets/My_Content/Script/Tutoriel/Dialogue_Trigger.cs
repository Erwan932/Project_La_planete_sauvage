using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Textes du tutoriel")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Références")]
    public Dialogue dialogue;       // Script Dialogue
    public GameObject spriteToShow; // Sprite à afficher

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;

        // Affiche le sprite si besoin
        if (spriteToShow != null)
            spriteToShow.SetActive(true);

        // Lance le dialogue
        if (dialogue != null)
            dialogue.StartNewDialogue(lines);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Player")) return;

        // Cache le sprite si besoin
        if (spriteToShow != null)
            spriteToShow.SetActive(false);

        // Ferme le dialogue
        if (dialogue != null)
            dialogue.ForceCloseDialogue();

        // Sécurité : assure que la trigger reste active
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // Sécurité : assure que le script reste actif
        if (!enabled)
            enabled = true;
    }
}
