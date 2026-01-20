using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum EffectType
    {
        Damage,
        FireRate,     // uses floatAmount
        Projectiles,
        Heal,
        MaxHealth,

        MaxAmmo,      // magazine size change (intAmount)
        ReloadSpeed,  // reload speed change (floatAmount) - positive = faster
        Ammo          // adds/removes bullets in current mag (intAmount)
    }

    [Serializable]
    public class Effect
    {
        public EffectType type;

        [Tooltip("Used for Damage / Projectiles / Heal / MaxHealth / MaxAmmo / Ammo. Can be negative.")]
        public int intAmount = 1;

        [Tooltip("Used for FireRate (timeBetweenFiring change) and ReloadSpeed (reloadTime change). Can be negative.")]
        public float floatAmount = 0.02f;
    }

    [Header("This item can apply multiple effects")]
    public List<Effect> effects = new List<Effect>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        Shooting shooting = other.GetComponentInChildren<Shooting>();
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        foreach (var e in effects)
        {
            switch (e.type)
            {
                case EffectType.Damage:
                    if (stats != null) stats.AddDamage(e.intAmount);
                    break;

                case EffectType.Projectiles:
                    if (stats != null) stats.AddProjectiles(e.intAmount);
                    break;

                case EffectType.FireRate:
                    if (shooting != null) shooting.AddFireRate(e.floatAmount);
                    // + => faster (timeBetweenFiring down), - => slower (timeBetweenFiring up)
                    break;

                case EffectType.Heal:
                    if (health != null) health.Heal(e.intAmount);
                    break;

                case EffectType.MaxHealth:
                    if (health != null) health.AddMaxHealth(e.intAmount);
                    break;

                case EffectType.MaxAmmo:
                    if (shooting != null) shooting.AddMagazineSize(e.intAmount);
                    break;

                case EffectType.ReloadSpeed:
                    if (shooting != null) shooting.AddReloadSpeed(e.floatAmount);
                    // + => faster reload (reloadTime down), - => slower reload (reloadTime up)
                    break;

                case EffectType.Ammo:
                    if (shooting != null) shooting.AddAmmo(e.intAmount);
                    break;
            }
        }

        Destroy(gameObject);
    }
}
