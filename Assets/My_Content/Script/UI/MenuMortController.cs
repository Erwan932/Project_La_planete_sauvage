using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMortController : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasFade deathCanvasFade;

    [Header("Boutons")]
    public Button boutonStart;
    public Button boutonMenu;

    void Start()
    {
        // Active le canvas (le fade se lance automatiquement)
        deathCanvasFade.gameObject.SetActive(true);

        boutonStart.onClick.AddListener(OnStartClick);
        boutonMenu.onClick.AddListener(OnReloadMap);
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Menu_Principal");
    }

    public void OnReloadMap()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        CheckpointData.Reset();

        SceneManager.LoadScene("Map_Test_V3", LoadSceneMode.Single);
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
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
    }
}
