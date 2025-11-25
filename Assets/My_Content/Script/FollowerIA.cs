using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    private Vector2 targetPosition;
    public float followSpeed = 2f;

    public void SetTarget(Transform player)
    {
        // Rien à faire ici pour la formation
    }

    public void SetFormationPosition(Vector2 pos)
    {
        targetPosition = pos;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}

