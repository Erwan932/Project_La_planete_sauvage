using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractableObject : MonoBehaviour
{
    private bool player = false;

    [Header("FX & Anim")]
    public ParticleSystem interactFX;
    private Animator anim;

    [Header("Audio")]
    public AudioClip destroySound;    // Le son à jouer
    public float soundVolume = 1f;    // Volume du son (tu peux mettre >1)
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Configurer AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = soundVolume;
    }

    void Update()
    {
        if (player && Input.GetButtonDown("Fire3"))
        {
            // Jouer animation
            if (anim != null)
                anim.SetTrigger("destroy");

            // Jouer particules
            if (interactFX != null)
                interactFX.Play();

            // Jouer le son via AudioSource
            if (destroySound != null && audioSource != null)
            {
                audioSource.clip = destroySound;
                audioSource.volume = soundVolume; // Ajustable >1 si nécessaire
                audioSource.Play();
            }

            // Détruire l'objet après 1 seconde
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = true;
            if (anim != null)
                anim.SetBool("isPlayerNear", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = false;
            if (anim != null)
                anim.SetBool("isPlayerNear", false);
        }
    }
}
