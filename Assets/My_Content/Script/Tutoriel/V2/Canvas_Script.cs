using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("UI Références")]
    public GameObject canvasDialogue;          // Le Canvas principal
    public Image backgroundImage;               // Background du dialogue
    public Image characterImage;                // Image (portrait, icône, etc.)
    public TextMeshProUGUI dialogueText;         // Texte du dialogue

    [Header("Réglages")]
    public KeyCode skipKey = KeyCode.B;

    private string[] currentLines;
    private int currentIndex;
    private bool dialogueActive;

    private void Start()
    {
        canvasDialogue.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetKeyDown(skipKey))
        {
            NextLine();
        }
    }

    // 🔹 Lancer un dialogue depuis une trigger
    public void StartDialogue(string[] lines, Sprite image = null)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentIndex = 0;
        dialogueActive = true;

        canvasDialogue.SetActive(true);
        dialogueText.text = currentLines[currentIndex];

        if (characterImage != null)
        {
            characterImage.gameObject.SetActive(image != null);
            characterImage.sprite = image;
        }
    }

    // 🔹 Passer à la ligne suivante
    private void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = currentLines[currentIndex];
        }
    }

    // 🔹 Fermer le dialogue
    private void EndDialogue()
    {
        dialogueActive = false;
        canvasDialogue.SetActive(false);
        dialogueText.text = "";
    }
}
