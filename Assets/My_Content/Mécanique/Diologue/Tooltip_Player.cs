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

    void Start()
    {
        player = transform.parent; // texte enfant du joueur

        ShowAll();
        dialogueText.text = movementText;
    }

    void Update()
    {
        KeepFacingCorrectSide();
        HandleMovementTutorial();
        HandleJumpTutorial();
    }

    // --- Le texte flip comme le joueur (toujours dans le bon sens) ---
    void KeepFacingCorrectSide()
    {
        if (player.localScale.x > 0)
        {
            // joueur regarde à droite
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            // joueur regarde à gauche
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
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

    // --- Détection saut avec touche Y ---
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
