using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundZoneCycle : MonoBehaviour
{
    private bool playerInside = false;
    private AudioSource audioSource;
    public AudioClip soundClip;

    private Coroutine soundRoutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // pour que le son puisse tourner en boucle
        audioSource.clip = soundClip;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            // lancer la routine si pas déjà active
            if (soundRoutine == null)
                soundRoutine = StartCoroutine(SoundCycle());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            // arrêter la routine et le son
            if (soundRoutine != null)
            {
                StopCoroutine(soundRoutine);
                soundRoutine = null;
            }

            audioSource.Stop();
        }
    }

    IEnumerator SoundCycle()
    {
        // attendre 7 secondes avant de lancer le son
        yield return new WaitForSeconds(7.5f);

        while (playerInside)
        {
            // jouer le son pendant 4 secondes
            audioSource.Play();
            yield return new WaitForSeconds(3.5f);

            // couper le son pendant 7 secondes
            audioSource.Stop();
            yield return new WaitForSeconds(7f);
        }
    }
}
