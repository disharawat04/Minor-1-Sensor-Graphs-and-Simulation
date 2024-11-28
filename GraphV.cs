using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GraphV : MonoBehaviour
{
    public CSVReader csvReader; // Reference to the CSVReader script
    public float graphScale = 1f; // Scale factor to adjust the graph size
    public Color lineColor = Color.green; // Color of the line graph
    private LineRenderer lineRenderer;

    // Variables for detecting obstacle spikes
    private float obstacleSpikeHeight = 10f; // Height of the spike when an obstacle is detected
    private bool isObstacleDetected = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (csvReader.timeData.Count > 0)
        {
            DrawGraph();
        }
        /*else
        {
            Debug.LogError("No data to visualize!");
        }*/
    }

    void Update()
    {
        if (isObstacleDetected)
        {
            SpikeGraph();
        }
        else
        {
            DrawGraph(); // Redraw the graph with normal values
        }
    }

    void DrawGraph()
    {
        // Set the number of points in the LineRenderer
        int pointCount = csvReader.timeData.Count;
        lineRenderer.positionCount = pointCount;

        // Set line color 
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Variables to store previous position
        Vector3 prevPosition = Vector3.zero;
        float totalDistance = 0f; // Total distance traveled by the character

        // Plot each point based on time (X) and distance (Y)
        for (int i = 0; i < pointCount; i++)
        {
            // current position from the CSV
            Vector3 currentPosition = csvReader.positionData[i];

            if (i > 0)
            {
                float distance = Vector3.Distance(prevPosition, currentPosition);
                totalDistance += distance;
            }

            // spike when an obstacle is detected
            if (isObstacleDetected)
            {
                totalDistance += obstacleSpikeHeight; // Increase the distance temporarily
            }

            // Scale the data for visualization 
            Vector3 position = new Vector3(csvReader.timeData[i], totalDistance * graphScale, 0);

            // Set the position in the LineRenderer
            lineRenderer.SetPosition(i, position);

            // Update the previous position for the next iteration
            prevPosition = currentPosition;
        }

        Debug.Log("Graph drawn!");
    }

    void SpikeGraph()
    {
       
        int spikePointIndex = GetSpikePointIndex();
        if (spikePointIndex >= 0 && spikePointIndex < csvReader.positionData.Count)
        {
            Vector3 spikePosition = csvReader.positionData[spikePointIndex] * graphScale;
            spikePosition.y += obstacleSpikeHeight;
            lineRenderer.SetPosition(spikePointIndex, spikePosition);
        }
    }

    int GetSpikePointIndex()
    {
        return csvReader.timeData.Count - 1;
    }

    public void SetObstacleDetected(bool detected)
    {
        isObstacleDetected = detected;
    }
}




