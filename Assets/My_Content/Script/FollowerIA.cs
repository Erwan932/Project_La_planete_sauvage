using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    private Transform target;
    public float followSpeed = 3f;
    public float followDistance = 1f;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // Se rapproche si trop loin
        if (distance > followDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * followSpeed * Time.deltaTime);
        }
    }
}
