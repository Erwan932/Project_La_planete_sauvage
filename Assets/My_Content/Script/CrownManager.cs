using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject followerPrefab;
    public float spawnRadius = 1.5f;

    private List<GameObject> crowd = new List<GameObject>();

    void Start()
    {
        crowd.Add(gameObject); // Le player compte comme un membre
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddFollower();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }
    }

    public void AddFollower()
    {
        // Position aléatoire autour du joueur
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        GameObject newFollower = Instantiate(followerPrefab, spawnPos, Quaternion.identity);

        // On donne au follower la référence du joueur
        newFollower.GetComponent<FollowerAI>().SetTarget(transform);

        crowd.Add(newFollower);
    }

    public void TakeDamage()
    {
        if (crowd.Count > 1)
        {
            // Retirer le dernier follower ajouté
            GameObject lostFollower = crowd[crowd.Count - 1];
            crowd.RemoveAt(crowd.Count - 1);
            Destroy(lostFollower);
        }
        else
        {
            Debug.Log("GAME OVER : plus de membres !");
            // ici tu peux lancer ta scène Game Over
        }
    }

    public int GetCrowdSize()
    {
        return crowd.Count;
    }
}