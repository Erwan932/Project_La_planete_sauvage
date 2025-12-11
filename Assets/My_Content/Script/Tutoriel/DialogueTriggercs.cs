using UnityEngine;

public class DialogueTriggercs : MonoBehaviour
{
    [Header("Dialogue à afficher")]
    [TextArea(3, 10)]
    public string dialogueText;

    public Sprite leftImage;
    public Sprite rightImage;

    [Header("Référence au Dialogue UI")]
    public DialogueUI dialogueUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogueUI != null)
            {
                dialogueUI.ShowDialogue(dialogueText, leftImage, rightImage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogueUI != null)
            {
                dialogueUI.HideDialogue();
            }
        }
    }
}
