using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    private Vector2 targetPosition;
    public float followSpeed = 2f;
    private bool inFormation = false;
    public bool IsHidden = false;
    public GameObject tooltip;


    void OnEnable()
    {
        targetPosition = transform.position;
        inFormation = false;
    }

    void Update()
    {
        if (inFormation)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );
        }
    }

    public void SetFormationPosition(Vector2 pos)
    {
        targetPosition = pos;
        inFormation = true;
    }

   private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        tooltip.SetActive(true); // Affiche le tooltip

        CrowdManager manager = collision.GetComponent<CrowdManager>();
        if (manager != null)
        {
            manager.SetNearbyFollower(this);
        }

        Debug.Log("Follower à proximité !");
    }
}

    private void OnTriggerExit2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        tooltip.SetActive(false); // Masque le tooltip

        CrowdManager manager = collision.GetComponent<CrowdManager>();
        if (manager != null)
        {
            manager.ClearNearbyFollower(this);
        }

        Debug.Log("Follower hors de portée !");
    }
}

}

