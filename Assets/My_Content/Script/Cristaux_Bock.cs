using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool player = false;
    public ParticleSystem interactFX;

    void Update()
    {
        if (player && Input.GetMouseButtonDown(0))
        {
        
            interactFX.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = false;
        }
    }
}
