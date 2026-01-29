using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    // =====================
    // DEFAULT RUN STATS (edit in Inspector)
    // =====================
    [Header("Defaults (New Run)")]
    [SerializeField] private int defaultBaseDamage = 1;
    [SerializeField] private int defaultBonusDamage = 0;
    [SerializeField] private int defaultProjectileCount = 1;

    [SerializeField] private int defaultMaxHealth = 3;
    [SerializeField] private float defaultTimeBetweenFiring = 0.3f;

    [SerializeField] private int defaultMagazineSize = 10;
    [SerializeField] private float defaultReloadTime = 1.2f;

    [SerializeField] private int defaultCurrency = 0;

    // =====================
    // CURRENT RUN STATS (runtime)
    // =====================
    [Header("Current Run (Runtime)")]
    public int baseDamage;
    public int bonusDamage;
    public int projectileCount;

    public int maxHealth;
    public int currentHealth;

    public float timeBetweenFiring;

    public int magazineSize;
    public int currentAmmo;
    public float reloadTime;

    public int currency;

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

        // Fresh run on app start (run-stats only)
        ResetRun();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (I == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Push manager values into scene objects whenever a scene loads
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
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0) return true;
        if (currency < amount) return false;
        currency -= amount;
        return true;
    }

    // =====================
    // Run Reset (New Run)
    // =====================
    public void ResetRun()
    {
        baseDamage = Mathf.Max(0, defaultBaseDamage);
        bonusDamage = defaultBonusDamage;
        projectileCount = Mathf.Max(1, defaultProjectileCount);

        maxHealth = Mathf.Max(1, defaultMaxHealth);
        currentHealth = maxHealth;

        timeBetweenFiring = Mathf.Clamp(defaultTimeBetweenFiring, 0.05f, 1.5f);

        magazineSize = Mathf.Max(1, defaultMagazineSize);
        currentAmmo = magazineSize;

        reloadTime = Mathf.Clamp(defaultReloadTime, 0.1f, 5f);

        currency = Mathf.Max(0, defaultCurrency);
    }
}
