using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody2D rb;

    [Header("Combat")]
    public int damage = 1;

    [Header("Movement")]
    public float force = 12f;

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position);
        direction.Normalize();

        rb.linearVelocity = direction * force;

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot + 90f);

        Destroy(gameObject, 3f); // bullet lifetime fail-safe
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Enemy hit
        Enemy enemy = col.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Wall hit
        if (col.gameObject.layer == LayerMask.NameToLayer("OnTop"))
        {
            Destroy(gameObject);
        }
    }
}
