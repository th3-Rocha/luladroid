using UnityEngine;
using Unity.Cinemachine;

public class CamRotationCinemachine : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Sensitivity for mouse movement
    [System.Obsolete]
    public CinemachineFreeLook cinemachineFreeLook; // Reference to the CinemachineFreeLook component

    private float xRotation = 0f; // Tracks vertical rotation (camera pitch)

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input for X and Y axes
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera vertically (X-axis rotation)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation

        // Update CinemachineFreeLook's LookAt and Up directions
        cinemachineFreeLook.m_XAxis.Value = mouseX;
        cinemachineFreeLook.m_YAxis.Value = xRotation; 
    }
}