using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 savedPosition;   // Position sauvegardée
    private bool hasSavedPosition;   // Pour savoir si un checkpoint a été activé

    private void Start()
    {
        // Au début, on sauvegarde la position initiale du joueur
        savedPosition = transform.position;
        hasSavedPosition = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si le joueur entre dans une Trigger Box avec le tag "Checkpoint"
        if (other.CompareTag("Checkpoint"))
        {
            savedPosition = transform.position;
            hasSavedPosition = true;
            Debug.Log("Checkpoint atteint ! Position sauvegardée : " + savedPosition);
        }

        // Si le joueur touche un objet avec le tag "Trap"
        if (other.CompareTag("Trap"))
        {
            DieAndRespawn();
        }
    }

    private void DieAndRespawn()
    {
        Debug.Log("Le joueur est mort !");

        if (hasSavedPosition)
        {
            // On replace le joueur à la dernière position sauvegardée
            transform.position = savedPosition;
            Debug.Log("Respawn au checkpoint : " + savedPosition);
        }
        else
        {
            // Si aucun checkpoint n'a été activé, respawn à la position initiale
            transform.position = Vector3.zero;
            Debug.Log("Respawn à la position par défaut (0,0)");
        }
    }
}
