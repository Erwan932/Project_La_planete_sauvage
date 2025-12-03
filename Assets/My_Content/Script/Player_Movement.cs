using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 0f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public ParticleSystem SmokeFX;
    private bool isGrounded = false;
    public bool canMove = true;
    public SpawnCristal spawnCristal;
    private bool isCrouching = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animators;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            SmokeFX.Play();
            isGrounded = false;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (!canMove && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(spawnCristal.BlinkAndDestroy (spawnCristal.Spawnobject, 0.5f, 0.1f));
        }

        {
            horizontal = Input.GetAxisRaw("Horizontal");
            UpdateAnimations();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            isCrouching = false;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return; 
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // le joueur ne bouge plus
            return;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
    private void UpdateAnimations()
    {
        bool isMoving = horizontal != 0;
        animators.SetBool("IsRunning", isMoving);
        animators.SetBool("IsJumping", !isGrounded);
        animators.SetBool("IsCrouching", isCrouching);
    }
}