using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FollowerAI))]
public class FollowerMovementSync : MonoBehaviour
{
    public Transform player; // le joueur à suivre
    public float speed = 2f;

    private Animator animator;
    private bool isFacingRight = true;
    private FollowerAI ai;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ai = GetComponent<FollowerAI>();
    }

    private void Update()
    {
        if (ai != null && ai.inFormation)
        {
            // Suivre la position de formation
            transform.position = Vector2.MoveTowards(transform.position, ai.targetPosition, speed * Time.deltaTime);

            // Copier directement les animations du joueur
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                animator.SetBool("IsRunning", Mathf.Abs(playerMovement.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.01f);
                animator.SetBool("IsJumping", !playerMovement.isGrounded);
                animator.SetBool("IsCrouching", playerMovement.isCrouching);
            }

            // Copier le flip en temps réel
            CopyFlip();
        }
        else
        {
            // Idle si pas dans la formation
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsCrouching", false);
        }
    }

    private void CopyFlip()
    {
        bool playerFacingRight = player.localScale.x > 0;
        if (playerFacingRight != isFacingRight)
        {
            isFacingRight = playerFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }
}

