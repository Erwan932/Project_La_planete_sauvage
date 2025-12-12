using UnityEngine;

public class CameraSizeTrigger : MonoBehaviour
{
    [Header("Taille de caméra à appliquer")]
    public float cameraSize = 7f;           // Taille spécifique pour ce trigger
    public Camera_Movement camScript;       // Script de la caméra à contrôler

    private static CameraSizeTrigger currentTrigger; // Trigger actuel du joueur

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Si ce n'est pas le trigger actuel, on force la taille de la caméra
        if (currentTrigger != this)
        {
            ForceCameraSize();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Quand le joueur sort de ce trigger, on supprime la référence
        if (currentTrigger == this)
        {
            currentTrigger = null;
        }
    }

    private void ForceCameraSize()
    {
        if (camScript != null)
        {
            camScript.SetCameraSize(cameraSize); // Force la taille
            currentTrigger = this;               // Déclare ce trigger comme actif
        }
    }
}
