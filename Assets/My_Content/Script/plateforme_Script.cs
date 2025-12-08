using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public Transform pointA;     // Position 1
    public Transform pointB;     // Position 2
    public float moveSpeed = 3f; // Si tu veux interpolation (option)

    private bool canActivate = false;
    private bool atPointA = true;

    void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.E))
        {
            if (atPointA)
                transform.position = pointB.position;
            else
                transform.position = pointA.position;

            atPointA = !atPointA;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canActivate = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canActivate = false;
    }
}

