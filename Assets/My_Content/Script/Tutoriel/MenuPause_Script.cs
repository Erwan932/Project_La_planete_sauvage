using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject canvasPause;
    public PlayerController playerController;
    public Button boutonParDefaut; // le bouton à sélectionner en premier

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7)) // bouton Menu Xbox
        {
            if (isPaused)
                ReprendreJeu();
            else
                MettreEnPause();
        }
    }

    void MettreEnPause()
    {
        canvasPause.SetActive(true);
        if (playerController != null)
            playerController.enabled = false;
        Time.timeScale = 0f;
        isPaused = true;

        // Sélectionne automatiquement le bouton par défaut
        EventSystem.current.SetSelectedGameObject(boutonParDefaut.gameObject);
    }

    public void ReprendreJeu()
    {
        canvasPause.SetActive(false);
        if (playerController != null)
            playerController.enabled = true;
        Time.timeScale = 1f;
        isPaused = false;
    }
}
