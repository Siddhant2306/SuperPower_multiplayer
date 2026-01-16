using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


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
    private Direction direction;


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
        float inputZ = Input.GetAxis("Vertical");  
        float inputX = Input.GetAxis("Horizontal"); 

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDir = (forward * inputZ + right * inputX).normalized;

        velocity.x = moveDir.x * currentSpeed;
        velocity.z = moveDir.z * currentSpeed;
    }

    void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; 
        }
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = jumpVelocity;
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
            Vector3 spawnPos =
                playerCamera.transform.position +
                playerCamera.transform.forward * 1f;

            Instantiate(
                fireballPrefab,
                spawnPos,
                playerCamera.transform.rotation
            );
        }
    }

}
