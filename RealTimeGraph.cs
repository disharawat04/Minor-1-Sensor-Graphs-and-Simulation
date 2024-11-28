using UnityEngine;
using UnityEngine.UI;

public class RealTimeGraph : MonoBehaviour
{
    public RectTransform graphPanel;  // UI Panel for graph
    public LineRenderer lineRenderer;  // LineRenderer to draw the graph
    public float graphUpdateInterval = 0.1f;  // Time interval to update the graph
    public float graphHeight = 100f;  // Maximum height of the graph (adjust as needed)

    private float[] graphData;  // Stores graph data points (time series)
    private int dataIndex = 0;  // Current index of data array
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        graphData = new float[100];  // You can set the size of the graph (number of points to store)
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= graphUpdateInterval)
        {
            timeSinceLastUpdate = 0f;
            UpdateGraph();
        }
    }

    void UpdateGraph()
    {
        // This method will update the graph data array and draw it in real-time
        // Check for obstacles in the path of the character
        bool isObstacleDetected = DetectObstacle();  // You'll define this method below
        float graphValue = isObstacleDetected ? 1f : 0f;  // High value for obstacle detected, low otherwise

        // Add the graph data point (time series)
        graphData[dataIndex] = graphValue;

        // Update LineRenderer positions to represent the graph
        Vector3[] points = new Vector3[graphData.Length];
        for (int i = 0; i < graphData.Length; i++)
        {
            points[i] = new Vector3(i * 0.1f, graphData[i] * graphHeight, 0);  // Scaling for visual effect
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        // Increment data index (loop back when max size is reached)
        dataIndex = (dataIndex + 1) % graphData.Length;
    }

    bool DetectObstacle()
    {
        // Use a raycast to detect obstacles in front of the character
        // Adjust this based on your setup
        RaycastHit hit;
        Vector3 direction = transform.forward;
        if (Physics.Raycast(transform.position, direction, out hit, 10f))  // Adjust distance as needed
        {
            if (hit.collider.CompareTag("Obstacle"))  // Assume obstacles have a tag named "Obstacle"
            {
                return true;
            }
        }
        return false;
    }
}
