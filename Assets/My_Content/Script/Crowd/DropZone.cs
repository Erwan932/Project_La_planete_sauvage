using UnityEngine;
using System.Collections;
using TMPro;

public class DropZone : MonoBehaviour
{
    [Header("Win Condition")]
    public int followersNeededForWin = 1;
    private bool winCondition = false;
    private bool readyToFinish = false;

    [Header("Win UI")]
    public GameObject winUI;
    private Coroutine winUIRoutine;

    [Header("Missing Followers UI")]
    public GameObject missingFollowersUI;
    private Coroutine missingUIRoutine;

    [Header("UI")]
    public GameObject playerInteractUI;
    public GameObject shipUI;
    private Coroutine playerUIRoutine;
    private Coroutine shipUIRoutine;

    [Header("UI Deposit Feedback")]
    public GameObject depositFeedbackUI;
    public TextMeshProUGUI depositCountText;
    private bool depositUIActive = false;

    [Header("Tween Settings")]
    public float uiTweenSpeed = 5f;
    public Vector3 playerUIOffset = new Vector3(0, 1.5f, 0);
    public Vector3 shipUIOffset = new Vector3(0, 2f, 0);

    private bool inputLocked = false;
    private bool playerInZone = false;
    private CrowdManager crowd;
    private Transform playerTransform;

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

        if (depositFeedbackUI != null)
        {
            depositFeedbackUI.SetActive(false);
            depositFeedbackUI.transform.localScale = Vector3.zero;
        }

        if (winUI != null)
        {
            winUI.SetActive(false);
            winUI.transform.localScale = Vector3.zero;
        }

        if (missingFollowersUI != null)
        {
            missingFollowersUI.SetActive(false);
            missingFollowersUI.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        if (!playerInZone || inputLocked) return;

        if (Input.GetButtonDown("Fire1"))
        {
            // 1️⃣ PRIORITÉ : déposer les followers actifs
            if (crowd.activeFollowers.Count > 0)
            {
                DetachFollowers();
                StartCoroutine(UnlockInputDelayed());
                return;
            }

            // 2️⃣ Si prêt à finir et aucun follower actif → finir le niveau
            if (readyToFinish && crowd.activeFollowers.Count == 0)
            {
                Debug.Log("Niveau Fini !");
                return;
            }
        }

        // Maintenir la position du tooltip player
        if (playerInteractUI != null && playerInteractUI.activeSelf && playerTransform != null)
            playerInteractUI.transform.position = playerTransform.position + playerUIOffset;
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
                rb.angularVelocity = 0;
            }

            droppedCount++;
        }

        if (droppedCount > 0)
            ShowDepositUI();

        CheckWin();
    }

    private void CheckWin()
    {
        if (!winCondition && crowd.SavedFollowers.Count >= followersNeededForWin)
        {
            winCondition = true;
            readyToFinish = true;

            // Si aucun follower actif → afficher message "Appuie pour terminer"
            if (crowd.activeFollowers.Count == 0)
            {
                Debug.Log("Appuie pour terminer !");
                ShowDepositUIMessage("Niveau prêt à finir !");
            }

            // Afficher Win UI
            if (winUI != null && !winUI.activeSelf)
            {
                StartUIAppear(winUI, ref winUIRoutine);
            }

            // Masquer MissingFollowersUI si actif
            if (missingFollowersUI != null)
                StartUIDisappear(missingFollowersUI, ref missingUIRoutine);
        }
    }

    private void ShowDepositUI()
    {
        if (depositFeedbackUI == null) return;

        if (!depositUIActive)
        {
            depositFeedbackUI.SetActive(true);
            depositUIActive = true;
            depositFeedbackUI.transform.localScale = Vector3.one;
        }

        if (depositCountText != null)
            depositCountText.text = "+ " + crowd.SavedFollowers.Count + " sauvés";
    }

    private void ShowDepositUIMessage(string message)
    {
        if (depositFeedbackUI == null) return;

        if (!depositUIActive)
        {
            depositFeedbackUI.SetActive(true);
            depositUIActive = true;
            depositFeedbackUI.transform.localScale = Vector3.one;
        }

        if (depositCountText != null)
            depositCountText.text = message;
    }

    private IEnumerator UnlockInputDelayed()
    {
        inputLocked = true;
        yield return null; // attendre une frame
        inputLocked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
            playerTransform = collision.transform;

            if (playerInteractUI != null)
                StartUIAppear(playerInteractUI, ref playerUIRoutine);

            if (shipUI != null)
                StartUIAppear(shipUI, ref shipUIRoutine);

            if (!winCondition && crowd.SavedFollowers.Count < followersNeededForWin && missingFollowersUI != null)
                StartUIAppear(missingFollowersUI, ref missingUIRoutine);
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

            if (missingFollowersUI != null)
                StartUIDisappear(missingFollowersUI, ref missingUIRoutine);
        }
    }

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
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;
        ui.transform.localScale = start;

        while (t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
        ui.transform.localScale = end;
    }

    private IEnumerator UIDisappearCoroutine(GameObject ui)
    {
        float t = 0f;
        Vector3 start = ui.transform.localScale;
        Vector3 end = Vector3.zero;

        while (t < 1f)
        {
            t += Time.deltaTime * uiTweenSpeed;
            ui.transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
        ui.transform.localScale = end;
        ui.SetActive(false);
    }
}
