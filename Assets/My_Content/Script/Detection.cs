using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    public float interval;           // Temps entre deux scans
    public float flashTime;          // Durée totale du scan
    public float startFlashRate = 0.4f; // Intervalle entre flashs au début
    public float endFlashRate = 0.05f;  // Intervalle juste avant dégâts
    public Image redOverlay;         // UI rouge
    public CrowdManager crowdManager;

    private bool scanInProgress = false;

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

        // Petit délai pour laisser passer les triggers
        yield return new WaitForSeconds(0.05f);

        while (timer < flashTime)
        {
            // Pourcentage du temps écoulé
            float t = timer / flashTime;

            // On interpole la vitesse du flash : de lent à rapide
            float currentFlashRate = Mathf.Lerp(startFlashRate, endFlashRate, t);

            // Inversion état overlay
            overlayOn = !overlayOn;
            SetOverlayVisibility(overlayOn);

            yield return new WaitForSeconds(currentFlashRate);
            timer += currentFlashRate;
        }

        // Dégâts à la fin
        if (!IsGroupHidden())
            crowdManager.TakeDamage();

        CloseScan();
        scanInProgress = false;
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


