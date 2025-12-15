using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Combat")]
    public int damage = 1;

    [Header("Movement")]
    public float speed = 12f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // If your bullet sprite points RIGHT by default, keep transform.right.
        // If it points UP by default, change to transform.up.
        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Enemy enemy = col.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("OnTop"))
        {
            Destroy(gameObject);
        }
    }
}
