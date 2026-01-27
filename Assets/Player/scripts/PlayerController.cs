using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private bool playingFootsteps = false;
    private AudioManager audioManager;
    public float footstepSpeed = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>(); // Make sure AudioManager exists in scene
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

        // Start/Stop footsteps based on movement
        if (isRunning && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if (!isRunning && playingFootsteps)
        {
            StopFootsteps();
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }

    void PlayFootstep()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.footstep);
    }
}
