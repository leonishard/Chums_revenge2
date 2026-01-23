using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIChase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseDistance = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isChasing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        // Sprite + Animator (usually on child, adjust if needed)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        isChasing = distance <= chaseDistance;
        animator.SetBool("isChasing", isChasing);
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // 🔁 Flip sprite ONLY when moving left/right
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }
    }
}
