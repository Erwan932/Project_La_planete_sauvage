using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerWithBlink : MonoBehaviour
{
    public string followerLayerName = "Followers";
    public float timeToKill = 0f;
    public float blinkSpeed = 0f;

    private int followerLayer;
    private float timer = 0f;
    private bool playerInside = false;
    private bool playerVisible = false;
    private GameObject player;
    private float colliderbound;
    private SpriteRenderer triangleSR;
    private Color originalColor;

    void Start()
    {
        // Récupère l'index du layer Followers
        followerLayer = LayerMask.NameToLayer(followerLayerName);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            colliderbound = col.bounds.max.y;

        triangleSR = GetComponent<SpriteRenderer>();
        if (triangleSR != null)
            originalColor = triangleSR.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == followerLayer)
        {
            playerInside = true;
            timer = 0f;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == followerLayer)
        {
            playerInside = false;
            timer = 0f;
            player = null;

            if (triangleSR != null)
                triangleSR.color = originalColor;
        }
    }

    void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
            return;

        float gizmoBound = col.bounds.max.y;
        Vector3 vec = new Vector3(transform.position.x, gizmoBound);

        if (player == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(vec, player.transform.position);
    }

    void Update()
    {
        if (!playerInside || player == null)
            return;

        var vec = new Vector3(transform.position.x, colliderbound - 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(vec, player.transform.position - vec, 100f);

        // Vérifie si le raycast touche un follower
        playerVisible = (hit.collider != null && hit.collider.gameObject.layer == followerLayer);

        if (!playerVisible)
        {
            if (triangleSR != null)
                triangleSR.color = originalColor;
            return;
        }

        timer += Time.deltaTime;

        // Effet clignotement
        if (triangleSR != null)
        {
            float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            triangleSR.color = Color.Lerp(originalColor, Color.red, t);
        }

        if (timer >= timeToKill)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("Un follower est mort dans la zone !");
        SceneManager.LoadScene("Menu_Mort");
    }
}
