using UnityEngine;
using System.Collections;
using TMPro;

public class DropZone : MonoBehaviour
{
    [Header("Win Condition")]
    public int followersNeededForWin = 1;
    private bool readyToFinish = false;

    [Header("UI")]
    public GameObject playerInteractUI;      // Tooltip pour déposer
    public GameObject shipUI;                // UI secondaire
    public GameObject missingFollowersUI;    // UI "followers manquants"
    public GameObject winUI;                 // UI "appuie pour finir"

    [Header("Deposit Feedback UI")]
    public GameObject depositFeedbackUI;
    public TextMeshProUGUI depositCountText;

    [Header("Tween Settings")]
    public float uiTweenSpeed = 5f;

    private bool playerInZone = false;
    private bool inputLocked = false;


    private CrowdManager crowd;
    private Transform playerTransform;

    private Coroutine playerUIRoutine;
    private Coroutine shipUIRoutine;
    private Coroutine missingUIRoutine;
    private Coroutine winUIRoutine;

    private bool depositUIActive = false;

    // Inputs
    private const string INPUT_DEPOSIT = "Fire1";
    private const string INPUT_FINISH = "Fire3"; // ou "Fire2"

    private void Start()
    {
        crowd = FindFirstObjectByType<CrowdManager>();

        InitUI(playerInteractUI);
        InitUI(shipUI);
        InitUI(missingFollowersUI);
        InitUI(winUI);
        InitUI(depositFeedbackUI);
    }

    private void Update()
    {
        if (!playerInZone || inputLocked) return;

        // ⚠️ Si un follower est recrutables à proximité → ne rien faire
        if (crowd.nearbyFollower != null)
            return;

        // Affichage dynamique du tooltip : apparaît si le joueur a des followers, disparaît sinon
        if (crowd.activeFollowers.Count > 0)
        {
            if (!playerInteractUI.activeSelf)
                StartUIAppear(playerInteractUI, ref playerUIRoutine);
        }
        else
        {
            if (playerInteractUI.activeSelf)
                HideDepositTooltip();
        }

        // Dépôt des followers
        if (Input.GetButtonDown(INPUT_DEPOSIT))
        {
            if (crowd.activeFollowers.Count > 0)
            {
                DetachFollowers();
                // L'UI disparaît déjà automatiquement grâce à la condition ci-dessus
                StartCoroutine(UnlockInputDelayed());
            }
        }

        // Fin du niveau
        if (readyToFinish && Input.GetButtonDown(INPUT_FINISH))
        {
            Debug.Log("Niveau Fini");
        }
    }


    // -----------------------------
    // DÉPÔT DES FOLLOWERS
    // -----------------------------
    private void DetachFollowers()
    {
        int droppedCount = 0;

        for (int i = crowd.activeFollowers.Count - 1; i >= 0; i--)
        {
            FollowerAI follower = crowd.activeFollowers[i];

            crowd.activeFollowers.Remove(follower);
            crowd.SavedFollowers.Add(follower);

            follower.StopFollowing();
            follower.targetPosition = follower.transform.position;

            Rigidbody2D rb = follower.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            droppedCount++;
        }

        if (droppedCount > 0)
            ShowDepositUI(droppedCount);

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (crowd.SavedFollowers.Count >= followersNeededForWin)
        {
            readyToFinish = true;

            // Affiche l'UI de fin
            if (winUI != null && !winUI.activeSelf)
                StartUIAppear(winUI, ref winUIRoutine);

            // Masque l'UI "followers manquants"
            if (missingFollowersUI != null)
                StartUIDisappear(missingFollowersUI, ref missingUIRoutine);
        }
    }

    private void ShowDepositUI(int droppedCount)
    {
        if (depositFeedbackUI == null || depositCountText == null) return;

        if (!depositUIActive)
        {
            depositFeedbackUI.SetActive(true);
            depositUIActive = true;
            depositFeedbackUI.transform.localScale = Vector3.one;
        }

        depositCountText.text = $"+ {droppedCount} déposés";
    }

    private void HideDepositTooltip()
    {
        StartUIDisappear(playerInteractUI, ref playerUIRoutine);
    }

    // -----------------------------
    // UI INIT & TWEENS
    // -----------------------------
    private void InitUI(GameObject ui)
    {
        if (ui == null) return;
        ui.SetActive(false);
        ui.transform.localScale = Vector3.zero;
    }

    private void StartUIAppear(GameObject ui, ref Coroutine routine)
    {
        if (ui == null) return;
        if (routine != null) StopCoroutine(routine);

        ui.SetActive(true);
        routine = StartCoroutine(UIAppear(ui));
    }

    private void StartUIDisappear(GameObject ui, ref Coroutine routine)
    {
        if (ui == null) return;
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(UIDisappear(ui));
    }

    private IEnumerator UIAppear(GameObject ui)
    {
        float t = 0f;
        ui.transform.localScale = Vector3.zero;

        while (t < 1f)
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

        while (t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(start, Vector3.zero, t);
            yield return null;
        }

        ui.transform.localScale = Vector3.zero;
        ui.SetActive(false);
    }

    private IEnumerator UnlockInputDelayed()
    {
        inputLocked = true;
        yield return null;
        inputLocked = false;
    }

    // -----------------------------
    // TRIGGERS
    // -----------------------------
private void OnTriggerEnter2D(Collider2D collision)
{
    if (!collision.CompareTag("Player")) return;

    playerInZone = true;
    playerTransform = collision.transform;

    // Affiche les UI secondaires
    StartUIAppear(shipUI, ref shipUIRoutine);

    if (!readyToFinish && crowd.SavedFollowers.Count < followersNeededForWin)
        StartUIAppear(missingFollowersUI, ref missingUIRoutine);
}

private void OnTriggerExit2D(Collider2D collision)
{
    if (!collision.CompareTag("Player")) return;

    playerInZone = false;
    playerTransform = null;

    // Masque toutes les UI
    StartUIDisappear(playerInteractUI, ref playerUIRoutine);
    StartUIDisappear(shipUI, ref shipUIRoutine);
    StartUIDisappear(missingFollowersUI, ref missingUIRoutine);
}

}

