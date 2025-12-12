using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Textes du tutoriel")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Références")]
    public Dialogue dialogue;       // ton script Dialogue
    public GameObject spriteToShow; // ton sprite à afficher

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            // Affiche le sprite si besoin
            if (spriteToShow != null)
                spriteToShow.SetActive(true);

            // Lance le dialogue
            if (dialogue != null)
                dialogue.StartNewDialogue(lines);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            // Cache le sprite si besoin
            if (spriteToShow != null)
                spriteToShow.SetActive(false);

            // Ferme le dialogue proprement
            if (dialogue != null)
                dialogue.ForceCloseDialogue();
        }
    }
}
