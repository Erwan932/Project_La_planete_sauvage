using UnityEngine;

public class FollowerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 0f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private ParticleSystem SmokeFX;
    private bool isGrounded = false;
    private bool canMove = true;
    private SpawnCristal spawnCristal; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animators;


    void Update()
    {
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            UpdateAnimations();
        }
        Flip();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
    }
}
