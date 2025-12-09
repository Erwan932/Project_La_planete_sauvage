using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;
    public float typingSpeed = 0.03f;
    public float delayBetweenLines = 2f;

    [TextArea(3, 10)]
    public string[] lines;

    [TextArea]
    public string extraLine; // ➤ TEXTE affiché quand on appuie sur B

    private int index = 0;
    private Coroutine dialogueCoroutine;
    private bool playerInside = false;
    private bool pressedB = false; // ➤ Pour savoir si B a été pressé

    void Start()
    {
        HideAll();
    }

    void Update()
    {
        // ➤ Touche B (manette Xbox : "joystick button 1")
        if (playerInside && Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            pressedB = true;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            dialogueCoroutine = StartCoroutine(TypeLine(extraLine));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;
            ShowAll();
            pressedB = false;
            index = 0;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            dialogueCoroutine = StartCoroutine(PlayDialogue());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            HideAll();
        }
    }

    IEnumerator PlayDialogue()
    {
        while (index < lines.Length && playerInside)
        {
            yield return StartCoroutine(TypeLine(lines[index]));
            yield return new WaitForSeconds(delayBetweenLines);
            index++;

            // ➤ Si le joueur a appuyé sur B, on coupe le cycle normal
            if (pressedB)
                yield break;
        }

        // ➤ Si B n’a pas été pressé, finir normalement
        if (playerInside && !pressedB)
            HideAll();
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line)
        {
            if (!playerInside) yield break;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // ➤ Si c'est le texte B, attendre avant de disparaître
        if (pressedB)
        {
            yield return new WaitForSeconds(2f);
            HideAll();
            playerInside = false;
        }
    }

    void ShowAll()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    void HideAll()
    {
        dialogueText.text = "";

        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
