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
        MaxHealth
    }

    [Serializable]
    public class Effect
    {
        public EffectType type;

        [Tooltip("Used for Damage / Projectiles / Heal / MaxHealth. Can be negative.")]
        public int intAmount = 1;

        [Tooltip("Used for FireRate (timeBetweenFiring change). Can be negative.")]
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
                    if (stats != null) stats.AddDamage(e.intAmount); // negative = reduce damage
                    break;

                case EffectType.Projectiles:
                    if (stats != null) stats.AddProjectiles(e.intAmount); // negative = fewer projectiles
                    break;

                case EffectType.FireRate:
                    if (shooting != null) shooting.AddFireRate(e.floatAmount);
                    // IMPORTANT:
                    //  +0.03 => faster (timeBetweenFiring goes DOWN)
                    //  -0.03 => slower (timeBetweenFiring goes UP)
                    break;

                case EffectType.Heal:
                    if (health != null) health.Heal(e.intAmount); // negative = hurt
                    break;

                case EffectType.MaxHealth:
                    if (health != null) health.AddMaxHealth(e.intAmount); // negative = reduce max health
                    break;
            }
        }

        Destroy(gameObject);
    }
}
