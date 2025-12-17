using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrowdManager : MonoBehaviour
{
    [Header("Followers Lists")]
    public List<FollowerAI> recruitableFollowers = new List<FollowerAI>();
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();
    public List<FollowerAI> SavedFollowers = new List<FollowerAI>();

    [Header("Follower Settings")]
    public float followDistance = 0.5f;
    public int maxFollowers = 2;
    public FollowerAI nearbyFollower;

    [Header("Player State")]
    public bool playerIsHidden = false;

    [Header("Feedback Recrutement")]
    public ParticleSystem joinParticles;
    public GameObject followerJoinUI;
    public float followerJoinDuration = 1f;
    private Coroutine uiRoutine;

    [Header("Feedback Follower Perdu")]
    public ParticleSystem lostParticles;
    public GameObject followerLostUI;
    public float followerLostDuration = 1f;
    private Coroutine lostUiRoutine;

    [Header("Feedback Max Followers")]
    public GameObject maxFollowersUI;
    private bool maxFollowersUIShown = false;

    [Header("Respawn Player")]
    public PlayerRespawn playerRespawn;

    [Header("Game Over UI")]
    public GameObject gameOverCanvas;
    public float gameOverCanvasDuration = 4f;

    // Flag pour éviter plusieurs Game Over
    private bool checkingGameOver = false;

    // -----------------------------
    // PLAYER STATE
    // -----------------------------
    public void SetHidden(bool state) => playerIsHidden = state;
    public bool IsPlayerHidden() => playerIsHidden;

    // -----------------------------
    // UPDATE
    // -----------------------------
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            TryRecruitNearbyFollower();

        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFollowers();
        CheckMaxFollowersUI();
        CheckGameOverCondition();
    }

    // -----------------------------
    // RECRUTEMENT
    // -----------------------------
    public void TryRecruitNearbyFollower()
    {
        if (nearbyFollower == null) return;
        if (activeFollowers.Count >= maxFollowers) return;

        if (recruitableFollowers.Contains(nearbyFollower))
        {
            recruitableFollowers.Remove(nearbyFollower);
            activeFollowers.Add(nearbyFollower);

            nearbyFollower.gameObject.SetActive(true);
            nearbyFollower.OnRecruited();
            nearbyFollower = null;

            PlayJoinFeedback();
            CheckMaxFollowersUI();
        }
    }

    // -----------------------------
    // SUPPRESSION RECRUTABLE
    // -----------------------------
    public void RemoveRecruitableFollower(FollowerAI follower)
    {
        if (follower == null) return;

        if (recruitableFollowers.Contains(follower))
        {
            recruitableFollowers.Remove(follower);

            if (nearbyFollower == follower)
                nearbyFollower = null;

            Destroy(follower.gameObject);

            if (recruitableFollowers.Count == 3)
                Debug.Log("Dernière chance");

            CheckGameOverCondition();
        }
    }

    // -----------------------------
    // DOMMAGES
    // -----------------------------
    public void TakeDamage()
    {
        if (activeFollowers.Count > 0)
        {
            FollowerAI lost = activeFollowers[activeFollowers.Count - 1];
            activeFollowers.RemoveAt(activeFollowers.Count - 1);
            lost.gameObject.SetActive(false);

            PlayLostFeedback();
        }
        else
        {
            if (playerRespawn != null && !playerRespawn.IsRespawning)
                playerRespawn.StartCoroutine(playerRespawn.DeathSequence(true));
        }

        CheckGameOverCondition();
        CheckMaxFollowersUI();
    }

    // -----------------------------
    // GAME OVER CHECK
    // -----------------------------
    private void CheckGameOverCondition()
    {
        // Condition : aucun follower actif ET moins de 3 recrutables
        if (activeFollowers.Count == 0 && recruitableFollowers.Count < 3)
        {
            if (!checkingGameOver)
            {
                checkingGameOver = true;
                StartCoroutine(GameOverDelay());
            }
        }
    }

    private IEnumerator GameOverDelay()
    {
        // Affiche le canvas Game Over
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Bloque les inputs ici si besoin

        yield return new WaitForSeconds(gameOverCanvasDuration);

        // Vérifie encore la condition
        if (activeFollowers.Count == 0 && recruitableFollowers.Count < 3)
        {
            Debug.Log("GAME OVER → Chargement Menu_Mort");
            SceneManager.LoadScene("Menu_Mort");
        }
        else
        {
            if (gameOverCanvas != null)
                gameOverCanvas.SetActive(false);
        }

        checkingGameOver = false;
    }

    // -----------------------------
    // FOLLOW FORMATION
    // -----------------------------
    public void UpdateFollowers()
    {
        Vector2 previousPos = transform.position;

        for (int i = 0; i < activeFollowers.Count; i++)
        {
            FollowerAI follower = activeFollowers[i];

            Vector2 direction = previousPos - (Vector2)follower.transform.position;
            float distance = direction.magnitude;

            Vector2 targetPos = follower.transform.position;
            if (distance > followDistance)
                targetPos += direction.normalized * (distance - followDistance);

            follower.SetFormationPosition(targetPos);
            previousPos = targetPos;
        }
    }

    // -----------------------------
    // NEARBY FOLLOWER
    // -----------------------------
    public void SetNearbyFollower(FollowerAI follower) => nearbyFollower = follower;

    public void ClearNearbyFollower(FollowerAI follower)
    {
        if (nearbyFollower == follower)
            nearbyFollower = null;
    }

    // -----------------------------
    // FEEDBACK JOIN
    // -----------------------------
    public void PlayJoinFeedback()
    {
        if (joinParticles != null) joinParticles.Play();

        if (followerJoinUI != null)
        {
            if (uiRoutine != null) StopCoroutine(uiRoutine);
            uiRoutine = StartCoroutine(ShowJoinUI());
        }
    }

    private IEnumerator ShowJoinUI()
    {
        followerJoinUI.SetActive(true);
        yield return new WaitForSeconds(followerJoinDuration);
        followerJoinUI.SetActive(false);
    }

    // -----------------------------
    // FEEDBACK LOST
    // -----------------------------
    public void PlayLostFeedback()
    {
        if (lostParticles != null) lostParticles.Play();

        if (followerLostUI != null)
        {
            if (lostUiRoutine != null) StopCoroutine(lostUiRoutine);
            lostUiRoutine = StartCoroutine(ShowLostUI());
        }
    }

    private IEnumerator ShowLostUI()
    {
        followerLostUI.SetActive(true);
        yield return new WaitForSeconds(followerLostDuration);
        followerLostUI.SetActive(false);
    }

    // -----------------------------
    // UI MAX FOLLOWERS
    // -----------------------------
    private void CheckMaxFollowersUI()
    {
        if (activeFollowers.Count >= maxFollowers)
        {
            if (!maxFollowersUIShown)
            {
                maxFollowersUI.SetActive(true);
                maxFollowersUIShown = true;
            }
        }
        else
        {
            if (maxFollowersUIShown)
            {
                maxFollowersUI.SetActive(false);
                maxFollowersUIShown = false;
            }
        }
    }
}
