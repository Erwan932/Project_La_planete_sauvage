using UnityEngine;

public class SpawnCristal : MonoBehaviour
{
    public Transform Spawnpoint;
    public GameObject Prefab;
    private bool IsSpawn = false;
    public PlayerMovement player;
    public GameObject Spawnobject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpawn && collision.CompareTag("Player"))
        {
           Spawnobject = Instantiate(Prefab, collision.transform.position, Quaternion.identity);
            IsSpawn = true;
            player.canMove = false;
        }
    }

}
