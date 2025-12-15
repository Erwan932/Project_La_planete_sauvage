using UnityEngine;

public class FollowerRemoveTrigger : MonoBehaviour
{
    [SerializeField] private CrowdManager crowdManager;

    void Awake()
    {
        if (crowdManager == null)
            Debug.LogError("CrowdManager non assigné dans l’Inspector !");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        FollowerAI follower = other.GetComponent<FollowerAI>();
        if (follower == null) return;

        crowdManager.RemoveRecruitableFollower(follower);
    }
}
