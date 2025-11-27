using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    public float interval = 20f;        // Temps entre deux scans
    public float flashSpeed = 0.2f;     // Vitesse du clignotement
    public float flashTime = 3f;        // Durée totale du scan

    public Image redOverlay;            // Image UI rouge (alpha = 0)
    public CrowdManager crowdManager;

    private bool scanInProgress = false;

    void Start()
    {
        StartCoroutine(DetectionRoutine());
        SetOverlayVisibility(false);
        redOverlay.gameObject.SetActive(false);
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            // Attente avant scan
            yield return new WaitForSeconds(interval);

            // Activation du scan
            yield return StartCoroutine(ScanSequence());
        }
    }

    IEnumerator ScanSequence()
    {
        scanInProgress = true;

        float timer = 0f;
        bool overlayOn = false;

        // On active l'objet (mais pas encore visible)
        redOverlay.gameObject.SetActive(true);

        // Le scan clignote pendant X secondes OU jusqu'au dégât
        while (timer < flashTime)
        {
            timer += Time.deltaTime;

            // Alternance on/off
            overlayOn = !overlayOn;
            SetOverlayVisibility(overlayOn);

            // Attendre un petit peu entre les flashes
            yield return new WaitForSeconds(flashSpeed);

            // Vérifier si le groupe est caché (si pas caché → dégâts)
            if (!IsGroupHidden())
            {
                crowdManager.TakeDamage();

                // On coupe immédiatement la zone
                CloseScan();
                scanInProgress = false;
                yield break; // Fin du scan + on relance l'intervalle
            }
        }

        // Personne n'a été touché → on referme la zone
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
        // Joueur visible = non caché
        if (!crowdManager.playerIsHidden)
            return false;

        // Followers visibles = non cachés
        foreach (var f in crowdManager.activeFollowers)
        {
            if (!f.IsHidden)
                return false;
        }

        return true;
    }
}


