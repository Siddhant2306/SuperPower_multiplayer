using UnityEngine;

enum PlayerState
{
    Grounded,
    Jumped
}
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject fireballPrefab;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSpeed = 2f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpVelocity = 6f;
    [SerializeField] private float gravity = -20f;

    [Header("Dash")]
    [SerializeField] private float dashSpeedMultiplier = 2.5f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 2f;

    private CharacterController controller;
    public Vector3 velocity;
    private Vector2 mouseLook;
    private float dashTimer;
    private float dashCooldownTimer;
    private float currentSpeed;
    PlayerState currentState = PlayerState.Grounded;
    bool isGrounded;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMouseLook();
        HandleDash();
        HandleMovement();
        HandleJump();
        HandleFireball();
    }


    void HandleMouseLook()
    {
        mouseLook.x += Input.GetAxis("Mouse X") * lookSpeed;
        mouseLook.y -= Input.GetAxis("Mouse Y") * lookSpeed;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -80f, 80f);

        transform.rotation = Quaternion.Euler(0f, mouseLook.x, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(mouseLook.y, 0f, 0f);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();

        velocity.x = move.x * currentSpeed;
        velocity.z = move.z * currentSpeed;
    }

    void HandleJump()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            currentState = PlayerState.Grounded;
            velocity.y = -2f; 
        }
        else
        {
            isGrounded = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState == PlayerState.Grounded)
        {
            currentState = PlayerState.Jumped;
            velocity.y = jumpVelocity;
            isGrounded = false;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    void HandleDash()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            currentSpeed = Mathf.Lerp(
                moveSpeed * dashSpeedMultiplier,
                moveSpeed,
                1f - (dashTimer / dashDuration)
            );
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }
    void HandleFireball()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vector3 spawnPos = transform.position + transform.forward;
            Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        }
    }

}
