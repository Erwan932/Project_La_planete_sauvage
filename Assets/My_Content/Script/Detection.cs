using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
public float interval;        // Temps entre deux scans
public float flashSpeed;     // Vitesse du clignotement
public float flashTime;        // Durée totale du scan
public Image redOverlay;            // Image UI rouge (alpha = 0)  
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

    // Petit délai pour que les triggers aient le temps d'être détectés  
    yield return new WaitForSeconds(0.05f);  

    while (timer < flashTime)  
    {  
        timer += flashSpeed;  
        overlayOn = !overlayOn;  
        SetOverlayVisibility(overlayOn);  

        yield return new WaitForSeconds(flashSpeed);  
    }  

    // À la fin du flash, appliquer le dégât si le groupe n'est pas caché  
    if (!IsGroupHidden())  
    {  
        crowdManager.TakeDamage();  
    }  

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
        if (f == null || !f.IsHidden)  
            return false;  
    }  

    return true;  
}  
}

