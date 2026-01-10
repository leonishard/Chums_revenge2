using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int damage = 1;

    [Header("Stats")]
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }

    [Header("Hit Feedback")]
    public float hitFlashDuration = 0.1f;
    public Color hitColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isHit = false;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || isHit) return;

        CurrentHealth -= amount;
        StartCoroutine(HitFlash());

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        isHit = true;

        if (spriteRenderer != null)
            spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(hitFlashDuration);

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isHit = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
