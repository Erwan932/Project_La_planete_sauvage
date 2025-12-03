using UnityEngine;

public class Ia_Vaisseaux_Script : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;
    private Transform targetPoint;
    public float speed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPoint = pointB;
    }

    void Update()
    {

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }
}

