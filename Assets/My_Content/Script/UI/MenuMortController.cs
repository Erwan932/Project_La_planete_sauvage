using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMortController : MonoBehaviour
{
    [Header("Boutons du Canvas")]
    public Button boutonStart;
    public Button boutonMenu;

    void Start()
    {


        boutonStart.onClick.AddListener(OnStartClick);
        boutonMenu.onClick.AddListener(OnReloadMap);
    }


    public void OnStartClick()
    {
        SceneManager.LoadScene("Menu_Principal");
    }

    public void OnReloadMap()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Map_Test_V3");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CrowdManager cm = Object.FindFirstObjectByType<CrowdManager>();
        if (cm != null)
        {
           // cm.OnPlayerRespawn();
        }
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
