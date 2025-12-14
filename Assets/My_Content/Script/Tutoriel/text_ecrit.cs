using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypewriterText : MonoBehaviour
{
    [Header("UI")]
    public Text uiText;           // Texte UI (Text classique)

    [Header("Réglages écriture")]
    [TextArea(2, 4)]
    public string fullText;
    public float letterDelay = 0.05f; // Temps entre chaque lettre

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        StartTyping();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        uiText.text = "";

        foreach (char letter in fullText)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    public void SetText(string newText)
    {
        fullText = newText;
    }

    public void SetSpeed(float newSpeed)
    {
        letterDelay = newSpeed;
    }
}
