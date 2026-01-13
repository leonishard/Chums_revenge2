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

    private Vector2 moveDirection;
    private float timer;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
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
