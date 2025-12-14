using UnityEngine;
using System.Collections;

public class UIRocketLaunch : MonoBehaviour
{
    [Header("UI")]
    public RectTransform rocketTransform;

    [Header("Décollage")]
    public float launchHeight = 800f;
    public float launchDuration = 2f;

    [Header("Tremblement")]
    public float shakeDuration = 0.6f;
    public float shakeStrength = 10f;

    private Vector2 startPosition;

    private void Awake()
    {
        if (rocketTransform == null)
            rocketTransform = GetComponent<RectTransform>();

        startPosition = rocketTransform.anchoredPosition;
    }

    private void Start()
    {
        // 🚀 Lancement automatique dès le début
        StartCoroutine(LaunchRoutine());
    }

    private IEnumerator LaunchRoutine()
    {
        // 🔥 Tremblement
        yield return StartCoroutine(Shake());

        // 🚀 Montée
        float time = 0f;
        Vector2 targetPosition = startPosition + Vector2.up * launchHeight;

        while (time < launchDuration)
        {
            time += Time.deltaTime;
            float t = time / launchDuration;

            rocketTransform.anchoredPosition =
                Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        rocketTransform.anchoredPosition = targetPosition;
    }

    private IEnumerator Shake()
    {
        float time = 0f;

        while (time < shakeDuration)
        {
            time += Time.deltaTime;

            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            rocketTransform.anchoredPosition =
                startPosition + new Vector2(offsetX, offsetY);

            yield return null;
        }

        rocketTransform.anchoredPosition = startPosition;
    }
}
