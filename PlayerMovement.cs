/*using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    private Animator animator; // Reference to the Animator component
    private CharacterController characterController;

    private void Start()
    {
        // Get the Animator and CharacterController components
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Trigger walking animation manually
            animator.Play("Walking With Shopping Bag");

            // Move character forward
            Vector3 moveDirection = transform.forward * speed * Time.deltaTime;
            characterController.Move(moveDirection);
        }
        else
        {
            // Trigger idle animation manually
            animator.Play("Standing W_Briefcase Idle");
        }
    }
}
*/


using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    private Animator animator; // Reference to the Animator component
    private NavMeshAgent agent; // Reference to the NavMeshAgent
    public Transform goal; // The goal the player needs to walk to

    private void Start()
    {
        // Get the Animator and NavMeshAgent components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Ensure NavMeshAgent settings are correct
        agent.speed = speed; // Set speed of NavMeshAgent
        agent.stoppingDistance = 1f; // Stop at the goal position

        if (goal != null)
        {
            // Set the destination for the NavMeshAgent
            agent.SetDestination(goal.position);
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Move character towards goal while avoiding obstacles
            animator.Play("Walking With Shopping Bag");
            agent.isStopped = false; // Ensure movement is not stopped

            // Optionally, you can update the destination if you want dynamic changes
            if (goal != null)
            {
                agent.SetDestination(goal.position); // Update goal position if needed
            }
        }
        else
        {
            // If not pressing "W", stop the character
            animator.Play("Standing W_Briefcase Idle");
            agent.isStopped = true; // Stop movement when "W" is not pressed
        }
    }
}
