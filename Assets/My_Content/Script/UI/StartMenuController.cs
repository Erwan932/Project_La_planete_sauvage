using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        // Sélectionne le bouton Start au début
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Map_Test_V3");
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

