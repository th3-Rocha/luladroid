using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get reference to the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for left mouse button press (Mouse 1)
         if (Input.GetMouseButtonDown(0))
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack1");
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack2");
        }
    }
}