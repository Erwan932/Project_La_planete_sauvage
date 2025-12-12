using UnityEngine;

public class TutorialTriggerBox2 : MonoBehaviour
{
    public TutorialText tutorial;
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            tutorial.ShowFinalText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            tutorial.HideFinalText();
        }
    }
}
