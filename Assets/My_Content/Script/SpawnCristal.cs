using UnityEngine;

public class SpawnCristal : MonoBehaviour
{
    public Transform Spawnpoint;
    public GameObject Prefab;
    private bool IsSpawn = false;
    public PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpawn && collision.CompareTag("Player"))
        {
            Instantiate(Prefab, Spawnpoint.position, Spawnpoint.rotation);
            IsSpawn = true;
            player.canMove = false;
        }
    }
}
