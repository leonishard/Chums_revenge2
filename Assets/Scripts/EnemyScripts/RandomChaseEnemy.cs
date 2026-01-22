using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ErraticChaseEnemy2D : MonoBehaviour
{
    [Header("Detection")]
    public float aggroRange = 8f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Erratic Motion")]
    public float offsetRadius = 1.5f;     // How far from the player the target can shift
    public float offsetChangeSpeed = 2f;  // How quickly the offset changes

    private Transform player;
    private Rigidbody2D rb;

    private Vector2 currentOffset;
    private Vector2 targetOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // important for top-down games
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        PickNewOffset();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > aggroRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Smoothly shift offset over time
        currentOffset = Vector2.Lerp(
            currentOffset,
            targetOffset,
            offsetChangeSpeed * Time.deltaTime
        );

        // Occasionally pick a new offset
        if (Vector2.Distance(currentOffset, targetOffset) < 0.1f)
        {
            PickNewOffset();
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 targetPosition = (Vector2)player.position + currentOffset;
        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    void PickNewOffset()
    {
        targetOffset = Random.insideUnitCircle * offsetRadius;
    }
}
