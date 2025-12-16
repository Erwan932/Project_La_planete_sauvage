using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    private Vector3 spawnPosition;

    [Header("Canvas Fade")]
    public GameObject deathCanvas;
    public CanvasFade canvasFade;

    [Header("Player")]
    public MonoBehaviour playerMovementScript;

    [Header("Crowd")]
    public CrowdManager crowdManager;

    [Header("DetectionZones")]
    public List<DetectionZone> detectionZones; // Tous les triggers à désactiver

    public bool isRespawning;
    public bool IsRespawning => isRespawning;

    private void Start()
    {
        if (CheckpointData.hasSavedPosition)
        {
            spawnPosition = CheckpointData.savedPosition;
            transform.position = spawnPosition;
        }
        else
        {
            spawnPosition = transform.position;
            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;
        }

        if (deathCanvas != null)
            deathCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            spawnPosition = other.transform.position;
            CheckpointData.savedPosition = spawnPosition;
            CheckpointData.hasSavedPosition = true;
        }

        if (other.CompareTag("Trap") && !isRespawning)
        {
            StartCoroutine(DeathSequence());
        }
    }

    // -----------------------------
    // DEATH / RESPAWN SEQUENCE
    // -----------------------------
    public IEnumerator DeathSequence()
    {
        isRespawning = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (deathCanvas != null)
            deathCanvas.SetActive(true);

        if (canvasFade != null)
            yield return StartCoroutine(canvasFade.FadeIn());

        foreach (var dz in detectionZones)
        {
            if (dz != null)
                dz.detectionEnabled = false;
        }

        transform.position = spawnPosition;

        ResetFollowers();

        yield return new WaitForSeconds(0.2f);

        if (canvasFade != null)
            yield return StartCoroutine(canvasFade.FadeOut());

        if (deathCanvas != null)
            deathCanvas.SetActive(false);

        foreach (var dz in detectionZones)
        {
            if (dz != null)
            {
                dz.detectionEnabled = true;
                dz.scanInProgress = false;
                if (dz.redOverlay != null)
                    dz.redOverlay.gameObject.SetActive(false);
            }
        }

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        isRespawning = false;
    }

    private void ResetFollowers()
    {
        if (crowdManager == null)
            return;

        foreach (FollowerAI follower in crowdManager.activeFollowers)
        {
            if (follower != null)
                follower.gameObject.SetActive(false);
        }

        crowdManager.activeFollowers.Clear();
    }
}
