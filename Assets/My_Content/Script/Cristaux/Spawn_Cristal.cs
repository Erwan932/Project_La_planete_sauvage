using System.Collections;
using UnityEngine;

public class SpawnCristal : MonoBehaviour
{
    public GameObject Prefab;
    private bool IsSpawn = false;

    public PlayerMovement player;
    public GameObject Spawnobject;

    private bool isConsuming = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpawn && collision.CompareTag("Player"))
        {
            IsSpawn = true;

            Vector3 spawnPos = collision.transform.position;
            spawnPos.y -= 1.4f;

            Spawnobject = Instantiate(Prefab, spawnPos, Quaternion.identity);

            if (player != null)
            {
                player.currentCristal = this;
                player.canMove = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 🔥 Quand le joueur sort → tout reset !
            ResetSpawn();
        }
    }

    public IEnumerator BlinkAndDestroy(GameObject obj, float blinkDuration, float blinkSpeed)
    {
        if (isConsuming) yield break;
        isConsuming = true;

        if (obj == null)
        {
            RestorePlayerControl();
            yield break;
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            RestorePlayerControl();
            yield break;
        }

        float timer = 0f;

        while (timer < blinkDuration)
        {
            sr.enabled = !sr.enabled;
            timer += blinkSpeed;
            yield return new WaitForSeconds(blinkSpeed);
        }

        sr.enabled = true;

        Destroy(obj);

        RestorePlayerControl();
    }

    private void ResetSpawn()
    {
        // 🔹 Reset Flags
        IsSpawn = false;
        isConsuming = false;

        // 🔹 Si un cristal existe encore, on le détruit
        if (Spawnobject != null)
        {
            Destroy(Spawnobject);
            Spawnobject = null;
        }

        // 🔹 Le trigger doit rester pour refonctionner
        // donc on NE détruit PAS "gameObject" ici

        RestorePlayerControl();
    }

    private void RestorePlayerControl()
    {
        if (player != null)
        {
            player.currentCristal = null;
            player.canMove = true;
        }
    }
}
