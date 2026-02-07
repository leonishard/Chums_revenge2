using UnityEngine;
using System.Collections;

public class DoorTeleport : MonoBehaviour
{
    [Header("Where the player should appear")]
    [SerializeField] private Transform targetSpawnPoint;

    [Header("Room Enemies container (optional). If not set: door is always open.")]
    [SerializeField] private Transform enemiesRoot;

    [Header("Sprite that blocks the doorway (disable when room is clear / open)")]
    [SerializeField] private SpriteRenderer blockedSprite; // drag your “base/closed” sprite here (optional)

    [Header("Optional: small cooldown to avoid re-triggering")]
    [SerializeField] private float cooldownSeconds = 0.2f;

    private bool _onCooldown;

    private void Awake()
    {
        UpdateDoorVisual();
    }

    private void Update()
    {
        UpdateDoorVisual();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onCooldown) return;
        if (!other.CompareTag("Player")) return;
        if (targetSpawnPoint == null) return;

        // If no enemiesRoot assigned, door is always open
        if (!IsRoomClear())
        {
            StartCoroutine(Cooldown());
            return;
        }

        other.transform.position = targetSpawnPoint.position;
        StartCoroutine(Cooldown());
    }

    private bool IsRoomClear()
    {
        // No enemies container assigned => treat as clear (open room)
        if (enemiesRoot == null) return true;

        // You destroy enemies, so clear = no children left
        return enemiesRoot.childCount == 0;
    }

    private void UpdateDoorVisual()
    {
        if (blockedSprite == null) return;

        // Blocked sprite visible only when NOT clear
        blockedSprite.enabled = !IsRoomClear();
    }

    private IEnumerator Cooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        _onCooldown = false;
    }
}
