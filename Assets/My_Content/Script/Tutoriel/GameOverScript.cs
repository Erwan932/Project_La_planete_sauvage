using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvasController : MonoBehaviour
{
    [Header("Boutons du Canvas")]
    public Button boutonContinuer;   // bouton pour relancer la partie
    public Button boutonQuitter;     // bouton pour aller au menu mort

    [Header("Nom des scènes")]
    public string sceneJeu = "Map_Test_V3";   // scène principale
    public string sceneMenuMort = "Menu_Mort"; // scène du menu mort

    private void Start()
    {
        // Associer les boutons aux méthodes
        if (boutonContinuer != null)
            boutonContinuer.onClick.AddListener(OnContinuerClick);

        if (boutonQuitter != null)
            boutonQuitter.onClick.AddListener(OnQuitterClick);
    }

    private void Update()
    {
        // 🎮 Vérifie si le joueur appuie sur B (joystick button 1)
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            OnQuitterClick();
        }
    }

    // 🔄 Relancer la partie depuis le début
    public void OnContinuerClick()
    {
        // Reset des checkpoints
        CheckpointData.Reset();

        // Recharge la scène principale
        SceneManager.LoadScene(sceneJeu, LoadSceneMode.Single);
    }

    // 🚪 Aller au menu mort
    public void OnQuitterClick()
    {
        SceneManager.LoadScene(sceneMenuMort, LoadSceneMode.Single);
    }
}
