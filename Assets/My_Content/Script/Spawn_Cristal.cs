using System.Collections;
using UnityEngine;

public class SpawnCristal : MonoBehaviour
{
    public GameObject Prefab;
    private bool IsSpawn = false;
    public PlayerMovement player;
    public GameObject Spawnobject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpawn && collision.CompareTag("Player"))
        {
            Vector3 spawnPos = collision.transform.position;
            spawnPos.y -= 1.35f;
            Spawnobject = Instantiate(Prefab, spawnPos, Quaternion.identity);
            player.canMove = false;
        }
    }

    public IEnumerator BlinkAndDestroy(GameObject obj, float blinkDuration, float blinkSpeed)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
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
        player.canMove = true;
    }
}
