using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("Player Persistent Stats")]
    public int baseDamage = 1;
    public int bonusDamage = 0;
    public int projectileCount = 1;

    [Header("Player Persistent Health")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    [Header("Weapon / Combat")]
    public float timeBetweenFiring = 0.3f;

    // NEW: Ammo / Reload persistence
    public int magazineSize = 10;
    public int currentAmmo = 10;
    public float reloadTime = 1.2f;

    [Header("Currency")]
    public int currency = 0;

    public int Damage => baseDamage + bonusDamage;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        // Keep health sane
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
            currentHealth = maxHealth;

        // Keep weapon sane
        timeBetweenFiring = Mathf.Clamp(timeBetweenFiring, 0.05f, 1.5f);

        magazineSize = Mathf.Max(1, magazineSize);
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);
        if (currentAmmo == 0)
            currentAmmo = magazineSize;

        reloadTime = Mathf.Clamp(reloadTime, 0.1f, 5f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (I == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var stats = FindFirstObjectByType<PlayerStats>();
        if (stats != null)
            stats.ApplyFromManager();

        var health = FindFirstObjectByType<PlayerHealth>();
        if (health != null)
            health.ApplyFromManager();

        var shooting = FindFirstObjectByType<Shooting>();
        if (shooting != null)
            shooting.ApplyFromManager();
    }

    // =====================
    // Currency API
    // =====================

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        currency += amount;
        // Later: notify UI here
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return true;
        if (currency < amount) return false;

        currency -= amount;
        return true;
    }

    // =====================
    // Run / Game Reset
    // =====================

    public void ResetRun()
    {
        baseDamage = 1;
        bonusDamage = 0;
        projectileCount = 1;

        maxHealth = 3;
        currentHealth = maxHealth;

        timeBetweenFiring = 0.3f;

        // NEW: reset ammo / reload
        magazineSize = 10;
        currentAmmo = magazineSize;
        reloadTime = 1.2f;

        currency = 0;
    }
}
