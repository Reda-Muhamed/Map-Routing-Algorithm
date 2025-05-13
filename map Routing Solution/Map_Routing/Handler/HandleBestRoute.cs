
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



        public static (string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)
        getOutputInfo(int nodesCount, Query query, Graph graph, List<Node> overlayNodes, List<Edge> overlayEdges)
        {
            var tempAdjacencyList = graph.AdjacencyList.ToDictionary(
                kvp => kvp.Key,
                kvp => new List<Edge>(kvp.Value) // Copy each list to avoid modifying original
            );

            foreach (var edge in overlayEdges)
            {
                if (!tempAdjacencyList.ContainsKey(edge.From))
                    tempAdjacencyList[edge.From] = new List<Edge>();

                tempAdjacencyList[edge.From].Add(edge);
            }

            var result = OptimalAlgorithm.detectShortestPath(
                nodesCount + overlayNodes.Count, 
                Program.VIRTUAL_SOURCE_NODE_ID,
                Program.VIRTUAL_DESTINATION_NODE_ID,
                tempAdjacencyList
            );

            return (
                result.path,
                result.optimalTime,
                result.allDistance,
                result.walkingDistance,
                result.pathDistance
            );
        }

    }
}
