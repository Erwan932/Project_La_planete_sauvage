using UnityEngine;

public class Box_Script : MonoBehaviour
{
    private Animator anim;
    private bool playerIsInside = false;

    [Header("Gestion des alliés")]
    public CrowdManager crowdManager; 

    private bool allyLost = false; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        
        if (playerIsInside && state.IsName("Anim_Box") && state.normalizedTime >= 1f)
        {
            anim.SetTrigger("IsStayInTrigger");

            
            if (!allyLost && crowdManager != null)
            {
                crowdManager.TakeDamage(); 
                allyLost = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInside = true;
            anim.SetBool("IsTrigger", true);
            allyLost = false; // Reset si le joueur rentre à nouveau
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInside = false;
            anim.SetBool("IsTrigger", false);
            anim.ResetTrigger("IsStayInTrigger");
        }
    }
}
