using UnityEngine;

public class FollowerTriggerBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet qui entre est un FollowerAI
        FollowerAI follower = other.GetComponent<FollowerAI>();
        if (follower != null)
        {
            // Récupère le CrowdManager dans la scène
            CrowdManager crowdManager = FindObjectOfType<CrowdManager>();
            if (crowdManager != null)
            {
                // Supprime le follower de la liste
                crowdManager.RemoveRecruitableFollower(follower);

                Debug.Log("Follower éliminé par la Trigger Box");
            }
        }
    }
}
