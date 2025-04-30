using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;


namespace MapRoutin 
{                   
    class Program   
    {               
        static void Main(string[] args)
        {
            //neglect the timer now
            Graph graph = InputReader.MapFileReader(@".\TestCases\Sample Cases\[1] Sample Cases\Input\map1.txt");
            List<Query> queries = InputReader.QueriesFileReader(@".\TestCases\Sample Cases\[1] Sample Cases\Input\queries1.txt");
            int nodesCount = graph.Nodes.Count;





            //Console.WriteLine(" all Nodes");
            //foreach (var g in graph.Nodes)
            //{
            //    Console.WriteLine(g.Id);
            //}
            //Console.WriteLine("----------------------------");
            //foreach (var key in graph.AdjacencyList)
            //{
            //    Console.Write(key.Key + " -> ");
            //        foreach (var value in graph.AdjacencyList[key.Key])
            //    {
            //        Console.Write(value.To + "  "); 
            //    }
            //    Console.WriteLine();
                
            //}



            //================================================================================================





            //foreach (Query query in queries)
            //{
            //    List<Node> sourceNodes = HelperFunctions.GetNearbyNodes(query.SourceX, query.SourceY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to start with
            //    List<Node> destinationNodes = HelperFunctions.GetNearbyNodes(query.DestinationX, query.DestinationY, graph.Nodes, query.MaxWalkingDistance);// all nodes that able to end with
            //    double minTime = 1000000000;
            //    string outputPath = "";
            //    string path = "";
            //    // try all neighbors nodes 
            //    //foreach(Node node in destinationNodes)
            //    //    Console.WriteLine(node.Id);
            //    foreach (Node sourceNode in sourceNodes)
            //    {
            //        double walkingDistanceToSourceNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
            //        double walkingTimeToSourceNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToSourceNode);
            //        foreach (Node destinationNode in destinationNodes)
            //        {
            //            double walkingDistanceToDestinationNode = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
            //            double walkingTimeToDestinationNode = HelperFunctions.CalculateWalkingTimeInH(walkingDistanceToDestinationNode);

            //            (path,double carTime) = OptimalAlgorithm.detectShortestPath(nodesCount, sourceNode.Id, destinationNode.Id,  graph.AdjacencyList);
            //            if(carTime + walkingTimeToSourceNode + walkingTimeToDestinationNode < minTime)
            //            {
            //                minTime = carTime + walkingTimeToSourceNode + walkingTimeToDestinationNode;
            //                outputPath = path;
            //            }
            //        }   
            //    }

            //}
            (string path, double carTime) = OptimalAlgorithm.detectShortestPath(nodesCount, 0, 2, graph.AdjacencyList);
            Console.WriteLine(path);
            Console.WriteLine(carTime);
        }
    }
}