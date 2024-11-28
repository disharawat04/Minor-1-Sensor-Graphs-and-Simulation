using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
   
    public class Graph
    {
        public Dictionary<string, Dictionary<string, int>> Nodes = new Dictionary<string, Dictionary<string, int>>();
//node
        public void AddNode(string nodeName)
        {
            if (!Nodes.ContainsKey(nodeName))
            {
                Nodes[nodeName] = new Dictionary<string, int>();
            }
        }
//edge
        public void AddEdge(string fromNode, string toNode, int weight)
        {
            if (Nodes.ContainsKey(fromNode) && Nodes.ContainsKey(toNode))
            {
                Nodes[fromNode][toNode] = weight;
                Nodes[toNode][fromNode] = weight; 
            }
        }
    }

    // Dijkstra's Algorithm
    public Dictionary<string, int> FindShortestPaths(Graph graph, string startNode)
    {
        var distances = new Dictionary<string, int>();
        var priorityQueue = new SortedSet<(int, string)>();
        var visited = new HashSet<string>();

        
        foreach (var node in graph.Nodes.Keys)
        {
            distances[node] = int.MaxValue;
        }
        distances[startNode] = 0;

        
        priorityQueue.Add((0, startNode));

        while (priorityQueue.Count > 0)
        {
          
            var current = priorityQueue.Min;
            priorityQueue.Remove(current);

            string currentNode = current.Item2;

            if (visited.Contains(currentNode))
                continue;

            visited.Add(currentNode);

            foreach (var neighbor in graph.Nodes[currentNode])
            {
                string neighborNode = neighbor.Key;
                int weight = neighbor.Value;

                if (visited.Contains(neighborNode))
                    continue;

                int newDistance = distances[currentNode] + weight;
                if (newDistance < distances[neighborNode])
                {
                  
                    distances[neighborNode] = newDistance;
                    priorityQueue.Add((newDistance, neighborNode));
                }
            }
        }

        return distances;
    }

    void Start()
    {
       
        Graph graph = new Graph();
        graph.AddNode("A");
        graph.AddNode("B");
        graph.AddNode("C");
        graph.AddNode("D");

        graph.AddEdge("A", "B", 1);
        graph.AddEdge("A", "C", 4);
        graph.AddEdge("B", "C", 2);
        graph.AddEdge("B", "D", 5);
        graph.AddEdge("C", "D", 1);

        var shortestPaths = FindShortestPaths(graph, "A");

        
        foreach (var path in shortestPaths)
        {
            Debug.Log($"Node {path.Key} has a distance of {path.Value}");
        }
    }
}
