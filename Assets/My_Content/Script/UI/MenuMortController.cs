using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        // Sélectionner automatiquement le premier bouton pour la manette/clavier
        EventSystem.current.SetSelectedGameObject(boutonStart.gameObject);
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

    void Update()
    {
        // Exemple : valider avec la touche "Start" de la manette (Input Manager classique)
        if (Input.GetButtonDown("Submit")) // par défaut "Enter" ou bouton A
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            if (current != null)
            {
                Button btn = current.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke();
                }
            }
        }

        // Exemple : quitter avec la touche "Cancel" (par défaut "Esc" ou bouton B)
        if (Input.GetButtonDown("Cancel"))
        {
            OnQuitClick();
        }
    }
}
