using UnityEngine;

public class CanvasDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Références")]
    public DialogueBox dialogueBox;
    public Sprite imageToShow;

    [Header("Options")]
    public bool playOnlyOnce = true;

    private bool hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playOnlyOnce && hasPlayed)
            return;

        dialogueBox.StartDialogue(dialogueLines, imageToShow);
        hasPlayed = true;
    }
}
