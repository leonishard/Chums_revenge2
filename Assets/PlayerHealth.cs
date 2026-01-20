using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [Header("Minimums")]
    public int minMaxHealth = 1;

    [Header("Invincibility")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyFromManager();

        if (GameManager.I == null)
        {
            currentHealth = maxHealth;
        }

        maxHealth = Mathf.Max(minMaxHealth, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
        SaveToManager();
    }

    public void ApplyFromManager()
    {
        if (GameManager.I == null) return;

        maxHealth = GameManager.I.maxHealth;
        currentHealth = GameManager.I.currentHealth;

        maxHealth = Mathf.Max(minMaxHealth, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0) currentHealth = maxHealth;
    }

    private void SaveToManager()
    {
        if (GameManager.I == null) return;

        GameManager.I.maxHealth = maxHealth;
        GameManager.I.currentHealth = currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;

        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.UpdateHearts(currentHealth);
        SaveToManager();

        if (currentHealth <= 0)
        {
            // Player is dead
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    // Now supports negative values too:
    public void Heal(int amount)
    {
        if (amount == 0) return;

        if (amount < 0)
        {
            // Treat negative heal as damage
            TakeDamage(-amount);
            return;
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthUI.UpdateHearts(currentHealth);
        SaveToManager();
    }

    // Now supports negative values too:
    public void AddMaxHealth(int amount)
    {
        if (amount == 0) return;

        maxHealth = Mathf.Max(minMaxHealth, maxHealth + amount);

        // If maxHealth decreased, currentHealth might now be too high
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // If maxHealth increased, optionally also increase current by same amount:
        if (amount > 0)
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);

        SaveToManager();
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        float blinkInterval = 0.1f;

        while (elapsed < invincibilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvincible = false;
    }
}
