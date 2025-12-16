using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [Header("UI")]
    public Button startButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public float fadeOutDuration = 1.5f;

    void Start()
    {
        // Sélection du bouton au démarrage
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);

        // Lance le son du menu
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OnStartClick()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;
            float timer = 0f;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
        }

        SceneManager.LoadScene("Map_Test_V3");
    }
}
