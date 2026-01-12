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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
            currentHealth = maxHealth;

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
        currency = 0;
    }
}
