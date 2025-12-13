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

    [Header("UI Recrutement")]
    public GameObject recruitUI; // UI assignable dans l'inspecteur
    private bool hasBeenRecruited = false; // pour empêcher la réapparition

    [Header("Objet à détruire après recrutement")]
    public GameObject objectToDestroy; // assignable dans l'inspecteur
    [Header("Feedback Recrutement")]
    public ParticleSystem joinParticlesPrefab; // Prefab de particules



    // Ajouter : état une fois déposé
    public bool isDropped = false;

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
        isDropped = false; // Remis à zéro si l'objet est réactivé
    }

    private void Update()
    {
        // Si le follower a été déposé → il ne bouge plus
        if (isDropped)
        {
            SetIdle();
            return;
        }

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

    // APPELÉ UNIQUEMENT PAR LA DROPZONE
    public void StopFollowing()
    {
        isDropped = true;         // ne doit plus suivre
        inFormation = false;      // désactive la formation automatiquement
        targetPosition = transform.position; // fixe où il est
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

    private void CopyFlip()
    {
        if (player == null) return;

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm == null) return;

        bool playerFacingRight = pm.IsFacingRight();

        if (playerFacingRight != isFacingRight)
        {
            isFacingRight = playerFacingRight;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }

    // --- TRIGGER : signalement au CrowdManager ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CrowdManager manager = collision.GetComponent<CrowdManager>();
            if (manager != null)
                manager.SetNearbyFollower(this);

            Debug.Log("Follower à proximité !");

            // --- AFFICHAGE UI ---
            if (!hasBeenRecruited && recruitUI != null)
                recruitUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CrowdManager manager = collision.GetComponent<CrowdManager>();
            if (manager != null)
                manager.ClearNearbyFollower(this);

            Debug.Log("Follower hors de portée !");

            // --- CACHER UI quand le joueur sort de la zone ---
            if (!hasBeenRecruited && recruitUI != null)
                recruitUI.SetActive(false);
        }
    }

    // Méthode publique pour signaler le recrutement
    public void OnRecruited()
    {
        hasBeenRecruited = true;

        // Désactiver UI définitivement
        if (recruitUI != null)
            recruitUI.SetActive(false);

        // Détruire l'objet assigné dans l'inspecteur
        if (objectToDestroy != null)
            Destroy(objectToDestroy);

        if (joinParticlesPrefab != null)
        {
            ParticleSystem particles = Instantiate(joinParticlesPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            particles.Play();

            // Détruire automatiquement après la durée
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }
}
