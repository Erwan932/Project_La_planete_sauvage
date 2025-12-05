using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerWithBlink : MonoBehaviour
{
    public string playerTag = "Player";
    public float timeToKill = 2f;
    public float blinkSpeed = 8f;

    private float timer = 0f;
    private bool playerInside = false;
    private bool playerVisible = false;
    private GameObject player;
    private float colliderbound;
    private SpriteRenderer triangleSR;
    private Color originalColor;

    void Start()
    {
        colliderbound = gameObject.GetComponent<Collider2D>().bounds.max.y;
        triangleSR = GetComponent<SpriteRenderer>();
        if (triangleSR != null)
        {
            originalColor = triangleSR.color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            timer = 0f;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
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
        var vec = new Vector3(transform.position.x, colliderbound);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(vec, player.transform.position);
    }

    void Update()
    {
        if (playerInside)
        {
            var vec = new Vector3(transform.position.x, colliderbound-0.1f);
            RaycastHit2D hit= Physics2D.Raycast(vec, player.transform.position - vec, 100f);
            if (hit.collider != null && hit.collider.CompareTag(playerTag))
            {
                playerVisible = true;
            }
            else
            {
                playerVisible = false;
            }
            if (!playerVisible)
            {
                
                if (triangleSR != null)
                    triangleSR.color = originalColor;
                return;
            }
            timer += Time.deltaTime;

            
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
    }

    void KillPlayer()
    {
        Debug.Log("Le joueur est mort dans la zone !");
        SceneManager.LoadScene("Menu_Mort"); // Charge la scène de mort
    }

}
