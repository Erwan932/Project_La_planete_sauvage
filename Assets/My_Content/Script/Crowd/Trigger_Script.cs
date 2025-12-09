using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadOnTrigger : MonoBehaviour
{
    // Tag du joueur (assure-toi que ton joueur porte ce tag)
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            // Recharge la scène active
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
