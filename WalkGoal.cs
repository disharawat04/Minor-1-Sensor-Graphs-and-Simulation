using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkGoal : MonoBehaviour
{
    protected Animator animator;
    private Locomotion locomotion = null;
    Vector3[] PointsOnPath;
    int whereOnPath = 0;
    int nWaypoints = 0;
    Vector3 GoalPosition;
    Collider CurrentGoalCollider;
    Transform GoalLastFrame = null;
    Vector3 VectorToTarget;
    Vector3 targetSub;
    bool SlowingDown = false;
    float speed = 0;
    float direction = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        locomotion = new Locomotion(animator);
    }

    public bool WalkTo(Transform CurrentGoal, bool DrawPath, float maxspeed, float mindist, float anglecut)
    {
        bool GoalReached = false;
        if (CurrentGoal != GoalLastFrame)
        {
            SlowingDown = true;
            CurrentGoalCollider = CurrentGoal.GetComponent<Collider>();
        }
        GoalLastFrame = CurrentGoal;

        if (SlowingDown)
        {
            speed = speed * 0.95f;
            direction = 0;

            if (speed < 0.02)
            {
                SlowingDown = false;
                PointsOnPath = FindPathToGoal(transform.position, CurrentGoal.position);
                whereOnPath = 100;
                nWaypoints = PointsOnPath.GetLength(0);
            }
        }

        if (CurrentGoal != null)
        {
            if (Vector3.Magnitude(GoalPosition - CurrentGoal.position) > .01f)
            {
                PointsOnPath = FindPathToGoal(transform.position, CurrentGoal.position);
                whereOnPath = 100;
                nWaypoints = PointsOnPath.GetLength(0);
            }
            GoalPosition = CurrentGoal.position;
        }

        if (CurrentGoal != null && SlowingDown == false)
        {
            if (CurrentGoalCollider != null) { VectorToTarget = transform.position - CurrentGoalCollider.ClosestPointOnBounds(transform.position); }
            else VectorToTarget = transform.position - CurrentGoal.position;
            VectorToTarget.y = 0;

            float DistanceToTarget = VectorToTarget.magnitude;
            speed = 0;
            if (DistanceToTarget > mindist) { speed = (DistanceToTarget - mindist) * 0.2f; }
            if (speed > maxspeed) { speed = maxspeed; }
            if (DistanceToTarget <= mindist) { speed = 0f; GoalReached = true; }

            if (DistanceToTarget <= mindist + .2f) { speed = 0f; GoalReached = true; }
            if (whereOnPath < nWaypoints) { targetSub = PointsOnPath[whereOnPath]; }

            Vector3 VectorToTargetSub = transform.position - targetSub;
            VectorToTargetSub.y = 0;

            float DistanceToTargetSub = VectorToTargetSub.magnitude;
            if (DistanceToTargetSub < 0.5f + speed * 2)
            {
                whereOnPath = whereOnPath + 10;
            }
            if (whereOnPath > nWaypoints)
            {
                speed = 0;
            }

            Vector3 directionTotargetSub = transform.position - targetSub;
            directionTotargetSub = transform.InverseTransformDirection(directionTotargetSub);
            float angle = Mathf.Atan2(directionTotargetSub.z, directionTotargetSub.x) * Mathf.Rad2Deg;
            angle = angle + 90f;
            if (angle >= 180f) { angle = angle - 360f; }
            if (angle <= -180f) { angle = angle + 360f; }
            if (angle < -150 || angle > 150) { angle = 150; }
            direction = -angle;
        }

        if (DrawPath == true && nWaypoints > 0)
        {
            for (int i = 1; i < nWaypoints - 1; i++) { Debug.DrawLine(PointsOnPath[i], PointsOnPath[i + 1], Color.red); }
        }

        if (animator && CurrentGoal != null)
        {
            locomotion.Do(speed * 6, direction);
        }
        return GoalReached;
    }

    private Vector3[] FindPathToGoal(Vector3 origin, Vector3 goal)
    {
        Vector3[] PathCorners;
        Vector3[] PointsOnPath;
        UnityEngine.AI.NavMeshPath path;
        path = new UnityEngine.AI.NavMeshPath();
        UnityEngine.AI.NavMesh.CalculatePath(origin, goal, UnityEngine.AI.NavMesh.AllAreas, path);
        PathCorners = path.corners;
        PointsOnPath = FindSmoothPathBetweenPoints(PathCorners, 60);
        return PointsOnPath;
    }

    private Vector3[] FindSmoothPathBetweenPoints(Vector3[] CornerPoints, int smoothing)
    {
        int pathlength = 0;
        for (int i = 0; i < CornerPoints.Length - 1; i++)
        {
            Vector3 OnePointToNext = (CornerPoints[i + 1] - CornerPoints[i]);
            pathlength += (int)Mathf.Floor((OnePointToNext.magnitude) * 100);
        }
        Vector3[] PointsOnPath = new Vector3[pathlength];
        int counter = 0;

        for (int i = 0; i < CornerPoints.Length - 1; i++)
        {
            Vector3 OnePointToNext = CornerPoints[i + 1] - CornerPoints[i];
            int LengthThisPart = (int)Mathf.Floor((OnePointToNext.magnitude) * 100);
            for (int j = 0; j < LengthThisPart; j++)
            {
                Vector3 ThisPoint;
                ThisPoint = Vector3.Lerp(CornerPoints[i], CornerPoints[i + 1], (float)j / LengthThisPart);
                PointsOnPath[counter] = ThisPoint;
                counter++;
            }
        }

        Vector3[] PointsOnPath2 = PointsOnPath;
        for (int i = smoothing; i < counter - smoothing; i++)
        {
            PointsOnPath2[i] = (PointsOnPath[i - smoothing] + PointsOnPath[i + smoothing]) / 2;
        }
        return PointsOnPath2;
    }
}
