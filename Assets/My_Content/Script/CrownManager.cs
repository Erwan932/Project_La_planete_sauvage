using System.Collections.Generic;
using UnityEngine;


public class CrowdManager : MonoBehaviour
{
    public List<FollowerAI> recruitableFollowers;    // Followers à recruter
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();  // Followers actifs
    public float followDistance = 0.5f;   // Distance minimale entre les followers

    private FollowerAI nearbyFollower; // Follower à portée pour recruter

    void Update()
    {
        // Recruter le follower proche si appui sur E
        if (Input.GetKeyDown(KeyCode.E))
            TryRecruitNearbyFollower();

        // Décrémenter la foule si le joueur prend des dégâts
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFollowers();
    }

    void TryRecruitNearbyFollower()
    {
        if (nearbyFollower == null)
        {
            Debug.Log("Aucun follower à proximité");
            return;
        }

        if (recruitableFollowers.Contains(nearbyFollower))
        {
            Debug.Log("Follower recruté !");
            Physics2D.IgnoreCollision(nearbyFollower.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            recruitableFollowers.Remove(nearbyFollower);
            nearbyFollower.gameObject.SetActive(true);
            activeFollowers.Add(nearbyFollower);
            nearbyFollower = null;
            
        }
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

    void UpdateFollowers()
    {
        Vector2 previousPos = transform.position;

        for (int i = 0; i < activeFollowers.Count; i++)
        {
            FollowerAI follower = activeFollowers[i];

            Vector2 direction = previousPos - (Vector2)follower.transform.position;
            float distance = direction.magnitude;

            Vector2 targetPos = follower.transform.position;
            if (distance > followDistance)
            {
                targetPos = (Vector2)follower.transform.position + direction.normalized * (distance - followDistance);
            }

            follower.SetFormationPosition(targetPos);
            previousPos = follower.transform.position;
        }
    }

    // Appelé depuis le FollowerAI quand le joueur entre dans le trigger
    public void SetNearbyFollower(FollowerAI follower)
    {
        nearbyFollower = follower;
    }

    // Appelé depuis le FollowerAI quand le joueur sort du trigger
    public void ClearNearbyFollower(FollowerAI follower)
    {
        if (nearbyFollower == follower)
            nearbyFollower = null;
    }
}
