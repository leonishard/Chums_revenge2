using UnityEngine;
using System.Collections;

public class DoorTeleport : MonoBehaviour
{
    [Header("Where the player should appear")]
    [SerializeField] private Transform targetSpawnPoint;

    [Header("Room Enemies container (Room_X/Enemies)")]
    [SerializeField] private Transform enemiesRoot;

    [Header("Enemies must have this tag")]
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Optional: small cooldown to avoid re-triggering")]
    [SerializeField] private float cooldownSeconds = 0.2f;

    private bool _onCooldown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onCooldown) return;
        if (!other.CompareTag("Player")) return;
        if (targetSpawnPoint == null) return;

        // Locked until no ENEMY-tagged objects remain in this room
        if (RoomHasEnemies())
        {
            StartCoroutine(Cooldown());
            return;
        }

        other.transform.position = targetSpawnPoint.position;
        StartCoroutine(Cooldown());
    }

    private bool RoomHasEnemies()
    {
        if (enemiesRoot == null) return false;

        for (int i = 0; i < enemiesRoot.childCount; i++)
        {
            var child = enemiesRoot.GetChild(i);
            if (child != null && child.CompareTag(enemyTag))
                return true;
        }
        return false;
    }

    private IEnumerator Cooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        _onCooldown = false;
    }
}
