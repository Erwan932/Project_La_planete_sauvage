using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // ← IMPORTANT pour charger une scène

public class CrowdManager : MonoBehaviour
{
    public List<FollowerAI> recruitableFollowers;
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();
    public List<FollowerAI> SavedFollowers = new List<FollowerAI>();
    public float followDistance = 0.5f;
    public FollowerAI nearbyFollower;
    public bool playerIsHidden = false;
    public int maxFollowers = 2;



    public void SetHidden(bool state)
    {
        playerIsHidden = state;
    }

    public bool IsPlayerHidden()
    {
        return playerIsHidden;
    }

    void Update()
    {
        // Recruter le follower proche si appui sur E
        if (Input.GetButtonDown("Fire3"))
            TryRecruitNearbyFollower();

        // Décrémenter la foule si le joueur prend des dégâts
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFollowers();
    }

public void TryRecruitNearbyFollower()
{
    if (nearbyFollower == null)
    {
        Debug.Log("Aucun follower à proximité");
        return;
    }

    if (activeFollowers.Count >= maxFollowers)
    {
        Debug.Log("Limite atteinte : 2 followers maximum");
        return;
    }

    if (recruitableFollowers.Contains(nearbyFollower))
    {
        nearbyFollower.tooltip.SetActive(false);

        recruitableFollowers.Remove(nearbyFollower);
        activeFollowers.Add(nearbyFollower);

        nearbyFollower.gameObject.SetActive(true);
        nearbyFollower = null;

        Debug.Log("Follower recruté !");
    }
}


    public void TakeDamage()
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
            SceneManager.LoadScene("Menu_Mort"); // ← Chargement de la scène Game Over
        }
    }

    public void UpdateFollowers()
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
