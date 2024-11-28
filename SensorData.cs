using System.Collections.Generic; 
using UnityEngine; 

public class SensorData : MonoBehaviour
{
  
    public class TimeDistancePair
    {
        public float TimeStamp { get; private set; } 
        public float Distance { get; private set; } 

        public TimeDistancePair(float timeStamp, float distance)
        {
            TimeStamp = timeStamp;
            Distance = distance;
        }
    }

    
    private List<TimeDistancePair> distanceReadings = new List<TimeDistancePair>();

    private Dictionary<float, float> timeDistanceMap = new Dictionary<float, float>();

   
    private float elapsedTime = 0f; // Time in seconds since the simulation started
    private float initialDistance = 10f; // Initial distance from the sensor to the nearest object
    private float currentDistance; // Holds the latest distance value

    // Simulation parameters
    [SerializeField] private float simulationFrequency = 0.5f; // Frequency of data capture in seconds
    private float nextCaptureTime = 0f; // Time for the next capture

    void Start()
    {
        // Initialize the simulation with the first reading
        Debug.Log("Sensor simulation started. Initializing with default distance.");
        AddReading(elapsedTime, initialDistance);
    }

    void Update()
    {
        // Increment the elapsed time using Time.deltaTime
        elapsedTime += Time.deltaTime;

        // Capture data at specified intervals (controlled by simulationFrequency)
        if (elapsedTime >= nextCaptureTime)
        {
            // Simulate distance using a sine wave to represent dynamic changes
            currentDistance = initialDistance + Mathf.Sin(elapsedTime);

            // Add the simulated reading
            AddReading(elapsedTime, currentDistance);

            // Update the next capture time
            nextCaptureTime += simulationFrequency;

            // Optional: Trigger events or alerts based on the distance
            ProcessDistanceChange(currentDistance);
        }
    }

    
    /// <param name="time">The timestamp of the reading.</param>
    /// <param name="distance">The distance recorded.</param>
    private void AddReading(float time, float distance)
    {
        var newReading = new TimeDistancePair(time, distance); // Create a new reading
        distanceReadings.Add(newReading); // Add to the sequential list

        // Ensure unique time keys in the dictionary
        if (!timeDistanceMap.ContainsKey(time))
        {
            timeDistanceMap.Add(time, distance);
            Debug.Log($"New reading added: Time = {time:F2}, Distance = {distance:F2}");
        }
        else
        {
            Debug.LogWarning($"Duplicate timestamp detected: {time:F2}. Skipping.");
        }
    }


    /// <param name="distance">The current distance value.</param>
    private void ProcessDistanceChange(float distance)
    {
        if (distance < 5f)
        {
            Debug.Log("Obstacle detected nearby! Triggering avoidance behavior...");
        }
        else if (distance > 15f)
        {
            Debug.Log("Obstacle is far away. Safe to proceed.");
        }
    }

  
    /// <returns>A list of TimeDistancePair objects.</returns>
    public List<TimeDistancePair> GetDistanceReadings()
    {
        return distanceReadings;
    }

    /// <returns>A dictionary with time as key and distance as value.</returns>
    public Dictionary<float, float> GetTimeDistanceMap()
    {
        return timeDistanceMap;
    }
}
