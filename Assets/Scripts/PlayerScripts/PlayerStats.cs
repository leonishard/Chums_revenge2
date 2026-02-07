using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat")]
    public int baseDamage = 1;
    public int bonusDamage = 0;
    public int projectileCount = 1;

    [Header("Minimums")]
    public int minTotalDamage = 1;
    public int minProjectiles = 1;

    public int Damage => Mathf.Max(minTotalDamage, baseDamage + bonusDamage);

    private void Start()
    {
        ApplyFromManager();
        ClampStats();
    }

    public void ApplyFromManager()
    {
        if (GameManager.I == null) return;

        baseDamage = GameManager.I.baseDamage;
        bonusDamage = GameManager.I.bonusDamage;
        projectileCount = GameManager.I.projectileCount;

        ClampStats();
    }

    private void SaveToManager()
    {
        if (GameManager.I == null) return;

        GameManager.I.baseDamage = baseDamage;
        GameManager.I.bonusDamage = bonusDamage;
        GameManager.I.projectileCount = projectileCount;
    }

    private void ClampStats()
    {
        projectileCount = Mathf.Max(minProjectiles, projectileCount);

        // Ensure total damage doesn't fall below minTotalDamage by adjusting bonusDamage.
        int total = baseDamage + bonusDamage;
        if (total < minTotalDamage)
            bonusDamage += (minTotalDamage - total);
    }

    public void AddDamage(int amount)
    {
        bonusDamage += amount;
        ClampStats();
        SaveToManager();
    }

    public void AddProjectiles(int amount)
    {
        projectileCount += amount;
        ClampStats();
        SaveToManager();
    }
}
