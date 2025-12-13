using UnityEngine;
using System.Collections;

public class UIFireTrigger : MonoBehaviour
{
    public GameObject uiElement; // UI World Space (Panel ou Canvas)
    public float uiTweenSpeed = 5f;

    private Coroutine uiRoutine;

    private void Start()
    {
        if(uiElement != null)
        {
            uiElement.SetActive(false);
            uiElement.transform.localScale = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        StartUIAppear(uiElement, ref uiRoutine);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        StartUIDisappear(uiElement, ref uiRoutine);
    }

    private void StartUIAppear(GameObject ui, ref Coroutine routine)
    {
        if(ui == null) return;
        if(routine != null) StopCoroutine(routine);
        ui.SetActive(true);
        routine = StartCoroutine(UIAppear(ui));
    }

    private void StartUIDisappear(GameObject ui, ref Coroutine routine)
    {
        if(ui == null) return;
        if(routine != null) StopCoroutine(routine);
        routine = StartCoroutine(UIDisappear(ui));
    }

    private IEnumerator UIAppear(GameObject ui)
    {
        float t = 0f;
        ui.transform.localScale = Vector3.zero;
        while(t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        ui.transform.localScale = Vector3.one;
    }

    private IEnumerator UIDisappear(GameObject ui)
    {
        float t = 0f;
        Vector3 start = ui.transform.localScale;
        while(t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(start, Vector3.zero, t);
            yield return null;
        }
        ui.transform.localScale = Vector3.zero;
        ui.SetActive(false);
    }
}

