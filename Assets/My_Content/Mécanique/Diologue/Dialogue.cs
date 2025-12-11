using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textcomponent;
    public string[] lines;
    public float textspeed = 0.05f;

    public PlayerMovement player;

    private int index;

    [Header("Animation de sortie")]
    public RectTransform panel;
    public float slideDistance = 600f;
    public float slideSpeed = 4f;

    [Header("Son lorsqu'on avance le dialogue")]
    public AudioClip nextTextSound;
    private AudioSource audioSource;

    private bool dialogueActive = false;

    void Start()
    {
        // Prépare l’audio
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        // Le dialogue ne démarre plus automatiquement
        textcomponent.text = "";
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetButtonDown("Fire2"))
        {
            if (nextTextSound != null)
                audioSource.PlayOneShot(nextTextSound);

            if (textcomponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textcomponent.text = lines[index];
            }
        }
    }

    // --- ACTIVATION depuis un trigger ---
    public void ActivateDialogue()
    {
        if (dialogueActive) return;

        dialogueActive = true;

        if (player != null)
            player.canMove = false;

        panel.gameObject.SetActive(true);

        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textcomponent.text = "";

        foreach (char c in lines[index])
        {
            textcomponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (player != null)
                player.canMove = true;

            StartCoroutine(SlideAndClose());
            dialogueActive = false;
        }
    }

    IEnumerator SlideAndClose()
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 target = start + new Vector2(-slideDistance, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
