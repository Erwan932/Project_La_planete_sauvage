using UnityEngine;

public class RockDropTrigger : MonoBehaviour
{
    public FallingRock2D rock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rock.DropRock();
            gameObject.SetActive(false);
        }
    }
}
