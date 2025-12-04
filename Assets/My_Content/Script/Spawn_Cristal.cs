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
            spawnPos.y -= 1.35f;

            Spawnobject = Instantiate(Prefab, spawnPos, Quaternion.identity);

            if (player != null)
            {
                player.currentCristal = this;
                player.canMove = false;
            }
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
        Destroy(gameObject);

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
