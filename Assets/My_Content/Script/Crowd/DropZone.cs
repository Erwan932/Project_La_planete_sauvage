using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class DropZone : MonoBehaviour
{
    [Header("Win Condition")]
    [Tooltip("Nombre de personnes à déposer pour gagner")]
    public int followersNeededForWin = 3; // 👈 RÉGLABLE DANS UNITY
    private bool readyToFinish = false;

    [Header("Win Scene")]
    [Tooltip("Nom exact de la scène de victoire")]
    public string winSceneName = "WinScene"; // 👈 À RÉGLER DANS UNITY

    [Header("UI")]
    public GameObject playerInteractUI;
    public GameObject shipUI;
    public GameObject missingFollowersUI;
    public GameObject winUI;

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
    private const string INPUT_DEPOSIT = "Fire1"; // bouton dépôt
    private const KeyCode INPUT_FINISH = KeyCode.JoystickButton3; // 🎮 Y (Xbox)

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

        // --- Tooltip dépôt ---
        if (crowd.activeFollowers.Count > 0)
        {
            if (!playerInteractUI.activeSelf)
                StartUIAppear(playerInteractUI, ref playerUIRoutine);
        }
        else
        {
            if (playerInteractUI.activeSelf)
                StartUIDisappear(playerInteractUI, ref playerUIRoutine);
        }

        // --- Dépôt ---
        if (Input.GetButtonDown(INPUT_DEPOSIT) && crowd.activeFollowers.Count > 0)
        {
            DetachFollowers();
            StartCoroutine(UnlockInputDelayed());
        }

        // --- FIN DU NIVEAU (Y Xbox) ---
        if (readyToFinish && Input.GetKeyDown(INPUT_FINISH))
        {
            LoadWinScene();
        }
    }

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

            if (winUI != null)
                StartUIAppear(winUI, ref winUIRoutine);

            if (missingFollowersUI != null)
                StartUIDisappear(missingFollowersUI, ref missingUIRoutine);
        }
    }

    private void LoadWinScene()
    {
        Debug.Log("🎉 Victoire !");
        SceneManager.LoadScene(winSceneName);
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

    // -----------------------------
    // UI HELPERS
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

        StartUIAppear(shipUI, ref shipUIRoutine);

        if (!readyToFinish)
            StartUIAppear(missingFollowersUI, ref missingUIRoutine);
        else
            StartUIAppear(winUI, ref winUIRoutine); // Si victoire déjà prête, afficher WinUI
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInZone = false;
        playerTransform = null;

        StartUIDisappear(playerInteractUI, ref playerUIRoutine);
        StartUIDisappear(shipUI, ref shipUIRoutine);
        StartUIDisappear(missingFollowersUI, ref missingUIRoutine);

        // NE PAS cacher le WinUI si la condition de victoire est remplie
        if (!readyToFinish)
            StartUIDisappear(winUI, ref winUIRoutine);
    }
}
