using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;   // ← IMPORTANT pour charger une scène

public class CrowdManager : MonoBehaviour
{
    public List<FollowerAI> recruitableFollowers;
    public List<FollowerAI> activeFollowers = new List<FollowerAI>();
    public List<FollowerAI> SavedFollowers = new List<FollowerAI>();
    public float followDistance = 0.5f;
    public FollowerAI nearbyFollower;
    public bool playerIsHidden = false;
    public int maxFollowers = 2;
    [Header("Feedback Recrutement")]
    public ParticleSystem joinParticles;
    public GameObject followerJoinUI;
    public float followerJoinDuration = 1f;
    private Coroutine uiRoutine;



    public void SetHidden(bool state)
    {
        playerIsHidden = state;
    }

    public bool IsPlayerHidden()
    {
        return playerIsHidden;
    }

    void Update()
    {
        // Recruter le follower proche si appui sur E
        if (Input.GetButtonDown("Fire1"))
            TryRecruitNearbyFollower();

        // Décrémenter la foule si le joueur prend des dégâts
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage();

        UpdateFollowers();
    }

public void TryRecruitNearbyFollower()
{
    if (nearbyFollower == null)
    {
        Debug.Log("Aucun follower à proximité");
        return;
    }

    if (activeFollowers.Count >= maxFollowers)
    {
        Debug.Log("Limite atteinte : 2 followers maximum");
        return;
    }

    if (recruitableFollowers.Contains(nearbyFollower))
    {
        // Ligne tooltip supprimée

        recruitableFollowers.Remove(nearbyFollower);
        activeFollowers.Add(nearbyFollower);

        nearbyFollower.gameObject.SetActive(true);
        nearbyFollower = null;

        Debug.Log("Follower recruté !");
        PlayJoinFeedback();
    }
}



    public void TakeDamage()
    {
        if (activeFollowers.Count > 0)
        {
            FollowerAI lost = activeFollowers[activeFollowers.Count - 1];
            activeFollowers.RemoveAt(activeFollowers.Count - 1);
            lost.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("GAME OVER");
            SceneManager.LoadScene("Menu_Mort"); // ← Chargement de la scène Game Over
        }
    }

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
                targetPos = (Vector2)follower.transform.position + direction.normalized * (distance - followDistance);
            }

            follower.SetFormationPosition(targetPos);
            previousPos = follower.transform.position;
        }
    }

    // Appelé depuis le FollowerAI quand le joueur entre dans le trigger
    public void SetNearbyFollower(FollowerAI follower)
    {
        nearbyFollower = follower;
    }

    // Appelé depuis le FollowerAI quand le joueur sort du trigger
    public void ClearNearbyFollower(FollowerAI follower)
    {
        if (nearbyFollower == follower)
            nearbyFollower = null;
    }
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

    Vector2 startScale2D = Vector2.one * 1f; // plus petit au départ
    Vector2 endScale2D = Vector2.one * 1f;   // taille finale réduite
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

}
