using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    [Header("Scan Settings")]
    public float interval;
    public float flashTime;
    public float startFlashRate = 0.4f;
    public float endFlashRate = 0.05f;
    public Image redOverlay;
    public CrowdManager crowdManager;

    [Header("Hand Animation")]
    public Animator handAnimator;
    public string attackTrigger = "Attack";
    public string handUpTrigger = "handUp";

    private bool scanInProgress = false;
    private bool elementScanned = false;

    void Start()
    {
        redOverlay.gameObject.SetActive(false);
        StartCoroutine(DetectionRoutine());
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return StartCoroutine(ScanSequence());
        }
    }

    IEnumerator ScanSequence()
    {
        scanInProgress = true;
        redOverlay.gameObject.SetActive(true);

        float timer = 0f;
        bool overlayOn = false;

        // petit délai avant le scan
        yield return new WaitForSeconds(0.05f);

        while (timer < flashTime)
        {
            float t = timer / flashTime;
            float currentFlashRate = Mathf.Lerp(startFlashRate, endFlashRate, t);

            overlayOn = !overlayOn;
            SetOverlayVisibility(overlayOn);

            yield return new WaitForSeconds(currentFlashRate);
            timer += currentFlashRate;
        }

        CloseScan();

        // Détection
        elementScanned = !IsGroupHidden();

        if (elementScanned)
            yield return StartCoroutine(PlayHandAnimation());

        scanInProgress = false;
    }

    IEnumerator PlayHandAnimation()
    {
        // 1. Lancer l’animation Attack
        handAnimator.SetTrigger(attackTrigger);

        bool animationStarted = false;
        float timeout = 2f;

        // Attendre l’entrée dans Attack OU timeout
        while (timeout > 0f)
        {
            if (handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animationStarted = true;
                break;
            }

            timeout -= Time.deltaTime;
            yield return null;
        }

        // Si l’anim a démarré → attendre la fin
        if (animationStarted)
        {
            while (handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                   handAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }

        // 2. Dégâts 
        crowdManager.TakeDamage();

        // 3. Animation de remontée
        handAnimator.SetTrigger(handUpTrigger);

        elementScanned = false;
    }

    void SetOverlayVisibility(bool visible)
    {
        Color c = redOverlay.color;
        c.a = visible ? 0.6f : 0f;
        redOverlay.color = c;
    }

    void CloseScan()
    {
        SetOverlayVisibility(false);
        redOverlay.gameObject.SetActive(false);
    }

    bool IsGroupHidden()
    {
        if (!crowdManager.playerIsHidden)
            return false;

        foreach (var f in crowdManager.activeFollowers)
        {
            if (f == null || !f.IsHidden)
                return false;
        }

        return true;
    }
}
