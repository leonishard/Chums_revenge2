using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Where the player should appear")]
    [SerializeField] private Transform targetSpawnPoint;

    [Header("Optional: freeze input briefly to avoid re-triggering")]
    [SerializeField] private float cooldownSeconds = 0.2f;

    private bool _onCooldown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onCooldown) return;

        // Tag your player as "Player"
        if (!other.CompareTag("Player")) return;

        // Move player
        other.transform.position = targetSpawnPoint.position;

        // Optional: small cooldown so you don't instantly trigger again
        StartCoroutine(Cooldown());
    }

    private System.Collections.IEnumerator Cooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        _onCooldown = false;
    }
}
