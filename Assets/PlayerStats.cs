using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat")]
    public int baseDamage = 1;
    public int bonusDamage = 0;

    public int projectileCount = 1;

    public int Damage => baseDamage + bonusDamage;

    public void AddDamage(int amount)
    {
        bonusDamage += amount;
    }

    public void AddProjectiles(int amount)
    {
        projectileCount += amount;
    }
}
