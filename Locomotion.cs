using UnityEngine;

public class Locomotion
{
    private Animator animator;

    // Constructor to initialize the Animator
    public Locomotion(Animator animator)
    {
        this.animator = animator;
    }

    // Method to control movement and direction
    public void Do(float speed, float direction)
    {
        if (animator != null)
        {
            // Assuming you have parameters "Speed" and "Direction" in your Animator
            animator.SetFloat("Speed", speed);
            animator.SetFloat("Direction", direction);
        }
    }
}
