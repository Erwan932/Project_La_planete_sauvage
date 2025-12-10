using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMortController : MonoBehaviour
{
    [Header("Références UI")]
    public CanvasGroup buttonsGroup;  // Le panel contenant les boutons

    [Header("Réglages")]
    public float delayBeforeShow = 2f;
    public float fadeDuration = 1f;

    void Start()
    {
        // Commence invisible
        if (buttonsGroup != null)
        {
            buttonsGroup.alpha = 0f;
            buttonsGroup.interactable = false;
            buttonsGroup.blocksRaycasts = false;
        }

        StartCoroutine(ShowButtons());
    }

    IEnumerator ShowButtons()
    {
        // ⏳ Attendre avant d'afficher
        yield return new WaitForSeconds(delayBeforeShow);

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;

            if (buttonsGroup != null)
                buttonsGroup.alpha = alpha;

            yield return null;
        }

        // 🔥 Activation finale
        buttonsGroup.alpha = 1f;
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
    }

    // === Boutons ===

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
