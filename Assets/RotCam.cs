using UnityEngine;

public class RotCam : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 90f;
    public float minLookAngle = -90f;
    [Range(0, 1)] public float rotationSmoothing = 0.1f;

    [Header("References")]
    public Transform cameraTransform;

    private float xRotation;
    private float yRotation;
    private float currentXRotation;
    private float currentYRotation;
    private float xRotationVelocity;
    private float yRotationVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get raw mouse input (disable any acceleration in Input Manager)
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Calculate rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        // Apply smoothing using Mathf.SmoothDamp
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, rotationSmoothing);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, rotationSmoothing);

        // Apply rotations
        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        cameraTransform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
    }
}