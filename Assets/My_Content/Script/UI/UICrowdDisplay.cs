using TMPro;
using UnityEngine;

public class UICrowdDisplay : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI recruitableText;
    public TextMeshProUGUI activeText;
    public TextMeshProUGUI savedText;

    private CrowdManager crowd;

    private void Awake()
    {
        // Récupérer le CrowdManager dans la scène
        crowd = FindFirstObjectByType<CrowdManager>();
    }

    private void Update()
    {
        if (crowd == null) return;

        // Mise à jour des valeurs UI
        recruitableText.text = "Prisonniers : " + crowd.recruitableFollowers.Count;
        activeText.text = "Suiveur : " + crowd.activeFollowers.Count + " / 2";
        savedText.text = "Sauvé : " + crowd.SavedFollowers.Count + " / 3";
    }
}
