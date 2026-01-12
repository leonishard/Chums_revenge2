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

    [Header("Weapon/Combat")]
    public float timeBetweenFiring = 0.3f;

    [Header("Currency (add later)")]
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

        // If starting values are inconsistent
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0) currentHealth = maxHealth;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (I == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find player in the new scene and apply stored values
        var player = FindFirstObjectByType<PlayerStats>();
        if (player != null)
        {
            player.ApplyFromManager();
        }

        var health = FindFirstObjectByType<PlayerHealth>();
        if (health != null)
        {
            health.ApplyFromManager();
        }

        // If you have a shooting script, do the same pattern there (see note at bottom)
    }

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
