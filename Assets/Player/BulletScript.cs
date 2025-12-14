using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;

    public float force;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();

        // Get mouse position in world space
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Compute direction
        Vector3 direction = mousePos - transform.position;

        // Apply velocity
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;

        // Rotate bullet to face direction
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot + 90f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🔴 Enemy hit
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Destroy(collision.gameObject); // destroy enemy
            Destroy(gameObject);           // destroy bullet
            return;
        }

        // 🧱 Wall hit (OnTop tilemap layer)
        if (collision.gameObject.layer == LayerMask.NameToLayer("OnTop"))
        {
            Destroy(gameObject); // destroy bullet only
        }
    }
}
