using UnityEngine;
using UnityEngine.SceneManagement;

public class TriangleKillZone : MonoBehaviour
{
    [Header("Réglages")]
    public float timeToKill = 2f;
    public float blinkSpeed = 6f;

    [Header("Layers")]
    public string cloudLayerName = "Cloud";

    private float timer = 0f;
    private bool playerInside = false;
    private bool cloudInside = false;

    private SpriteRenderer sprite;
    private Color originalColor;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player entre
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            timer = 0f;
        }

        // Cloud touche le triangle
        if (other.gameObject.layer == LayerMask.NameToLayer(cloudLayerName))
        {
            cloudInside = true;
            sprite.enabled = false; // invisible
            timer = 0f;            // 🔴 STOP attaque
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Player sort
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;
            ResetSprite();
        }

        // Cloud sort
        if (other.gameObject.layer == LayerMask.NameToLayer(cloudLayerName))
        {
            cloudInside = false;
            sprite.enabled = true; // redevient visible
            ResetSprite();
        }
    }

    void Update()
    {
        // 🔒 Cloud présent → AUCUNE attaque
        if (cloudInside)
        {
            timer = 0f;
            ResetSprite();
            return;
        }

        // Player détecté → attaque
        if (playerInside)
        {
            timer += Time.deltaTime;

            float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            sprite.color = Color.Lerp(originalColor, Color.red, t);

            if (timer >= timeToKill)
            {
                KillPlayer();
            }
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player tué par le triangle !");
        SceneManager.LoadScene("Menu_Mort");
    }

    void ResetSprite()
    {
        sprite.color = originalColor;
    }
}
