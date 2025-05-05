
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
using System;
using System.Collections.Generic;

namespace MapRouting.Handler
{
    public class HandleBestRoute
    {
        //      the output for each query should be like this:
        //      The path nodes : 0, 3, 4, 5, 2                              ✔️ Done
        //      Shortest time = 4.63 mins                                   ✔️ Done
        //      Path length = 1.72 km                                       ✔️ Done
        //          ->  Total walking distance = 0.28 km                    ✔️ Done
        //          ->  Total roads length = 1.44 km                        ✔️ Done
        public static (string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength) findBestStartAndEndNode(int nodesCount, Query query, Graph graph)
        {


            List<Node> sourceNodes = HelperFunctions.GetNearbyNodes(query.SourceX, query.SourceY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to start with
            List<Node> destinationNodes = HelperFunctions.GetNearbyNodes(query.DestinationX, query.DestinationY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to end with
            if (sourceNodes.Count == 0 || destinationNodes.Count == 0)
            {
                return ("No Path found", 0.0, 0.0, 0.0, 0.0);
            }
            double minTime = double.MaxValue;
            string outputPath = "";
            double distance = 0.0;
            double walkingDistanceToDestinationNode;
            double walkingTimeToDestinationNode;
            double finalWalkingDistance = 0.0;
            double walkingDistanceToSourceNode;
            double walkingTimeToSourceNode;
            Dictionary<int, Tuple<double, double>> allSourceNodesData = new Dictionary<int, Tuple<double, double>>();  // node Id -> { walking distance, walking time }
            Dictionary<int, Tuple<double, double>> allDestinationNodesData = new Dictionary<int, Tuple<double, double>>();  // node Id -> { walking distance, walking time }

            foreach (Node sourceNode in sourceNodes)
            {
                walkingDistanceToSourceNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
                walkingTimeToSourceNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToSourceNode);
                allSourceNodesData.Add(sourceNode.Id, new Tuple<double, double>(walkingDistanceToSourceNode, walkingTimeToSourceNode));
            }
            foreach (Node destinationNode in destinationNodes)
            {
                walkingDistanceToDestinationNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
                walkingTimeToDestinationNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToDestinationNode);
                allDestinationNodesData.Add(destinationNode.Id, new Tuple<double, double>(walkingDistanceToDestinationNode, walkingTimeToDestinationNode));
            }

            object lockObject = new object();

            // try all neighbors nodes 
            foreach(Node sourceNode in sourceNodes)
            {
                (walkingDistanceToSourceNode, walkingTimeToSourceNode) = allSourceNodesData[sourceNode.Id];
                foreach (Node destinationNode in destinationNodes)
                {
                    (walkingDistanceToDestinationNode, walkingTimeToDestinationNode) = allDestinationNodesData[destinationNode.Id];
                    if (walkingTimeToSourceNode + walkingTimeToDestinationNode >= minTime)
                        continue;
                    (string path, double carTime, double allDistance) = OptimalAlgorithm.detectShortestPath(nodesCount, sourceNode.Id, destinationNode.Id, graph.AdjacencyList);

                    lock (lockObject)
                    {
                        double totalTime = carTime + walkingTimeToSourceNode + walkingTimeToDestinationNode;

                        if (totalTime < minTime)
                        {
                            minTime = totalTime;
                            outputPath = path;
                            distance = allDistance + walkingDistanceToSourceNode + walkingDistanceToDestinationNode;
                            finalWalkingDistance = walkingDistanceToSourceNode + walkingDistanceToDestinationNode;
                        }
                    }
                }
            };
            if (minTime == double.MaxValue)
            {
                return ("No Path found", 0.0, 0.0, 0.0, 0.0);
            }
            return (outputPath, minTime * 60.0, distance, finalWalkingDistance, (distance - finalWalkingDistance));
        }

    }
}