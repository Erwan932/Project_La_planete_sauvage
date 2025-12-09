using UnityEngine;

public class HidingFromDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<CrowdManager>().SetHidden(true);
        }

        FollowerAI follower = col.GetComponent<FollowerAI>();
        if (follower != null)
        {
            follower.IsHidden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<CrowdManager>().SetHidden(false);
        }

        FollowerAI follower = col.GetComponent<FollowerAI>();
        if (follower != null)
        {
            follower.IsHidden = false;
        }
    }
}

