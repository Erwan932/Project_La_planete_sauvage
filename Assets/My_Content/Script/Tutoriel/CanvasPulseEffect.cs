using UnityEngine;
using System.Collections;

public class CanvasPulseEffect : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float speed = 2f;

    [Header("Flash Settings")]
    public CanvasGroup flashGroup;
    public float flashIntensity = 0.6f;
    public float flashSpeed = 5f;

    private bool isActive = false;
    private Coroutine pulseRoutine;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        // Flash invisible au début
        if (flashGroup != null)
            flashGroup.alpha = 0f;
    }

    // Appelé par ton DialogueTrigger
    public void Activate()
    {
        if (isActive) return;

        isActive = true;
        pulseRoutine = StartCoroutine(PulseEffect());
    }

    // Appelé quand le joueur quitte la box ou ferme le dialogue
    public void Deactivate()
    {
        isActive = false;

        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        // Reset scale
        transform.localScale = originalScale;

        // Reset flash
        if (flashGroup != null)
            flashGroup.alpha = 0f;
    }

    IEnumerator PulseEffect()
    {
        float t = 0f;

        while (isActive)
        {
            // Animation de scale (respiration)
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(t) + 1f) * 0.5f);
            transform.localScale = originalScale * scale;

            // Flash subtil
            if (flashGroup != null)
            {
                float flash = Mathf.Lerp(0f, flashIntensity, (Mathf.Sin(t * flashSpeed) + 1f) * 0.5f);
                flashGroup.alpha = flash;
            }

            t += Time.deltaTime * speed;
            yield return null;
        }
    }
}
