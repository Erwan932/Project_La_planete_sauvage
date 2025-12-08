using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;   // Texte TMP
    public SpriteRenderer[] backgrounds;   // Plusieurs BACKGROUNDS (SpriteRenderer)
    public float typingSpeed = 0.03f;
    public float delayBetweenLines = 2f;

    [TextArea(3, 10)]
    public string[] lines;

    private int index = 0;

    void Start()
    {
        if (dialogueText != null)
            dialogueText.text = "";

        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (index < lines.Length)
        {
            yield return StartCoroutine(TypeLine(lines[index]));
            yield return new WaitForSeconds(delayBetweenLines);
            index++;
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeOut()
    {
        float t = 1f;

        Color textColor = dialogueText.color;
        Color[] bgColors = new Color[backgrounds.Length];

        // On sauvegarde les couleurs de chaque background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            bgColors[i] = backgrounds[i].color;
        }

        while (t > 0)
        {
            t -= Time.deltaTime;

            // fade texte
            dialogueText.color = new Color(textColor.r, textColor.g, textColor.b, t);

            // fade de tous les backgrounds
            for (int i = 0; i < backgrounds.Length; i++)
            {
                SpriteRenderer bg = backgrounds[i];
                Color original = bgColors[i];
                bg.color = new Color(original.r, original.g, original.b, t);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}

