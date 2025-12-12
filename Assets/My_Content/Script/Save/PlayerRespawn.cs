using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPosition;

    private void Start()
    {
        // Si on a déjà un checkpoint sauvegardé (après mort → reload)
        if (CheckpointData.hasSavedPosition)
        {
            spawnPosition = CheckpointData.savedPosition;
            transform.position = spawnPosition;
        }
        else
        {
            // Sinon, première position du joueur
            spawnPosition = transform.position;
            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activation d'un checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            spawnPosition = transform.position;

            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;

            Debug.Log("Checkpoint activé : " + spawnPosition);
        }

        // Mort du joueur
        if (other.CompareTag("Trap"))
        {
            DieAndRespawn();
        }
    }

    private void DieAndRespawn()
    {
        Debug.Log("Le joueur est mort -> reload de la scène");

        // Recharge la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
