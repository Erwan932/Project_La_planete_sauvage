using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public List<FollowerAI> recruitableFollowers;
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();

    public float formationRadius = 1f;  // Rayon du cercle

    void Start()
    {
        // Le joueur est le centre => pas dans la liste active
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            AddFollower();

        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFormation();
    }

    void AddFollower()
    {
        if (recruitableFollowers.Count == 0) return;

        FollowerAI follower = recruitableFollowers[0];
        recruitableFollowers.RemoveAt(0);

        follower.gameObject.SetActive(true);
        follower.SetTarget(transform);

        activeFollowers.Add(follower);
    }

    void TakeDamage()
    {
        if (activeFollowers.Count > 0)
        {
            FollowerAI lost = activeFollowers[activeFollowers.Count - 1];
            activeFollowers.RemoveAt(activeFollowers.Count - 1);

            lost.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("GAME OVER");
        }
    }

    void UpdateFormation()
    {
        if (activeFollowers.Count == 0) return;

        float angleStep = 360f / activeFollowers.Count;

        for (int i = 0; i < activeFollowers.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector2 formationPos = new Vector2(
                transform.position.x + Mathf.Cos(angle) * formationRadius,
                transform.position.y + Mathf.Sin(angle) * formationRadius
            );

            activeFollowers[i].SetFormationPosition(formationPos);
        }
    }
}