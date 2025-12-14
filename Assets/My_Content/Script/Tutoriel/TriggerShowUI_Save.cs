using UnityEngine;
using System.Collections;

public class TriggerCanvasTimed : MonoBehaviour
{
    [Header("Canvas à afficher")]
    public GameObject canvasToShow;

    [Header("Temps d'affichage")]
    public float displayTime = 2f;

    private Coroutine hideRoutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Active le canvas
        canvasToShow.SetActive(true);

        // Reset le timer si on re-rentre
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideCanvasAfterDelay());
    }

    private IEnumerator HideCanvasAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        canvasToShow.SetActive(false);
    }
}
