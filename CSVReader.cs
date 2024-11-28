using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public string filePath = "PositionData.csv"; // Relative to the Assets folder

    public List<float> timeData = new List<float>();
    public List<Vector3> positionData = new List<Vector3>();
    public List<int> obstacleData = new List<int>(); // Stores obstacle detection data

    void Start()
    {
        string fullPath = Application.dataPath + "/" + filePath;

        if (File.Exists(fullPath))
        {
            using (StreamReader reader = new StreamReader(fullPath))
            {
                bool isHeader = true;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Skip the header
                    if (isHeader)
                    {
                        isHeader = false;
                        continue;
                    }

                    // Parse the line
                    string[] values = line.Split(',');

                    if (values.Length == 5) 
                    {
                        float time = float.Parse(values[0]);
                        float x = float.Parse(values[1]);
                        float y = float.Parse(values[2]);
                        float z = float.Parse(values[3]);
                        int obstacleDetected = int.Parse(values[4]);

                        timeData.Add(time);
                        positionData.Add(new Vector3(x, y, z));
                        obstacleData.Add(obstacleDetected); // Add obstacle data
                    }
                }
            }

            Debug.Log("CSV data loaded successfully!");
        }
        else
        {
            Debug.LogError("CSV file not found at " + fullPath);
        }
    }
}


