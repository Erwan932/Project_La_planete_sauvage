using UnityEngine;

public class FallingRock2D : MonoBehaviour
{
    public float fallGravity = 6f;

    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool isFrozen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // IMPORTANT : la roche est figée au départ
        rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    // Appelé par ton trigger
    public void DropRock()
    {
        if (hasFallen)
            return;

        hasFallen = true;

        // On active la physique AU MOMENT DE LA CHUTE
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFrozen)
            return;

        if (collision.gameObject.CompareTag("Rock"))
        {
            isFrozen = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;

            // Refige définitivement
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
