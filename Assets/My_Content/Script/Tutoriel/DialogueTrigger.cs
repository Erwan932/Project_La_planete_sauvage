using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogue.ActivateDialogue();
            Destroy(gameObject); // le trigger ne sert plus
        }
    }
}
