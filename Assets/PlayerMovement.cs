using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float slideSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float dashForce = 15f;
    public float slideDuration = 1f;
    public float dashCooldown = 1.5f;
    public float slideCooldown = 2f;

    public Transform cameraTrans;

    private CharacterController characterController;
    private Vector3 velocity;
    public bool isGrounded;
    private bool isDashing;
    private bool isSliding;
    private float dashTimer;
    private float slideTimer;
    private float lastDashTime;
    private float lastSlideTime;
    private Vector3 slideDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleDash();
        HandleSlide();
        ApplyGravity();
    }

    private void HandleGroundCheck()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = Vector3.zero;

        if (isSliding)
        {
            move = slideDirection * 1;
        }
        else if (!isDashing) // Only allow movement input when not dashing
        {
            move = transform.right * moveX + transform.forward * moveZ;
        }

        characterController.Move(move * walkSpeed * Time.deltaTime);
    }

    private void HandleDash()
    {
        if (Input.GetButton("Fire3") && Time.time > lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTimer = 0f;

            // Get raw input for precise direction control
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Calculate combined dash direction
            Vector3 inputDirection = Vector3.zero;

            if (horizontalInput != 0 || verticalInput != 0)
            {
                // Create direction vector from inputs
                inputDirection = (transform.right * horizontalInput) +
                               (transform.forward * verticalInput);

                // Normalize to prevent faster diagonal movement
                inputDirection.Normalize();
            }
            else
            {
                // Default forward direction if no input
                inputDirection = transform.forward;
            }

            // Preserve vertical velocity
            float verticalVelocity = velocity.y;

            // Apply dash force
            velocity = inputDirection * dashForce;
            velocity.y = verticalVelocity;

            lastDashTime = Time.time;
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > 0.2f)
            {
                isDashing = false;
                velocity.x = 0;
                velocity.z = 0;
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isSliding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    private void HandleSlide()
    {
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            StartSlide();
        }
        else
        {
            EndSlide();

        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideDirection = transform.forward; // Set the initial direction of the slide

        cameraTrans.localPosition = new Vector3(0, -1f, 0); // Adjust camera position
    }

    private void EndSlide()
    {
        isSliding = false;
        cameraTrans.localPosition = new Vector3(0, 0, 0); // Reset camera position
    }

    private void ApplyGravity()
    {
        if (!isDashing) // Only apply gravity when not dashing
        {
            velocity.y += gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
