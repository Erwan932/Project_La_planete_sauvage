using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPosition;

    private RespawnableObject[] respawnObjects;

    private void Awake()
    {
        // On récupère tous les objets respawnables de la scène
        respawnObjects = FindObjectsOfType<RespawnableObject>();
    }

    private void Start()
    {
        if (CheckpointData.hasSavedPosition)
        {
            spawnPosition = CheckpointData.savedPosition;
            transform.position = spawnPosition;
        }
        else
        {
            spawnPosition = transform.position;
            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            spawnPosition = other.transform.position;

            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;

            Debug.Log("Checkpoint activé : " + spawnPosition);
        }

        if (other.CompareTag("Trap"))
        {
            DieAndRespawn();
        }
    }

    private void DieAndRespawn()
    {
        Debug.Log("Respawn sans reload !");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // 1️⃣ Respawn du joueur

    }
}
