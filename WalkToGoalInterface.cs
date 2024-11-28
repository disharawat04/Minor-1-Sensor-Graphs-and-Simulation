using UnityEngine;

public class WalkToGoalInterface : MonoBehaviour
{
    [Tooltip("Manually drag goal 1 here.")]
    public Transform goal1;
    [Tooltip("Manually drag goal 2 here.")]
    public Transform goal2;
    [Tooltip("Manually drag goal 3 here.")]
    public Transform goal3;
    [Tooltip("Manually drag goal 4 here.")]
    public Transform goal4;

    [Tooltip("Enable to show path in the scene view.")]
    public bool drawPath = false;

    [Tooltip("Set maximum speed. 1 is full running, 0.5 is fast walking.")]
    public float MaxSpeed = 0.4f;

    [Tooltip("Reference to the character to move.")]
    public Transform Character;

    public Transform CurrentGoal { get; private set; } 

    private WalkGoal characterWalkToGoal = null;
    public bool CharacterHasReachedGoal; 

    void Start()
    {
        if (Character != null)
        {
            characterWalkToGoal = Character.GetComponent<WalkGoal>();
            if (characterWalkToGoal == null)
            {
                Debug.LogError("WalkGoal component not found on the character.");
            }
        }
        else
        {
            Debug.LogError("Character is not assigned.");
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(120, 10, 100, 200));
        if (GUILayout.Button("Walk to Goal 1"))
        {
            CurrentGoal = goal1;
        }
        if (GUILayout.Button("Walk to Goal 2"))
        {
            CurrentGoal = goal2;
        }
        if (GUILayout.Button("Walk to Goal 3"))
        {
            CurrentGoal = goal3;
        }
        if (GUILayout.Button("Walk to Goal 4"))
        {
            CurrentGoal = goal4;
        }
        GUILayout.EndArea();
    }

    void Update()
    {
        if (CurrentGoal != null && characterWalkToGoal != null)
        {
            CharacterHasReachedGoal = characterWalkToGoal.WalkTo(CurrentGoal, drawPath, MaxSpeed, 0.3f, 120f);
        }
    }
}
