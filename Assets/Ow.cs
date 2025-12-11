using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f;
    public int damage = 10;

    void Start()
    {
        // Destroy bullet after lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if bullet hit an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage to enemy (you'll need to implement this)
            // collision.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }

        // Destroy bullet on collision with walls
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}