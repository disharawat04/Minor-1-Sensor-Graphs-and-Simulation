using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    private NavMeshAgent agent; // Reference to the NavMeshAgent
    private Animator animator;  // Reference to the Animator

    [Tooltip("Reference to the WalkToGoalInterface script.")]
    public WalkToGoalInterface goalInterface;

    [Tooltip("Animation name for walking.")]
    public string walkingAnimation = "Walking With Shopping Bag";

    [Tooltip("Animation name for idle.")]
    public string idleAnimation = "Standing W_Briefcase Idle";

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the GameObject!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the GameObject!");
        }

        if (goalInterface == null)
        {
            Debug.LogError("WalkToGoalInterface script is not assigned!");
        }
    }

    void Update()
    {
        // current goal from the WalkToGoalInterface script
        if (goalInterface != null && goalInterface.CurrentGoal != null)
        {
            // Set the destination to the current goal's position
            agent.SetDestination(goalInterface.CurrentGoal.position);

            // Update the animation
            UpdateAnimationState();
        }
        else
        {
            Debug.LogWarning("CurrentGoal is not set in the WalkToGoalInterface script!");
        }
    }

    private void UpdateAnimationState()
    {
        //  NavMeshAgent movement
        if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkingAnimation))
            {
                animator.Play(walkingAnimation);
            }
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleAnimation))
            {
                animator.Play(idleAnimation);
            }
        }
    }
}
