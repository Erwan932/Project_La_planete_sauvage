using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;   // Le(s) Tooltip_Background à détruire
    public float typingSpeed = 0.03f;
    public float delayBetweenLines = 2f;

    [TextArea(3, 10)]
    public string[] lines;

    [TextArea]
    public string extraLine;

    private int index = 0;
    private Coroutine dialogueCoroutine;
    private bool playerInside = false;
    private bool pressedB = false;

    void Start()
    {
        HideAll();
    }

    void Update()
    {
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
            pressedB = false;
            index = 0;

            ShowAll();

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

            // 🔥 Détruit définitivement le/les Tooltip_Background
            foreach (var bg in backgrounds)
            {
                if (bg != null)
                    Destroy(bg.gameObject);
            }

            // ⚠ La trigger NE se détruit pas → le joueur peut revenir
        }
    }

    IEnumerator PlayDialogue()
    {
        while (index < lines.Length && playerInside)
        {
            yield return StartCoroutine(TypeLine(lines[index]));
            yield return new WaitForSeconds(delayBetweenLines);
            index++;

            if (pressedB)
                yield break;
        }

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

        if (pressedB)
        {
            yield return new WaitForSeconds(2f);
            HideAll();
            playerInside = false;
        }
    }

    void ShowAll()
    {
        if (dialogueText != null)
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);

        foreach (var bg in backgrounds)
        {
            if (bg == null) continue;
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    void HideAll()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);
        }

        foreach (var bg in backgrounds)
        {
            if (bg == null) continue;
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    // 🔥 Empêche le texte de se retourner
    void LateUpdate()
    {
        Vector3 scale = transform.localScale;
        if (scale.x < 0)
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
