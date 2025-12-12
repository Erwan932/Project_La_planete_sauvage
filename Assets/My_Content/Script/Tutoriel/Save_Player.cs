using UnityEngine;

public class RespawnableObject : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        // On sauvegarde la position de départ
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetObject()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
