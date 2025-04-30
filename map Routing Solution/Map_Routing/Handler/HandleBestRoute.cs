using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
using System;

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
        public static (string path , double shortestTime,double pathLength , double walkingDistance, double roadsLength) findBestStartAndEndNode(int nodesCount , Query query,Graph graph)
        {
            double minTime ;
            string outputPath ;
            double distance ;
            double walkingDistanceToDestinationNode;
            double walkingTimeToDestinationNode;
            double finalWalkingDistance=0.0 ;
            double walkingDistanceToSourceNode ;
            double walkingTimeToSourceNode ;

           
                List<Node> sourceNodes = HelperFunctions.GetNearbyNodes(query.SourceX, query.SourceY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to start with
                List<Node> destinationNodes = HelperFunctions.GetNearbyNodes(query.DestinationX, query.DestinationY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to end with
                minTime = double.MaxValue;
                outputPath = "";
                distance = 0.0;
                // try all neighbors nodes 
                //foreach(Node node in destinationNodes)
                //    Console.WriteLine(node.Id);
                foreach (Node sourceNode in sourceNodes)
                {
                    walkingDistanceToSourceNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
                    walkingTimeToSourceNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToSourceNode);
                    foreach (Node destinationNode in destinationNodes)
                    {
                         walkingDistanceToDestinationNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
                         walkingTimeToDestinationNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToDestinationNode);

                        (string path, double carTime, double allDistance) = OptimalAlgorithm.detectShortestPath(nodesCount, sourceNode.Id, destinationNode.Id, graph.AdjacencyList);
                        if (carTime + walkingTimeToSourceNode + walkingTimeToDestinationNode < minTime)
                        {
                            minTime = (carTime + walkingTimeToSourceNode + walkingTimeToDestinationNode);
                            outputPath = path;
                            distance = allDistance + walkingDistanceToSourceNode+walkingDistanceToDestinationNode;
                            finalWalkingDistance = walkingDistanceToSourceNode + walkingDistanceToDestinationNode;

                        }
                    }
                }

                //Console.WriteLine("The path nodes : " + outputPath);
                //Console.WriteLine("Shortest time = " + minTime*60 + " mins");
                //Console.WriteLine("Path length = " + distance + " km");
                //Console.WriteLine("Total walking distance = " + (finalWalkingDistance) + " km");
                //Console.WriteLine("Total roads length = " + (distance - (finalWalkingDistance)) + " km");


            return (outputPath, minTime * 60.00, distance, finalWalkingDistance, (distance - finalWalkingDistance));
        }

    }
}
