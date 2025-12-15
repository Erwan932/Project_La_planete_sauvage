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

        // ⛔ Bloquer les inputs joueur
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // 🟥 Activer le canvas
        if (deathCanvas != null)
            deathCanvas.SetActive(true);

        // 🔴 Fade IN
        if (canvasFade != null)
            yield return StartCoroutine(canvasFade.FadeIn());

        // 🚫 Désactiver toutes les DetectionZones
        foreach (var dz in detectionZones)
        {
            if (dz != null)
                dz.detectionEnabled = false;
        }

        // 🔁 Respawn caché
        transform.position = spawnPosition;

        // 👥 Reset complet des followers
        ResetFollowers();

        // 🟢 Petite pause optionnelle
        yield return new WaitForSeconds(0.2f);

        // 🟩 Fade OUT
        if (canvasFade != null)
            yield return StartCoroutine(canvasFade.FadeOut());

        // 🟦 Désactiver le canvas
        if (deathCanvas != null)
            deathCanvas.SetActive(false);

        // ✅ Réactiver les DetectionZones
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

        // ✅ Rendre les inputs joueur
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        isRespawning = false;
    }

    // -----------------------------
    // RESET FOLLOWERS (sans toucher CrowdManager)
    // -----------------------------
    private void ResetFollowers()
    {
        if (crowdManager == null)
            return;

        // Désactiver tous les followers actifs
        foreach (FollowerAI follower in crowdManager.activeFollowers)
        {
            if (follower != null)
                follower.gameObject.SetActive(false);
        }

        // Vider la liste
        crowdManager.activeFollowers.Clear();
    }
}
