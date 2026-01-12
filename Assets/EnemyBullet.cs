using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;   // Bullet movement speed
    public int damage = 1;      // Damage to player
    public float lifetime = 3f; // How long before bullet auto-destroys

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EnemyBullet: Rigidbody2D is missing!");
            return;
        }

        // Find player safely
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;

            // Compute direction as Vector2 explicitly
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * speed;

            // Rotate bullet to face movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Use UnityEngine.Object.Destroy explicitly to avoid ambiguity
        UnityEngine.Object.Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            UnityEngine.Object.Destroy(gameObject);
        }

        // Optional: destroy bullet on walls
        // if (other.CompareTag("Obstacle"))
        // {
        //     UnityEngine.Object.Destroy(gameObject);
        // }
    }
}
