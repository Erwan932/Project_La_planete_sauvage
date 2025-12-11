using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [Header("Références")]
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer[] backgrounds;

    [Header("Textes modifiables")]
    public string movementText = "Joystick pour se déplacer";
    public string jumpText = "Touche Y pour sauter";

    [Header("Délais")]
    public float delayBetweenTutorials = 1f;

    private bool hasMoved = false;
    private bool hasJumped = false;

    private Transform player;

    // --- AJOUT : compteur de pressions sur B ---
    private int pressCount = 0;
    private bool tutorialStarted = false;

    void Start()
    {
        player = transform.parent; // texte enfant du joueur

        // Le texte est totalement caché au début
        dialogueText.text = "";
        HideAll();
    }

    void Update()
    {
        // Tant que le joueur n'a pas pressé 3 fois B → on surveille seulement ça
        if (!tutorialStarted)
        {
            CheckStartTutorialInput();
            return;
        }

        // Une fois lancé → fonctionnement normal
        KeepFacingCorrectSide();
        HandleMovementTutorial();
        HandleJumpTutorial();
    }

    // --- AJOUT : détecter appui sur B (joystick button 1) ---
    void CheckStartTutorialInput()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            pressCount++;

            if (pressCount >= 4)
            {
                StartTutorial();
            }
        }
    }

    // --- AJOUT : lancement du tutoriel ---
    void StartTutorial()
    {
        tutorialStarted = true;

        ShowAll();
        dialogueText.text = movementText;
    }

    // --- Le texte flip comme le joueur mais reste lisible ---
    void KeepFacingCorrectSide()
    {
        Vector3 scale = transform.localScale;

        if (player.localScale.x > 0)
        {
            // Joueur regarde à droite → texte normal
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            // Joueur regarde à gauche → on inverse pour compenser
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    // --- Détection mouvement ---
    void HandleMovementTutorial()
    {
        if (!hasMoved)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (Mathf.Abs(moveX) > 0.2f || Mathf.Abs(moveY) > 0.2f)
            {
                hasMoved = true;
                dialogueText.text = "";
                HideAll();

                Invoke(nameof(ShowJumpTutorial), delayBetweenTutorials);
            }
        }
    }

    // --- Affichage tuto saut ---
    void ShowJumpTutorial()
    {
        if (!hasJumped)
        {
            ShowAll();
            dialogueText.text = jumpText;
        }
    }

    // --- Détection saut ---
    void HandleJumpTutorial()
    {
        if (hasMoved && !hasJumped)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Touche Y manette Xbox
            {
                hasJumped = true;
                dialogueText.text = "";
                HideAll();
            }
        }
    }

    // --- Affichage ---
    void ShowAll()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 1f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    // --- Cache ---
    void HideAll()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);

        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            bg.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
