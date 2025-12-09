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
    public AudioClip nextTextSound;      // Son joué à chaque E
    private AudioSource audioSource;     // Source audio interne

    void Start()
    {
        // Ajout automatique du composant AudioSource si absent
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        if (player != null)
            player.canMove = false;

        textcomponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // Jouer le son si un clip est assigné
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

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textcomponent.text = "";

        foreach (char c in lines[index].ToCharArray())
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
