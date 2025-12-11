using UnityEngine;

public class Target_UI_Script : MonoBehaviour
{
    [Header("Référence UI")]
    public GameObject groupBackground;

    // Montrer le Group BG
    public void ShowGroup(bool state = true)
    {
        if (groupBackground != null)
            groupBackground.SetActive(state);
    }

    // Cacher le Group BG
    public void HideGroup()
    {
        ShowGroup(false);
    }

    // Toggle si besoin
    public void ToggleGroup()
    {
        if (groupBackground != null)
            groupBackground.SetActive(!groupBackground.activeSelf);
    }
}
