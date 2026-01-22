using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomWanderEnemy2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Timing")]
    public float moveDuration = 1.5f;   // how long to move in one direction
    public float idleDuration = 0.5f;   // pause between moves

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private float timer;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        // If SpriteRenderer is on the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If it's on a child instead, use this:
        // spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        PickNewDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (isMoving)
            {
                StopMoving();
            }
            else
            {
                PickNewDirection();
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * moveSpeed;

            // 🔁 Flip only when moving left/right
            if (Mathf.Abs(moveDirection.x) > 0.01f)
            {
                spriteRenderer.flipX = moveDirection.x < 0f;
            }
        }
    }

    void PickNewDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        timer = moveDuration;
        isMoving = true;
    }

    void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        timer = idleDuration;
        isMoving = false;
    }
}
