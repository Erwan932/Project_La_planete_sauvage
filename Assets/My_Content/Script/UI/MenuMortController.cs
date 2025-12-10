using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonsManager : MonoBehaviour
{
    // 🔹 Charge la scène du menu principal
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu_Principal");
    }

    // 🔹 Quitte le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Permet de quitter en mode Play
#endif
    }

    // 🔹 Recharge la scène actuelle
    public void ReplayCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 🔹 Recharge une scène spécifique (si tu veux choisir laquelle dans l’inspecteur)
    public void LoadSpecificScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
