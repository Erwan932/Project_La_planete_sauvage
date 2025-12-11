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

    [Header("Liste des textes du tutoriel")]
    [TextArea(2, 3)]
    public string[] tutorialLines;   // 🔥 tu peux écrire autant de textes que tu veux ici !

    [Header("Délais")]
    public float delayBetweenTutorials = 1f;

    [Header("UI Supplémentaire")]
    public GameObject extraImage;    // Image à montrer sur un texte spécifique
    public int imageAppearsAtIndex = 1; // 🔥 Index du texte où l’image s’affiche

    private int currentIndex = 0;
    private bool tutorialStarted = false;
    private bool hasMoved = false;
    private bool hasJumped = false;

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
        HandleMovementTutorial();
        HandleJumpTutorial();
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
        ShowAll();
        dialogueText.text = tutorialLines[index];

        // 🔥 Affiche l'image SEULEMENT sur le texte choisi
        if (extraImage != null)
            extraImage.SetActive(index == imageAppearsAtIndex);
    }

    void HandleMovementTutorial()
    {
        if (!hasMoved && currentIndex == 0)
        {
            float mx = Input.GetAxis("Horizontal");
            float my = Input.GetAxis("Vertical");

            if (Mathf.Abs(mx) > 0.2f || Mathf.Abs(my) > 0.2f)
            {
                hasMoved = true;

                NextLine();
            }
        }
    }

    void HandleJumpTutorial()
    {
        if (currentIndex == 1 && !hasJumped)
        {
            if (Input.GetButton("Jump"))
            {
                hasJumped = true;

                NextLine();
            }
        }
    }

    void NextLine()
    {
        HideAll();

        currentIndex++;

        if (currentIndex < tutorialLines.Length)
        {
            Invoke(nameof(ShowNextLineDelayed), delayBetweenTutorials);
        }
        else
        {
            // Fin du tutoriel
            HideAll();
        }
    }

    void ShowNextLineDelayed()
    {
        ShowLine(currentIndex);
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
