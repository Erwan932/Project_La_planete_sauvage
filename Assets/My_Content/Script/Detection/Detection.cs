using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    [Header("Scan Settings")]
    public float interval = 1f;
    public Image redOverlay;
    public float overlayFadeDuration = 0.5f;
    public CrowdManager crowdManager;

    [Header("Hand Animation")]
    public Animator handAnimator;
    public string attackTrigger = "Attack";
    public string handUpTrigger = "handUp";

    [Header("Eyes Animation")]
    public Animator eyesAnimator;
    public string eyesIdleTrigger = "Idle";
    public string eyesSpotTrigger = "Spot";
    public string eyesDeclineTrigger = "Decline";

    [HideInInspector] public bool scanInProgress = false;
    [HideInInspector] public bool detectionEnabled = true;
    [HideInInspector] public Coroutine detectionCoroutine;

    void Start()
    {
        if (redOverlay != null)
            redOverlay.gameObject.SetActive(false);
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Si détection désactivée ou joueur caché, skip
            if (!detectionEnabled || crowdManager.playerIsHidden)
                continue;

            if (!scanInProgress)
                yield return StartCoroutine(ScanSequence());
        }
    }

    IEnumerator ScanSequence()
    {
        scanInProgress = true;

        // Idle de Eyes
        eyesAnimator.SetTrigger(eyesIdleTrigger);
        yield return new WaitForSeconds(0.1f);

        // Spot de Eyes
        eyesAnimator.SetTrigger(eyesSpotTrigger);
        yield return StartCoroutine(WaitForAnimation(eyesAnimator, "Spot"));

        // Overlay apparaît puis disparaît
        yield return StartCoroutine(FadeOverlay(true, overlayFadeDuration));
        yield return StartCoroutine(FadeOverlay(false, overlayFadeDuration));

        // Hand Attack + HandUp seulement si le joueur n’est pas caché
        if (!crowdManager.playerIsHidden)
        {
            handAnimator.SetTrigger(attackTrigger);
            yield return StartCoroutine(WaitForAnimation(handAnimator, "Attack"));

            crowdManager.TakeDamage();

            handAnimator.SetTrigger(handUpTrigger);
            yield return StartCoroutine(WaitForAnimation(handAnimator, "handUp"));
        }

        // Decline de Eyes
        eyesAnimator.SetTrigger(eyesDeclineTrigger);
        yield return StartCoroutine(WaitForAnimation(eyesAnimator, "Decline"));

        // Retour à Idle de Eyes
        eyesAnimator.SetTrigger(eyesIdleTrigger);

        scanInProgress = false;
    }

    IEnumerator WaitForAnimation(Animator animator, string stateName)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    IEnumerator FadeOverlay(bool show, float duration)
    {
        if (redOverlay == null)
            yield break;

        redOverlay.gameObject.SetActive(true);
        float startAlpha = redOverlay.color.a;
        float endAlpha = show ? 0.6f : 0f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            Color c = redOverlay.color;
            c.a = alpha;
            redOverlay.color = c;
            yield return null;
        }

        Color finalColor = redOverlay.color;
        finalColor.a = endAlpha;
        redOverlay.color = finalColor;

        if (!show)
            redOverlay.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && detectionCoroutine == null)
            detectionCoroutine = StartCoroutine(DetectionRoutine());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && detectionCoroutine != null)
        {
            StopCoroutine(detectionCoroutine);
            detectionCoroutine = null;
            redOverlay.gameObject.SetActive(false);
            scanInProgress = false;
        }
    }
}
