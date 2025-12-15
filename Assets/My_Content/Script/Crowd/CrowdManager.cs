using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    [Header("Followers Lists")]
    public List<FollowerAI> recruitableFollowers;
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();
    public List<FollowerAI> SavedFollowers = new List<FollowerAI>();

    [Header("Follower Settings")]
    public float followDistance = 0.5f;
    public int maxFollowers = 2; // limite uniquement pour le recrutement
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

    [Header("Respawn Player")]
    public PlayerRespawn playerRespawn; // assigner dans l’inspecteur

    // -----------------------------
    // PLAYER STATE
    // -----------------------------
    public void SetHidden(bool state) => playerIsHidden = state;
    public bool IsPlayerHidden() => playerIsHidden;

    // -----------------------------
    // UPDATE LOOP
    // -----------------------------
    private void Update()
    {
        // Recrutement d'un follower proche
        if (Input.GetButtonDown("Fire1"))
            TryRecruitNearbyFollower();

        // Décrémenter la foule si le joueur prend des dégâts
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFollowers();
    }

    // -----------------------------
    // RECRUTEMENT
    // -----------------------------
    public void TryRecruitNearbyFollower()
    {
        if (nearbyFollower == null)
        {
            Debug.Log("Aucun follower à proximité");
            return;
        }

        // Limite max uniquement pour le recrutement
        if (activeFollowers.Count >= maxFollowers)
        {
            Debug.Log("Limite atteinte : " + maxFollowers + " followers maximum pour le recrutement");
            return;
        }

        if (recruitableFollowers.Contains(nearbyFollower))
        {
            recruitableFollowers.Remove(nearbyFollower);
            activeFollowers.Add(nearbyFollower);

            nearbyFollower.gameObject.SetActive(true);

            nearbyFollower.OnRecruited();
            nearbyFollower = null;

            if (joinParticles != null)
                joinParticles.Play();

            Debug.Log("Follower recruté !");
            PlayJoinFeedback();
        }
    }

    // -----------------------------
    // DOMMAGES
    // -----------------------------
    public void TakeDamage()
    {
        if (activeFollowers.Count > 0)
        {
            // Retire le dernier follower actif
            FollowerAI lost = activeFollowers[activeFollowers.Count - 1];
            activeFollowers.RemoveAt(activeFollowers.Count - 1);
            lost.gameObject.SetActive(false);

            Debug.Log("Follower perdu !");
            PlayLostFeedback(); // Nouveau feedback visuel/particules
        }
        else
        {
            Debug.Log("PLAYER DEATH - Respawn sequence");

            // Au lieu de Game Over, on appelle la séquence de respawn
            if (playerRespawn != null && !playerRespawn.IsRespawning) // IsRespawning doit être public dans PlayerRespawn
            {
                playerRespawn.StartCoroutine(playerRespawn.DeathSequence());
            }
            else
            {
                Debug.LogWarning("PlayerRespawn non assigné ou déjà en respawn !");
            }
        }
    }

    // -----------------------------
    // POSITION DES FOLLOWERS
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
            {
                targetPos += direction.normalized * (distance - followDistance);
            }

            follower.SetFormationPosition(targetPos);
            previousPos = follower.transform.position;
        }
    }

    // -----------------------------
    // NEARBY FOLLOWER
    // -----------------------------
    public void SetNearbyFollower(FollowerAI follower)
    {
        nearbyFollower = follower;
    }

    public void ClearNearbyFollower(FollowerAI follower)
    {
        if (nearbyFollower == follower)
            nearbyFollower = null;
    }

    // -----------------------------
    // FEEDBACK RECRUTEMENT
    // -----------------------------
    public void PlayJoinFeedback()
    {
        // Particules
        if (joinParticles != null)
            joinParticles.Play();

        // UI
        if (followerJoinUI != null)
        {
            if (uiRoutine != null)
                StopCoroutine(uiRoutine);

            uiRoutine = StartCoroutine(ShowJoinUI());
        }
    }

    private IEnumerator ShowJoinUI()
    {
        followerJoinUI.SetActive(true);

        Vector2 startScale2D = Vector2.one * 1f;
        Vector2 endScale2D = Vector2.one * 1f;
        float t = 0f;

        followerJoinUI.transform.localScale = new Vector3(startScale2D.x, startScale2D.y, 1f);

        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            Vector2 currentScale2D = Vector2.Lerp(startScale2D, endScale2D, t);
            followerJoinUI.transform.localScale = new Vector3(currentScale2D.x, currentScale2D.y, 1f);
            yield return null;
        }

        yield return new WaitForSeconds(followerJoinDuration);
        followerJoinUI.SetActive(false);
    }

    // -----------------------------
    // FEEDBACK FOLLOWER PERDU
    // -----------------------------
    public void PlayLostFeedback()
    {
        // Particules
        if (lostParticles != null)
            lostParticles.Play();

        // UI
        if (followerLostUI != null)
        {
            if (lostUiRoutine != null)
                StopCoroutine(lostUiRoutine);

            lostUiRoutine = StartCoroutine(ShowLostUI());
        }
    }

    private IEnumerator ShowLostUI()
    {
        followerLostUI.SetActive(true);

        Vector2 startScale2D = Vector2.one * 1f;
        Vector2 endScale2D = Vector2.one * 1f;
        float t = 0f;

        followerLostUI.transform.localScale = new Vector3(startScale2D.x, startScale2D.y, 1f);

        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            Vector2 currentScale2D = Vector2.Lerp(startScale2D, endScale2D, t);
            followerLostUI.transform.localScale = new Vector3(currentScale2D.x, currentScale2D.y, 1f);
            yield return null;
        }

        yield return new WaitForSeconds(followerLostDuration);
        followerLostUI.SetActive(false);
    }
}
