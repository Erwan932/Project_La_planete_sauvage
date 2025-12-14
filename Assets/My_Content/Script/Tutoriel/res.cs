using UnityEngine;
using System.Collections;

public class CanvasFade : MonoBehaviour
{
    [Header("Canvas Group cible")]
    public CanvasGroup canvasGroup;   // Contrôle l’opacité du Canvas entier

    [Header("Durées")]
    public float fadeDuration = 1.5f;
    public float blackScreenHoldTime = 2f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(blackScreenHoldTime);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        // Si alpha = 0 → désactiver le Canvas pour ne plus bloquer les clics
        if (endAlpha == 0f)
            gameObject.SetActive(false);
    }
}
