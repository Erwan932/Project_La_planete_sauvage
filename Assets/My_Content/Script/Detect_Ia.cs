using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriangleKill : MonoBehaviour
{
    public float blinkDuration = 2f;
    public float blinkInterval = 0.2f;

    private SpriteRenderer sr;
    private Coroutine blinkCoroutine;

    private bool playerInside = false;
    private bool iaInside = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IA détectée → priorité absolue
        if (other.CompareTag("ia"))
        {
            iaInside = true;
            Debug.Log("IA détectée : le triangle est désactivé 🤖");

            StopTriangle();
            return;
        }

        // Player détecté seulement si aucune IA
        if (other.CompareTag("Player") && !playerInside && !iaInside)
        {
            playerInside = true;
            Debug.Log("Player détecté 🔺");

            blinkCoroutine = StartCoroutine(BlinkAndReload());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ia"))
        {
            iaInside = false;
            Debug.Log("IA sortie : le triangle peut se réactiver");

            return;
        }

        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player sorti ❌");

            StopTriangle();
        }
    }

    void StopTriangle()
    {
        playerInside = false;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (sr != null)
            sr.color = Color.white;
    }

    IEnumerator BlinkAndReload()
    {
        Color originalColor = sr.color;
        float timer = 0f;

        while (timer < blinkDuration && playerInside && !iaInside)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(blinkInterval);

            sr.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval * 2;
        }

        // Mort seulement si le player est encore dedans ET aucune IA
        if (playerInside && !iaInside)
        {
            Debug.Log("Le joueur est mort ☠️");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
