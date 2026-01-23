using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;

    public float speed = 2f;
    public float idleTime = 0.75f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Transform currentPoint;

    private bool isIdle;
    private float idleTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    void Update()
    {
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;

            if (idleTimer <= 0f)
            {
                isIdle = false;
                anim.SetBool("isRunning", true);
            }

            return;
        }

        // Move towards current point
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            sprite.flipX = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            sprite.flipX = true;
        }

        // Reached patrol point → idle
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = (currentPoint == pointB.transform)
                ? pointA.transform
                : pointB.transform;

            StartIdle();
        }
    }

    void StartIdle()
    {
        isIdle = true;
        idleTimer = idleTime;

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isRunning", false);
    }
}
