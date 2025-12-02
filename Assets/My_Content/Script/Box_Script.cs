using UnityEngine;

public class Box_Script : MonoBehaviour
{
    private Animator anim;
    private bool playerIsInside = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Vérifie si on est dans l'animation Anim_Box
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // Quand Anim_Box est finie → passer à Anim_Box_Kill
        if (state.IsName("Anim_Box") && state.normalizedTime >= 1f)
        {
            anim.SetTrigger("IsStayInTrigger");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInside = true;
            anim.SetBool("IsTrigger", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInside = false;
            anim.SetBool("IsTrigger", false);
        }
    }
}

