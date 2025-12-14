using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Texte cible")]
    public TMP_Text targetText; // Le Text UI à remplir

    [Header("Paramètres")]
    [TextArea]
    public string fullText; // Le texte complet à afficher
    public float duration = 2f; // Durée totale (en secondes) pour afficher tout le texte

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        // Quand le canvas s'active, on lance l'effet
        if (targetText != null && !string.IsNullOrEmpty(fullText))
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(ShowText());
        }
    }

    private IEnumerator ShowText()
    {
        targetText.text = "";

        float delay = duration / fullText.Length; // temps par lettre

        foreach (char c in fullText)
        {
            targetText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
