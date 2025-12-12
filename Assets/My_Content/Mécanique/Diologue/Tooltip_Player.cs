using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [Header("Références")]
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;

    [Header("Suivi du joueur")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    [Header("Textes du tutoriel")]
    [TextArea(2, 3)]
    public string[] tutorialLines;   // 0 = texte 1, 1 = texte 2

    [Header("UI Supplémentaire")]
    public GameObject extraImage;
    public int imageAppearsAtIndex = 1;

    private int currentIndex = 0;
    private bool tutorialStarted = false;
    private bool hasMoved = false;
    private bool hasJumped = false;

    // 🔥 Empêche le texte 2 de rejouer une 2e fois
    private bool finalTextAlreadyPlayed = false;

    void Start()
    {
        dialogueText.text = "";
        HideAll();

        if (extraImage != null)
            extraImage.SetActive(false);
    }

    void Update()
    {
        if (!tutorialStarted)
        {
            StartTutorial();
            return;
        }

        KeepFacingCorrectSide();

        // 🔥 SEULEMENT POUR LE TEXTE 1
        if (currentIndex == 0)
        {
            HandleMovementTutorial();
            HandleJumpTutorial();
        }
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (player == null) return;
        transform.position = player.position + offset;
    }

    void StartTutorial()
    {
        tutorialStarted = true;
        currentIndex = 0;
        ShowLine(currentIndex);
    }

    void ShowLine(int index)
    {
        if (index >= tutorialLines.Length)
        {
            HideAll();
            return;
        }

        ShowAll();
        dialogueText.text = tutorialLines[index];

        if (extraImage != null)
            extraImage.SetActive(index == imageAppearsAtIndex);
    }

    // 🔥 TEXTE 1 : mouvement
    void HandleMovementTutorial()
    {
        float mx = Input.GetAxis("Horizontal");
        float my = Input.GetAxis("Vertical");

        if (!hasMoved && (Mathf.Abs(mx) > 0.2f || Mathf.Abs(my) > 0.2f))
        {
            hasMoved = true;
            HideAll();
        }
    }

    // 🔥 TEXTE 1 : saut
    void HandleJumpTutorial()
    {
        if (!hasJumped && Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            HideAll();
        }
    }

    // 🔥 APPELÉ PAR LA DEUXIÈME TRIGGER BOX
    public void ShowFinalText()
    {
        if (finalTextAlreadyPlayed)
            return; // ❌ ne plus jamais rejouer

        finalTextAlreadyPlayed = true;
        currentIndex = 1; // texte 2
        ShowLine(currentIndex);
    }

    // 🔥 APPELÉ LORSQUE LE JOUEUR QUITTE LA TRIGGER BOX
    public void HideFinalText()
    {
        HideAll();
    }

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

        if (extraImage != null)
            extraImage.SetActive(false);
    }
}
