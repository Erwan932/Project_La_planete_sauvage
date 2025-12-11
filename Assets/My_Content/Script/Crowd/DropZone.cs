using UnityEngine;
using System.Collections;

public class DropZone : MonoBehaviour
{
    [Header("UI")]
    public GameObject playerInteractUI; // UI touche au-dessus du joueur
    public GameObject shipUI;           // UI stats au-dessus du vaisseau

    private bool playerInZone = false;
    private CrowdManager crowd;
    private Transform playerTransform;

    private Coroutine playerUIRoutine;
    private Coroutine shipUIRoutine;

    [Header("Tween Settings")]
    public float uiTweenSpeed = 5f;      // vitesse de l'apparition/disparition
    public Vector3 playerUIOffset = new Vector3(0, 1.5f, 0);
    public Vector3 shipUIOffset = new Vector3(0, 2f, 0);

    private void Start()
    {
        crowd = FindFirstObjectByType<CrowdManager>();

        if (playerInteractUI != null)
        {
            playerInteractUI.SetActive(false);
            playerInteractUI.transform.localScale = Vector3.zero;
        }

        if (shipUI != null)
        {
            shipUI.SetActive(false);
            shipUI.transform.localScale = Vector3.zero;
        }
    }

private void Update()
{
    // Appui sur E pour déposer les followers
    if (playerInZone && Input.GetKeyDown(KeyCode.E))
    {
        if (crowd.activeFollowers.Count > 0)
        {
            DetachFollowers();
            // L'UI disparaît normalement après le tween
            StartUIDisappear(playerInteractUI, ref playerUIRoutine);
        }
        else
        {
            // Ne rien faire si aucun follower actif
            Debug.Log("Aucun follower actif à déposer.");
        }
    }

    // Suivi de l'UI joueur et vaisseau si nécessaire
    if (playerInteractUI != null && playerInteractUI.activeSelf && playerTransform != null)
        playerInteractUI.transform.position = playerTransform.position + playerUIOffset;

}

    private void DetachFollowers()
    {
        for (int i = crowd.activeFollowers.Count - 1; i >= 0; i--)
        {
            FollowerAI follower = crowd.activeFollowers[i];
            crowd.activeFollowers.Remove(follower);
            crowd.SavedFollowers.Add(follower);

            follower.StopFollowing();
            follower.targetPosition = follower.transform.position;

            Debug.Log("Follower déposé dans la DropZone !");
        }

        CheckWin();
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        playerInZone = true;
        playerTransform = collision.transform;

        // Afficher le bouton uniquement si le joueur a des followers
        if (crowd.activeFollowers.Count > 0 && playerInteractUI != null)
        {
            StartUIAppear(playerInteractUI, ref playerUIRoutine);
        }

        if (shipUI != null)
            StartUIAppear(shipUI, ref shipUIRoutine);
    }
}


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            playerTransform = null;

            if (playerInteractUI != null)
                StartUIDisappear(playerInteractUI, ref playerUIRoutine);

            if (shipUI != null)
                StartUIDisappear(shipUI, ref shipUIRoutine);
        }
    }

    private void CheckWin()
    {
        if (crowd.recruitableFollowers.Count == 0 &&
            crowd.activeFollowers.Count == 0)
        {
            Debug.Log("Niveau Fini !");
        }
    }

    // ----------------- Tween UI -----------------
    private void StartUIAppear(GameObject ui, ref Coroutine routine)
    {
        if (routine != null) StopCoroutine(routine);
        ui.SetActive(true);
        routine = StartCoroutine(UIAppearCoroutine(ui));
    }

    private void StartUIDisappear(GameObject ui, ref Coroutine routine)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(UIDisappearCoroutine(ui));
    }

    private IEnumerator UIAppearCoroutine(GameObject ui)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        ui.transform.localScale = startScale;

        while (t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        ui.transform.localScale = endScale;
    }

    private IEnumerator UIDisappearCoroutine(GameObject ui)
    {
        float t = 0f;
        Vector3 startScale = ui.transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        ui.transform.localScale = endScale;
        ui.SetActive(false);
    }
}
