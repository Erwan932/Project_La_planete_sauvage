using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject dialogueCanvas;   // Le GameObject parent du Canvas (activé/désactivé)
    public Image imageLeft;
    public Image imageRight;
    public TextMeshProUGUI dialogueText;

    private void Start()
    {
        // Le canvas est désactivé au démarrage
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    // Fonction appelée par les triggers
    public void ShowDialogue(string text, Sprite leftImage = null, Sprite rightImage = null)
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = text;

        if (imageLeft != null)
            imageLeft.sprite = leftImage;

        if (imageRight != null)
            imageRight.sprite = rightImage;
    }

    public void HideDialogue()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }
}
