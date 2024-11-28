using System.IO;
using UnityEngine;

public class PositionLogger : MonoBehaviour
{
    private string filePath;
    private StreamWriter writer;

    [Tooltip("Detection radius for obstacles.")]
    public float detectionRadius = 2f;

    [Tooltip("Tag for identifying obstacles.")]
    public string obstacleTag = "Obstacle";

    void Start()
    {
        filePath = Application.dataPath + "/PositionData.csv";

        writer = new StreamWriter(filePath, false);
        writer.WriteLine("Time,X,Y,Z,ObstacleDetected");
    }

    void Update()
    {
        //current position of the character
        Vector3 position = transform.position;

        // Check if an obstacle is detected
        bool obstacleDetected = CheckForObstacles();

        string data = $"{Time.time},{position.x},{position.y},{position.z},{(obstacleDetected ? 1 : 0)}";
        writer.WriteLine(data);
    }

    void OnApplicationQuit()
    {
        // Close the file when the application stops
        if (writer != null)
        {
            writer.Close();
        }
    }

    private bool CheckForObstacles()
    {
        // Check for objects within the detection radius
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var obj in nearbyObjects)
        {
            if (obj.CompareTag(obstacleTag))
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
