using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;

    [Header("Suivi du joueur")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    [Header("Textes du tutoriel")]
    [TextArea(2, 3)]
    public string[] tutorialLines; // 0 = texte 1 / 1 = texte 2

    [Header("Images")]
    public GameObject firstImage;   // image du texte 1
    public GameObject secondImage;  // image du texte 2

    private bool firstTextActive = true;

    void Start()
    {
        dialogueText.text = "";
        HideAll();

        ShowFirstText();
    }

    void Update()
    {
        FollowPlayer();
        KeepFacingCorrectSide();

        // 🔥 Supprimer le TEXTE 1 quand le joueur bouge
        if (firstTextActive)
        {
            float mx = Input.GetAxis("Horizontal");
            float my = Input.GetAxis("Vertical");

            if (Mathf.Abs(mx) > 0.2f || Mathf.Abs(my) > 0.2f)
            {
                firstTextActive = false;
                HideAll();
            }
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;
        transform.position = player.position + offset;
    }

    // =============================
    // AFFICHAGE DES TEXTES
    // =============================

    void ShowFirstText()
    {
        ShowLine(0);
        ShowFirstImage();
    }

    public void ShowSecondText()
    {
        ShowLine(1);
        ShowSecondImage();
    }

    public void HideText()
    {
        HideAll();
    }

    void ShowLine(int index)
    {
        if (index >= tutorialLines.Length) return;

        ShowAll();
        dialogueText.text = tutorialLines[index];
    }

    // =============================
    // GESTION DES IMAGES
    // =============================

    void ShowFirstImage()
    {
        if (firstImage != null) firstImage.SetActive(true);
        if (secondImage != null) secondImage.SetActive(false);
    }

    void ShowSecondImage()
    {
        if (firstImage != null) firstImage.SetActive(false);
        if (secondImage != null) secondImage.SetActive(true);
    }

    // =============================
    // ORIENTATION & VISIBILITÉ
    // =============================

    void KeepFacingCorrectSide()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;
        scale.x = player.localScale.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void ShowAll()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    void HideAll()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 0f);
        }

        if (firstImage != null) firstImage.SetActive(false);
        if (secondImage != null) secondImage.SetActive(false);
    }
}
