using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyShooting : MonoBehaviour
{
    [Header("Combat")]
    public GameObject bullet;
    public Transform bulletPos;
    public float shootInterval = 2f;
    public float range = 15f;

    [Header("Deactivation")]
    public float deactivateDelay = 10f;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    private Rigidbody2D rb;

    private float shootTimer;
    private float outOfRangeTimer;

    private bool isActive = false;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        anim.Play("ShootingIdle"); // start idle
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

    void Shoot()
    {
        if (isDead) return;

        anim.ResetTrigger("shoot");
        anim.SetTrigger("shoot");

        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    void Deactivate()
    {
        isActive = false;
        outOfRangeTimer = 0f;
        anim.SetBool("isActive", false);
    }

    // Call this from your health/damage system
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop normal animations
        anim.SetBool("isActive", false);
        anim.ResetTrigger("shoot");

        // Stop movement and collisions
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (col != null) col.enabled = false;

        // Trigger death animation using the die trigger
        anim.SetTrigger("die");
    }

    // Animation Event at the last frame of ShootingDeath
    public void HideAfterDeath()
    {
        sprite.enabled = false;
    }
}
