using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrowdManager crowd = FindFirstObjectByType<CrowdManager>();

        // --- Si un follower entre ---
        FollowerAI follower = collision.GetComponent<FollowerAI>();
        if (follower != null)
        {
            if (crowd.activeFollowers.Contains(follower))
            {
                crowd.activeFollowers.Remove(follower);
                crowd.SavedFollowers.Add(follower);

                // ARRÊTE le mouvement uniquement pour ce follower
                follower.StopFollowing();  // => inFormation = false
                follower.targetPosition = follower.transform.position;

                Debug.Log("Follower déposé dans la DropZone !");
            }

            CheckWin(crowd);
            return;
        }

        // --- Si le joueur entre ---
        if (collision.CompareTag("Player"))
        {
            CheckWin(crowd);
        }
    }

    private void CheckWin(CrowdManager crowd)
    {
        if (crowd.recruitableFollowers.Count == 0 &&
            crowd.activeFollowers.Count == 0)
        {
            Debug.Log("WIN !!! Tous les followers ont été sauvés !");
        }
    }
}

