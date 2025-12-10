using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 0f;
    public float jumpingPower = 0f;
    private bool isFacingRight = true;

    public Transform visual;                // ⬅️ OBJET VISUEL QUI FLIP + SQUASH
    private Vector3 originalScale;

    public ParticleSystem SmokeFX;
    public bool isGrounded = false;
    public bool canMove = true;

    [HideInInspector] public SpawnCristal currentCristal;

    public bool isCrouching = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animators;
    [SerializeField] private TrailRenderer movementTrail; // drag le TrailRenderer du visual
    [SerializeField] private float trailMinTime = 0.05f;
    [SerializeField] private float trailMaxTime = 0.45f;
    [SerializeField] private float trailMinWidth = 0.15f;
    [SerializeField] private float trailMaxWidth = 0.5f;

    private bool wasGroundedLastFrame = true;

    // --- Gravité custom ---
    [Header("Jump Gravity Settings")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;


    void Start()
    {
        originalScale = visual.localScale;      // ⬅️ Important : scale du VISUEL
        if (movementTrail != null) movementTrail.emitting = false;

    }


    void Update()
    {
        if (canMove)
            horizontal = Input.GetAxisRaw("Horizontal");
        else
            horizontal = 0f;

        // SAUT
        if (Input.GetButtonDown("Jump") && isGrounded && canMove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            SmokeFX.Play();
            isGrounded = false;
        }

        // --- Squash & Stretch (montée) ---
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            visual.localScale = new Vector3(
                originalScale.x * (isFacingRight ? 0.85f : -0.85f),
                originalScale.y * 1.15f,
                originalScale.z
            );
        }

        // --- Retour pendant la chute ---
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            visual.localScale = Vector3.Lerp(
                visual.localScale,
                new Vector3(
                    originalScale.x * (isFacingRight ? 1 : -1),
                    originalScale.y,
                    originalScale.z
                ),
                Time.deltaTime * 10f
            );
        }

        // --- Squash à l’atterrissage ---
        if (!wasGroundedLastFrame && isGrounded)
        {
            visual.localScale = new Vector3(
                originalScale.x * (isFacingRight ? 1.2f : -1.2f),
                originalScale.y * 0.7f,
                originalScale.z
            );

            StartCoroutine(ResetScale());
        }

        wasGroundedLastFrame = isGrounded;


        // Attaque
        if (Input.GetButtonDown("Fire3"))
            animators.SetTrigger("Attack");

        if (!canMove && Input.GetButtonDown("Fire3") && currentCristal != null && currentCristal.Spawnobject != null)
            StartCoroutine(currentCristal.BlinkAndDestroy(currentCristal.Spawnobject, 0.5f, 0.1f));


        // Accroupi
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.C)) isCrouching = true;
            if (Input.GetKeyUp(KeyCode.C)) isCrouching = false;
        }
        else isCrouching = false;

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

        // --- Contrôle du TrailRenderer pour sensation de vitesse ---
    if (movementTrail != null)
    {
        float moveSpeed = Mathf.Abs(rb.linearVelocity.x); // vitesse réelle
        // mappe la vitesse sur [trailMinTime, trailMaxTime] et largeur
        float t = (speed > 0f) ? Mathf.Clamp01(moveSpeed / Mathf.Abs(speed)) : 0f;
        movementTrail.time = Mathf.Lerp(trailMinTime, trailMaxTime, t);
        movementTrail.widthMultiplier = Mathf.Lerp(trailMinWidth, trailMaxWidth, t);

        // n'émet que si on bouge horizontalement significativement
        movementTrail.emitting = moveSpeed > 0.05f && canMove;
    }


        // Gravité custom
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    // ----------- FLIP SUR L'OBJET VISUEL UNIQUEMENT ----------
    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;

            visual.localScale = new Vector3(
                originalScale.x * (isFacingRight ? 1 : -1),
                visual.localScale.y,
                visual.localScale.z
            );
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = true;
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


    // ---- Animation du retour à la forme ----
    private IEnumerator ResetScale()
    {
        float duration = 0.12f;
        float time = 0f;

        Vector3 startScale = visual.localScale;
        Vector3 targetScale = new Vector3(
            originalScale.x * (isFacingRight ? 1 : -1),
            originalScale.y,
            originalScale.z
        );

        while (time < duration)
        {
            visual.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        visual.localScale = targetScale;
    }
}
