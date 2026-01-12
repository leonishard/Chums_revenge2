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

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float maxKnockbackSpeed = 3f;
    public float knockbackDampTime = 0.1f;

    [Header("Drops")]
    [SerializeField] private GameObject coinPickupPrefab;
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 3;
    [SerializeField] private float dropScatter = 0.4f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isHit = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || isHit) return;

        CurrentHealth -= amount;
        StartCoroutine(HitFlash());
        ApplyKnockback();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback()
    {
        if (rb == null) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        // Push enemy away from player
        Vector2 knockDir = (transform.position - playerObj.transform.position);
        knockDir.Normalize();

        // Prevent stacking velocity into rockets
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxKnockbackSpeed);

        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxKnockbackSpeed);

        StartCoroutine(DampKnockback());
    }

    private IEnumerator DampKnockback()
    {
        if (rb == null) yield break;

        Vector2 startVel = rb.linearVelocity;
        float t = 0f;

        while (t < knockbackDampTime)
        {
            t += Time.deltaTime;
            rb.linearVelocity = Vector2.Lerp(startVel, Vector2.zero, t / knockbackDampTime);
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
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

    private void DropCoins()
    {
        if (coinPickupPrefab == null) return;

        int amount = Random.Range(minCoins, maxCoins + 1);

        // Spawn ONE pickup worth "amount"
        Vector3 pos = transform.position + (Vector3)Random.insideUnitCircle * dropScatter;
        GameObject go = Instantiate(coinPickupPrefab, pos, Quaternion.identity);

        CoinPickup coin = go.GetComponent<CoinPickup>();
        if (coin != null)
            coin.SetAmount(amount);

        // If you want multiple 1-coin pickups instead, use this instead:
        /*
        for (int i = 0; i < amount; i++)
        {
            Vector3 p = transform.position + (Vector3)Random.insideUnitCircle * dropScatter;
            Instantiate(coinPickupPrefab, p, Quaternion.identity);
        }
        */
    }

    private void Die()
    {
        DropCoins();
        Destroy(gameObject);
    }
}
