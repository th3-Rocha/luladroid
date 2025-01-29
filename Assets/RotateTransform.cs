using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    // Public variable to set rotation speed in the Unity Inspector
    public float speed = 100f;

    // FixedUpdate is called at a fixed time interval
    void FixedUpdate()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
