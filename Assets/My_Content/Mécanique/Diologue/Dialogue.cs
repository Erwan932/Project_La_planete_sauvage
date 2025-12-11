using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textcomponent;
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

    // Les lignes chargées depuis un trigger
    private string[] currentLines;

    // 🔥 Position d'origine du panel (fix du bug d'apparition)
    private Vector2 originalPanelPosition;


    void Start()
    {
        // Prépare l’audio
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        textcomponent.text = "";
        panel.gameObject.SetActive(false);

        // 🔥 On enregistre la position d'origine du panel
        originalPanelPosition = panel.anchoredPosition;
    }


    void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetButtonDown("Fire2"))
        {
            if (nextTextSound != null)
                audioSource.PlayOneShot(nextTextSound);

            if (textcomponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textcomponent.text = currentLines[index];
            }
        }
    }

    public void ForceCloseDialogue()
    {
        StopAllCoroutines();
        dialogueActive = false;

        if (panel != null)
            panel.gameObject.SetActive(false);

        if (player != null)
            player.canMove = true;
    }

    public void StartNewDialogue(string[] newLines)
    {
        if (dialogueActive) return;

        currentLines = newLines;
        index = 0;

        dialogueActive = true;

        if (player != null)
            player.canMove = false;

        // 🔥 Remet le panel à sa position d'origine AVANT d'afficher
        panel.anchoredPosition = originalPanelPosition;

        panel.gameObject.SetActive(true);

        StartCoroutine(TypeLine());
    }


    IEnumerator TypeLine()
    {
        textcomponent.text = "";

        foreach (char c in currentLines[index])
        {
            textcomponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }


    void NextLine()
    {
        if (index < currentLines.Length - 1)
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

        panel.gameObject.SetActive(false);
    }
}
