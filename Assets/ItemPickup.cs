using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        DamageUp,
        FireRateUp,
        ProjectilesUp,
        Heal,         // NEW: heals current health
        MaxHealthUp   // OPTIONAL: increases max health
    }

    public ItemType type;

    [Header("Amounts")]
    public int intAmount = 1;         // used for damage/projectiles/heal/maxHealth
    public float floatAmount = 0.02f; // used for fire rate

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        Shooting shooting = other.GetComponentInChildren<Shooting>();
        PlayerHealth health = other.GetComponent<PlayerHealth>(); // NEW

        switch (type)
        {
            case ItemType.DamageUp:
                if (stats != null) stats.AddDamage(intAmount);
                break;

            case ItemType.ProjectilesUp:
                if (stats != null) stats.AddProjectiles(intAmount);
                break;

            case ItemType.FireRateUp:
                if (shooting != null) shooting.AddFireRate(floatAmount);
                break;

            case ItemType.Heal:
                if (health != null) health.Heal(intAmount);
                break;

            case ItemType.MaxHealthUp:
                if (health != null) health.AddMaxHealth(intAmount);
                break;
        }

        Destroy(gameObject);
    }
}
