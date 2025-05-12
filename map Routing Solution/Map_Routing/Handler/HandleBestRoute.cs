
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
        public static (string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength) getOutputInfo(int nodesCount, Query query, Graph graph)
        {
            // find the best route between the two virtual nodes
            var result = OptimalAlgorithm.detectShortestPath(nodesCount, Program.VIRTUAL_SOURCE_NODE_ID, Program.VIRTUAL_DESTINATION_NODE_ID, graph.AdjacencyList);
            string path = result.path;
            double shortestTime = result.optimalTime;
            double pathLength = result.allDistance;
            double walkingDistance = result.walkingDistance;
            double roadsLength =result.pathDistance;
           
            return (path, shortestTime, pathLength, walkingDistance, roadsLength);




        }
    }
}
