using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // If SpriteRenderer is on the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If SpriteRenderer is on a child object instead, use this:
        // spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize to prevent faster diagonal movement
        moveInput.Normalize();

        // Running animation
        bool isRunning = moveInput.x != 0 || moveInput.y != 0;
        animator.SetBool("isRunning", isRunning);

        // 🔁 Flip sprite based on horizontal direction
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
