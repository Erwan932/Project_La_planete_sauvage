using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FollowerAI : MonoBehaviour
{
    [Header("Références")]
    public Transform player;

    [Header("Formation")]
    public Vector2 targetPosition;
    public float followSpeed = 2f;
    public bool inFormation = false;

    [Header("État du follower")]
    public bool IsHidden = false;
    public GameObject tooltip;

    // Animation & flip
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        targetPosition = transform.position;
        inFormation = false;
    }

    private void Update()
    {
        if (inFormation)
        {
            HandleMovement();
            CopyAnimationFromPlayer();
            CopyFlip();
        }
        else
        {
            SetIdle();
        }
    }

    // MOUVEMENT 
    private void HandleMovement()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

    public void SetFormationPosition(Vector2 pos)
    {
        targetPosition = pos;
        inFormation = true;
    }

    // ANIMATIONS 
    private void CopyAnimationFromPlayer()
    {
        if (player == null) return;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();

            animator.SetBool("IsRunning", Mathf.Abs(rb.linearVelocity.x) > 0.01f);
            animator.SetBool("IsJumping", !playerMovement.isGrounded);
            animator.SetBool("IsCrouching", playerMovement.isCrouching);
        }
    }

    private void SetIdle()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsCrouching", false);
    }

    // --- FLIP ---
    private void CopyFlip()
    {
        if (player == null) return;

        bool playerFacingRight = player.localScale.x > 0;

        if (playerFacingRight != isFacingRight)
        {
            isFacingRight = playerFacingRight;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }

    // --- TRIGGER : tooltip & signalement au CrowdManager ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tooltip.SetActive(true);

            CrowdManager manager = collision.GetComponent<CrowdManager>();
            if (manager != null)
                manager.SetNearbyFollower(this);

            Debug.Log("Follower à proximité !");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tooltip.SetActive(false);

            CrowdManager manager = collision.GetComponent<CrowdManager>();
            if (manager != null)
                manager.ClearNearbyFollower(this);

            Debug.Log("Follower hors de portée !");
        }
    }
}
