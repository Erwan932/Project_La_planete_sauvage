using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CanvasFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public float blackScreenHoldTime = 2f;

    private void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();
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
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, endAlpha);
    }

    internal string FadeInRealtime()
    {
        throw new NotImplementedException();
    }

    internal string FadeOutRealtime()
    {
        throw new NotImplementedException();
    }
}
