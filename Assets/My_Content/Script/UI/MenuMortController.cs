using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuMortController : MonoBehaviour
{
    [Header("Boutons du Canvas")]
    public Button boutonStart;
    public Button boutonMenu;
    public Button boutonQuit;

    void Start()
    {
        // Les boutons sont invisibles au début
        SetButtonsVisible(false);

        // On active les boutons après 2 secondes
        StartCoroutine(ShowButtonsWithDelay());

        // Branche les clics des boutons
        boutonStart.onClick.AddListener(OnStartClick);
        boutonMenu.onClick.AddListener(LoadMenu);
        boutonQuit.onClick.AddListener(OnQuitClick);
    }

    IEnumerator ShowButtonsWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SetButtonsVisible(true);
    }

    void SetButtonsVisible(bool visible)
    {
        boutonStart.gameObject.SetActive(visible);
        boutonMenu.gameObject.SetActive(visible);
        boutonQuit.gameObject.SetActive(visible);
    }

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
