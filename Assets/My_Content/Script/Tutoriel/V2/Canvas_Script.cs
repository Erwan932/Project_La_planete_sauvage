using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueBox : MonoBehaviour
{
    [Header("UI Références")]
    public GameObject canvasDialogue;
    public Image backgroundImage;
    public Image characterImage;
    public TextMeshProUGUI dialogueText;

    [Header("Réglages")]
    public float timeBetweenLines = 2f;

    [Header("Player Input")]
    public MonoBehaviour playerMovementScript;

    private string[] currentLines;
    private int currentIndex;
    private bool dialogueActive;
    private Coroutine dialogueCoroutine;

    private void Start()
    {
        if (canvasDialogue != null)
            canvasDialogue.SetActive(false);
    }

    // 🔹 Lancer un dialogue
    public void StartDialogue(string[] lines, Sprite image = null)
    {
        if (dialogueActive)
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

        // ⛔ Bloquer les inputs joueur
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        dialogueCoroutine = StartCoroutine(AutoDialogue());
    }

    private IEnumerator AutoDialogue()
    {
        while (dialogueActive)
        {
            yield return new WaitForSeconds(timeBetweenLines);
            NextLine();
        }
    }

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

    // 🔹 Fin du dialogue
    private void EndDialogue()
    {
        dialogueActive = false;

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        canvasDialogue.SetActive(false);
        dialogueText.text = "";

        // ✅ Rendre les inputs joueur
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}
