using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        DamageUp,
        FireRateUp,
        ProjectilesUp
    }

    public ItemType type;
    public int intAmount = 1;         // for damage/projectiles
    public float floatAmount = 0.02f; // for fire rate (seconds reduced)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // PlayerStats is on Player
        PlayerStats stats = other.GetComponent<PlayerStats>();

        // Shooting is on child/pivot (your setup)
        Shooting shooting = other.GetComponentInChildren<Shooting>();

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
        }

        Destroy(gameObject);
    }
}
