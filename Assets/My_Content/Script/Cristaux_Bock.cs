using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool player = false;
    public ParticleSystem interactFX;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player && Input.GetMouseButtonDown(0))
        {

            anim.SetTrigger("destroy");

            interactFX.Play();

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


