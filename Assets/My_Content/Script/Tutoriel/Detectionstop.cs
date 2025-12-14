using UnityEngine;

public class TriggerStopDetection : MonoBehaviour
{
    [Header("Detection Reference")]
    public DetectionZone detectionZone;

    [Header("Player Tag")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && detectionZone != null)
        {
            // Désactiver la détection
            detectionZone.detectionEnabled = false;

            // Remettre scan à zéro
            detectionZone.scanInProgress = false;

            // Masquer l’overlay
            if (detectionZone.redOverlay != null)
                detectionZone.redOverlay.gameObject.SetActive(false);

            Debug.Log("Detection stoppée et remise à zéro !");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && detectionZone != null)
        {
            // Réactiver la détection
            detectionZone.detectionEnabled = true;
            Debug.Log("Detection réactivée !");
        }
    }
}
