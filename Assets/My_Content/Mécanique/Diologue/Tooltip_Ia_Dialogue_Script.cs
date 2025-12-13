using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;

    [Header("Réglages du dialogue")]
    public float typingSpeed = 0.03f;
    public float delayBetweenLines = 2f;

    [Header("Identifiant et textes")]
    public string nameID; // Pour sauvegarde
    [TextArea(3, 10)]
    public string[] defaultLines; // Lignes par défaut
    [TextArea]
    public string triggerText;    // Texte spécifique pour ce trigger (optionnel)

    private string[] linesToUse;
    private int index = 0;
    private Coroutine dialogueCoroutine;
    private bool playerInside = false;

    void Start()
    {
        HideAll();

        if (!CheckpointData.savedStates.ContainsKey(nameID))
            CheckpointData.savedStates.Add(nameID, false);

        linesToUse = defaultLines;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") ||
            (CheckpointData.savedStates.ContainsKey(nameID) && CheckpointData.savedStates[nameID]))
            return;

        playerInside = true;
        index = 0;

        // Choix du texte
        if (!string.IsNullOrEmpty(triggerText))
            linesToUse = new string[] { triggerText };
        else
            linesToUse = defaultLines;

        ShowAll();

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(PlayDialogue());
        CheckpointData.savedStates[nameID] = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        HideAll();

        // Détruit définitivement les backgrounds
        foreach (var bg in backgrounds)
        {
            if (bg != null)
                Destroy(bg.gameObject);
        }
    }

    IEnumerator PlayDialogue()
    {
        while (index < linesToUse.Length && playerInside)
        {
            yield return StartCoroutine(TypeLine(linesToUse[index]));
            yield return new WaitForSeconds(delayBetweenLines);
            index++;
        }

        if (playerInside)
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
    }

    void ShowAll()
    {
        if (dialogueText != null)
            dialogueText.color = new Color(
                dialogueText.color.r,
                dialogueText.color.g,
                dialogueText.color.b,
                1f
            );

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
            dialogueText.color = new Color(
                dialogueText.color.r,
                dialogueText.color.g,
                dialogueText.color.b,
                0f
            );
        }

        foreach (var bg in backgrounds)
        {
            if (bg == null) continue;

            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    // 🔁 Corrige le flip pour que le texte reste toujours lisible
    void LateUpdate()
    {
        if (transform.parent == null) return;

        float parentScaleX = transform.parent.localScale.x;

        Vector3 localScale = transform.localScale;
        localScale.x = parentScaleX < 0
            ? -Mathf.Abs(localScale.x)
            : Mathf.Abs(localScale.x);

        transform.localScale = localScale;
    }
}
