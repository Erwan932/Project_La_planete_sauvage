using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerWithBlink : MonoBehaviour
{
    public string playerTag = "Player";
    public float timeToKill = 2f;
    public float blinkSpeed = 8f;

    private float timer = 0f;
    private bool playerInside = false;

    private SpriteRenderer triangleSR;
    private Color originalColor;

    void Start()
    {
        // Récupère le SpriteRenderer du Triangle
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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            timer = 0f;

            // Arrête le clignotement → couleur normale
            if (triangleSR != null)
                triangleSR.color = originalColor;
        }
    }

    void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;

            // Clignotement du Triangle
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
