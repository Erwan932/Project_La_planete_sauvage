using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialText tutorialText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        tutorialText.ShowSecondText();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        tutorialText.HideText();
    }
}
