using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasMenu : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject canvasMenu;
    public Button boutonContinuer;
    public Button boutonMenu;

    [Header("Contrôle joueur")]
    public PlayerController playerController;

    void Start()
    {
        boutonContinuer.onClick.AddListener(FermerCanvas);
        boutonMenu.onClick.AddListener(RetourMenu);

        canvasMenu.SetActive(false);
    }

    public void OuvrirCanvas()
    {
        canvasMenu.SetActive(true);

        // Pause du jeu
        Time.timeScale = 0f;

        if (playerController != null)
            playerController.enabled = false;

        // FOCUS MANETTE
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boutonContinuer.gameObject);
    }

    void FermerCanvas()
    {
        canvasMenu.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Time.timeScale = 1f;
    }

    void RetourMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu_Principal");
    }
}
