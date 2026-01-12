using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat")]
    public int baseDamage = 1;
    public int bonusDamage = 0;
    public int projectileCount = 1;

    public int Damage => baseDamage + bonusDamage;

    private void Start()
    {
        ApplyFromManager();
    }

    public void ApplyFromManager()
    {
        if (GameManager.I == null) return;

        baseDamage = GameManager.I.baseDamage;
        bonusDamage = GameManager.I.bonusDamage;
        projectileCount = GameManager.I.projectileCount;
    }

    private void SaveToManager()
    {
        if (GameManager.I == null) return;

        GameManager.I.baseDamage = baseDamage;
        GameManager.I.bonusDamage = bonusDamage;
        GameManager.I.projectileCount = projectileCount;
    }

    public void AddDamage(int amount)
    {
        bonusDamage += amount;
        SaveToManager();
    }

    public void AddProjectiles(int amount)
    {
        projectileCount += amount;
        SaveToManager();
    }
}
