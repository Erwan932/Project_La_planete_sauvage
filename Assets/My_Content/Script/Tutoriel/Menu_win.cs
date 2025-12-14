using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    public Button startButton;
    public TextMeshProUGUI infoText;

    [Header("Scène")]
    public string sceneToLoad = "Menu_Principal";

    [Header("Delay d'apparition")]
    public float showUIDelay = 5f;

    private void Start()
    {
        // Cache les éléments UI au démarrage
        startButton.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);

        StartCoroutine(ShowUIAfterDelay());
    }

    private IEnumerator ShowUIAfterDelay()
    {
        yield return new WaitForSeconds(showUIDelay);

        // Affiche le texte
        infoText.gameObject.SetActive(true);

        // Affiche le bouton
        startButton.gameObject.SetActive(true);

        // Sélection automatique du bouton
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    // 🔘 Bouton → Changement de scène
    public void OnStartClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // 🔘 Quitter
    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
