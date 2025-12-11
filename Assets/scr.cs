using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePosition;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get movement input (WASD or Arrow Keys)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize so diagonal movement isn't faster
        moveInput.Normalize();

        // Get mouse position for aiming
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = moveInput * moveSpeed;

        // Rotate player to face mouse
        Vector2 lookDirection = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}