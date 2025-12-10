using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMortController : MonoBehaviour
{
    public void OnStartClick()

    {
        SceneManager.LoadScene("Menu_Principal");
    }
    public void LoadMenu()
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

