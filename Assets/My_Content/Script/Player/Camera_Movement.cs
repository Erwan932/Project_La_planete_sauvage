using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    private Camera cam;

    [Header("Position")]
    public Vector3 offset;
    public float followSpeed = 3f;

    [Header("Zoom (Size)")]
    public float defaultSize = 5f;      // Taille au début
    public float zoomSpeed = 3f;        // Vitesse transition zoom

    private float targetSize;           // Taille désirée

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = defaultSize;
        cam.orthographicSize = defaultSize; // valeur initiale
    }

    private void Update()
    {
        // --- SUIVI DU JOUEUR ---
        Vector3 desiredPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // --- SMOOTH ZOOM ---
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
    }

    // Fonction appelée par une trigger pour changer le zoom
    public void SetCameraSize(float size)
    {
        targetSize = size;
    }
}
