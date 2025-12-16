using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public bool fadeOnEnable = true;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // État initial
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OnEnable()
    {
        if (fadeOnEnable)
            FadeIn();
    }

    public void FadeIn()
    {
        StartFade(0f, 1f, true);
    }

    public void FadeOut()
    {
        StartFade(1f, 0f, false);
    }

    private void StartFade(float from, float to, bool enable)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(from, to, enable));
    }

    private IEnumerator FadeRoutine(float from, float to, bool enable)
    {
        float timer = 0f;

        canvasGroup.alpha = from;
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;
    }
}
