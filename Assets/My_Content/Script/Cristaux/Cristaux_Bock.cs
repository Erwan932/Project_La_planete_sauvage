using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool player = false;
    public ParticleSystem interactFX;
    private Animator anim;

    [Header("Audio")]
    public AudioClip destroySound;    // Le son que tu glisseras dans Unity
    public float soundVolume = 1f;    // Volume du son

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player && Input.GetButtonDown("Fire3"))
        {
            anim.SetTrigger("destroy");

            if (interactFX != null)
                interactFX.Play();

            // 🎵 Jouer le son SANS AudioSource
            if (destroySound != null)
                AudioSource.PlayClipAtPoint(destroySound, transform.position, soundVolume);

            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = true;
            anim.SetBool("isPlayerNear", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = false;
            anim.SetBool("isPlayerNear", false);
        }
    }
}
