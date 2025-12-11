using UnityEngine;

public class Target_UI_Script : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject groupBackground;
    public GameObject savedBackground;
    public GameObject recruitableBackground;

    // -------------------------
    //  FONCTIONS INDIVIDUELLES
    // -------------------------

    public void ShowGroup(bool state = true)
    {
        if (groupBackground != null)
            groupBackground.SetActive(state);
    }

    public void ShowSaved(bool state = true)
    {
        if (savedBackground != null)
            savedBackground.SetActive(state);
    }

    public void ShowRecruitable(bool state = true)
    {
        if (recruitableBackground != null)
            recruitableBackground.SetActive(state);
    }

    // -------------------------
    //  FONCTIONS POUR N’AFFICHER QU’UN PANEL
    // -------------------------

    public void ShowOnlyGroup()
    {
        ShowGroup(true);
        ShowSaved(false);
        ShowRecruitable(false);
    }

    public void ShowOnlySaved()
    {
        ShowGroup(false);
        ShowSaved(true);
        ShowRecruitable(false);
    }

    public void ShowOnlyRecruitable()
    {
        ShowGroup(false);
        ShowSaved(false);
        ShowRecruitable(true);
    }

    // -------------------------
    //  TOUT CACHER
    // -------------------------

    public void HideAll()
    {
        ShowGroup(false);
        ShowSaved(false);
        ShowRecruitable(false);
    }
}
