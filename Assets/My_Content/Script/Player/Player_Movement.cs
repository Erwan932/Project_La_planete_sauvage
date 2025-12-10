using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 0f;
    public float jumpingPower = 0f;
    private bool isFacingRight = true;

    public ParticleSystem SmokeFX;
    public bool isGrounded = false;
    public bool canMove = true;

    [HideInInspector] public SpawnCristal currentCristal;

    public bool isCrouching = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animators;

    void Update()
    {
        if (canMove)
            horizontal = Input.GetAxisRaw("Horizontal");
        else
            horizontal = 0f;

        if (Input.GetButtonDown("Jump") && isGrounded && canMove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            SmokeFX.Play();
            isGrounded = false;
        }


   
        if (Input.GetButtonDown("Fire1"))
        {
            animators.SetTrigger("Attack");
        }

        if (!canMove && Input.GetButtonDown("Fire1") && currentCristal != null && currentCristal.Spawnobject != null)
        {
            StartCoroutine(currentCristal.BlinkAndDestroy(currentCristal.Spawnobject, 0.5f, 0.1f));
        }

        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.C)) isCrouching = true;
            if (Input.GetKeyUp(KeyCode.C)) isCrouching = false;
        }
        else
        {
            isCrouching = false;
        }

        UpdateAnimations();

        if (canMove)
            Flip();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
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
        animators.SetBool("IsRunning", horizontal != 0);
        animators.SetBool("IsJumping", !isGrounded);
        animators.SetBool("IsCrouching", isCrouching);
    }

    public void EndAttack()
    {
        animators.ResetTrigger("Attack");
    }
}
