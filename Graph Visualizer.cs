using System.Collections.Generic; 
using UnityEngine; 

public class GraphVisualizer : MonoBehaviour
{
    // Reference to the SensorDataProcessor
    [SerializeField] private SensorData dataProcessor;

    // LineRenderer to draw the graph
    private LineRenderer lineRenderer;

    // Graph scaling parameters
    [SerializeField] private float graphWidth = 10f; // Width 
    [SerializeField] private float graphHeight = 5f; // Height 

    // Offset for the graph position
    [SerializeField] private Vector3 graphOffset = Vector3.zero;

    // Data for the graph
    private List<SensorData.TimeDistancePair> distanceReadings;

    void Start()
    {
        // Ensure the LineRenderer component is attached
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = 0.1f; // Thickness 

        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
        lineRenderer.startColor = Color.green; 
        lineRenderer.endColor = Color.blue; 
    }

    void Update()
    {
        // Fetch data from the SensorDataProcessor
        if (dataProcessor != null)
        {
            distanceReadings = dataProcessor.GetDistanceReadings();
            RenderGraph(); // Update the graph with new data
        }
        else
        {
            Debug.LogError("SensorDataProcessor reference not assigned!");
        }
    }

    private void RenderGraph()
    {
        if (distanceReadings == null || distanceReadings.Count == 0)
        {
            Debug.LogWarning("No distance readings available for visualization.");
            return;
        }

        // Set the number of points in the LineRenderer to match the data
        lineRenderer.positionCount = distanceReadings.Count;

        // Calculate the maximum time and distance for normalization
        float maxTime = 0f;
        float maxDistance = 0f;

        foreach (var reading in distanceReadings)
        {
            if (reading.TimeStamp > maxTime) maxTime = reading.TimeStamp;
            if (reading.Distance > maxDistance) maxDistance = reading.Distance;
        }

        // Render each point in the graph
        for (int i = 0; i < distanceReadings.Count; i++)
        {
            var reading = distanceReadings[i];

            // Normalize the time and distance values to fit the graph's dimensions
            float normalizedX = (reading.TimeStamp / maxTime) * graphWidth;
            float normalizedY = (reading.Distance / maxDistance) * graphHeight;

            // Apply offset and create the graph point
            Vector3 point = new Vector3(normalizedX, normalizedY, 0) + graphOffset;

            // Set the position in the LineRenderer
            lineRenderer.SetPosition(i, point);
        }
    }

    public void ClearGraph()
    {
        lineRenderer.positionCount = 0; // Reset the LineRenderer
        Debug.Log("Graph cleared.");
    }
}
