using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMerged : MonoBehaviour
{
    [Header("Combat")]
    public GameObject bullet;
    public Transform bulletPos;
    public float shootInterval = 2f;
    public float range = 15f;

    [Header("Stats")]
    public int maxHealth = 5;
    public int damage = 1;
    public int CurrentHealth { get; private set; }

    [Header("Hit Feedback")]
    public float hitFlashDuration = 0.1f;
    public Color hitColor = Color.red;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float maxKnockbackSpeed = 3f;
    public float knockbackDampTime = 0.1f;

    [Header("Drops")]
    public GameObject coinPickupPrefab;
    public int minCoins = 1;
    public int maxCoins = 3;
    public float dropScatter = 0.4f;

    [Header("Deactivation")]
    public float deactivateDelay = 10f;

    // Components
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private Transform player;

    // Timers
    private float shootTimer;
    private float outOfRangeTimer;

    // State
    private bool isActive = false;
    private bool isDead = false;
    private bool isHit = false;

    private Color originalColor;

    void Awake()
    {
        CurrentHealth = maxHealth;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (sprite != null)
            originalColor = sprite.color;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Start idle animation
        anim.Play("ShootingIdle");
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool playerInRange = distance <= range;

        // First-time activation
        if (!isActive && playerInRange)
        {
            isActive = true;
            anim.SetBool("isActive", true);
            outOfRangeTimer = 0f;
        }

        if (!isActive) return;

        // Active behavior
        if (playerInRange)
        {
            outOfRangeTimer = 0f;

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f;
            outOfRangeTimer += Time.deltaTime;

            if (outOfRangeTimer >= deactivateDelay)
            {
                Deactivate();
            }
        }
    }

    private void Shoot()
    {
        if (isDead) return;

        anim.ResetTrigger("shoot");
        anim.SetTrigger("shoot");

        if (bullet != null && bulletPos != null)
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    private void Deactivate()
    {
        isActive = false;
        outOfRangeTimer = 0f;
        anim.SetBool("isActive", false);
    }

    // --- DAMAGE & DEATH ---
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || isHit || isDead) return;

        CurrentHealth -= amount;
        StartCoroutine(HitFlash());
        ApplyKnockback();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        isHit = true;
        if (sprite != null)
            sprite.color = hitColor;

        yield return new WaitForSeconds(hitFlashDuration);

        if (sprite != null)
            sprite.color = originalColor;

        isHit = false;
    }

    private void ApplyKnockback()
    {
        if (rb == null || player == null) return;

        Vector2 knockDir = (transform.position - player.position).normalized;

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

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop active/shoot animations
        anim.SetBool("isActive", false);
        anim.ResetTrigger("shoot");

        // Stop movement & collisions
        rb.linearVelocity = Vector2.zero;
        if (col != null) col.enabled = false;

        // Trigger death animation
        anim.SetTrigger("die");

        // Drop coins immediately
        DropCoins();

        // Destroy after animation length (replace 1f with your actual animation length)
        StartCoroutine(DestroyAfterDeathAnimation(1f));
    }

    private IEnumerator DestroyAfterDeathAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void DropCoins()
    {
        if (coinPickupPrefab == null) return;

        int amount = Random.Range(minCoins, maxCoins + 1);
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = transform.position + (Vector3)Random.insideUnitCircle * dropScatter;
            GameObject go = Instantiate(coinPickupPrefab, pos, Quaternion.identity);

            Rigidbody2D coinRb = go.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                Vector2 dir = Random.insideUnitCircle.normalized;
                float force = Random.Range(1.5f, 3.5f);
                coinRb.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }
    }

    // Animation Event at the last frame of ShootingDeath
    public void HideAfterDeath()
    {
        if (sprite != null)
            sprite.enabled = false;
    }
}
