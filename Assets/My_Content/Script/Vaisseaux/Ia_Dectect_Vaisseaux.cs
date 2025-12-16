using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerWithBlink : MonoBehaviour
{
    [Header("Settings")]
    public string followerLayerName = "Followers";
    public float timeToKill = 2f;
    public float blinkSpeed = 5f;

    private int followerLayer;
    private float timer = 0f;
    private bool playerInside = false;
    private GameObject player;
    private SpriteRenderer triangleSR;
    private Color originalColor;

    void Start()
    {
        // Récupère l'index du layer Followers
        followerLayer = LayerMask.NameToLayer(followerLayerName);

        triangleSR = GetComponent<SpriteRenderer>();
        if (triangleSR != null)
        {
            originalColor = triangleSR.color;
            // Rendre invisible au départ
            Color c = originalColor;
            c.a = 0f;
            triangleSR.color = c;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == followerLayer)
        {
            playerInside = true;
            player = other.gameObject;
            timer = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == followerLayer)
        {
            playerInside = false;
            player = null;
            timer = 0f;

            // Rendre invisible
            if (triangleSR != null)
            {
                Color c = originalColor;
                c.a = 0f;
                triangleSR.color = c;
            }
        }
    }

    void Update()
    {
        if (!playerInside || player == null)
            return;

        // Timer pour tuer le follower
        timer += Time.deltaTime;

        // Clignotement du sprite
        if (triangleSR != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            Color c = originalColor;
            c.a = alpha;
            triangleSR.color = c;
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
