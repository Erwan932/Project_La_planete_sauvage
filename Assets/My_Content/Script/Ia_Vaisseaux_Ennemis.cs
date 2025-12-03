using UnityEngine;

public class MovePingPong : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Transform target;

    void Start()
    {
        target = pointB; // On commence en allant vers B
    }

    void Update()
    {
        // Déplacement vers la cible
        transform.position = Vector2.MoveTowards
        (
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Si on atteint un point, on change de direction
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
