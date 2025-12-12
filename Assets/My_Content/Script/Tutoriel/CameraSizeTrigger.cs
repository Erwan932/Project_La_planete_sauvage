using UnityEngine;

public class CameraSizeTrigger : MonoBehaviour
{
    public float newSize = 7f; // taille que tu veux appliquer
    public Camera_Movement camScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        camScript.SetCameraSize(newSize);
    }
}
