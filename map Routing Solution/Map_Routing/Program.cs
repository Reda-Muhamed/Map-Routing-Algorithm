using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MapRouting.Handler;
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
using System.Collections;

namespace MapRouting
{
    public class Program
    {
        public static double WALKING_SPEED = 5.0; // KM/H
        public static int VIRTUAL_SOURCE_NODE_ID = -2;
        public static int VIRTUAL_DESTINATION_NODE_ID = -3;

        static async Task Main(string[] args)
        {
            try
            {
                TimerHandler.StartTotalProgramTimer();
                string[] graphLines;
                string[] queriesLines;
                try
                {
                    graphLines = await File.ReadAllLinesAsync(@"C:\Users\L E N O V O\Desktop\algo\Map-Routing-Algorithm-master\Map-Routing-Algorithm-master\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFMap.txt");
                    queriesLines = await File.ReadAllLinesAsync(@"C:\Users\L E N O V O\Desktop\algo\Map-Routing-Algorithm-master\Map-Routing-Algorithm-master\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFQueries.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when reading files: {ex.Message}");
                    return;
                }

                TimerHandler.StartLogicTimer();
                Graph graph = BuildGraph.constructGraph(graphLines);// original version of graph

                List<Query> queries = BuildGraph.constructQueries(queriesLines);
                int nodesCount = graph.Nodes.Count;

                List<(int index, string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)> results = new List<(int, string, double, double, double, double)>();
                int queryIndex = 0;

                foreach (var query in queries)
                {



                    List<Node> sourceNodes = HelperFunctions.GetNearbyNodes(query.SourceX, query.SourceY, graph.Nodes, query.MaxWalkingDistance);
                    List<Node> destinationNodes = HelperFunctions.GetNearbyNodes(query.DestinationX, query.DestinationY, graph.Nodes, query.MaxWalkingDistance);


                    if (sourceNodes.Count == 0 || destinationNodes.Count == 0)
                    {
                        results.Add((queryIndex, "No Path found", 0.0, 0.0, 0.0, 0.0));
                        //(item.index, "No Path found", 0.0, 0.0, 0.0, 0.0));
                        Console.WriteLine("No Path found ");
                        queryIndex++;
                        continue;
                    }


                    Node virtualSourceNode = new Node(VIRTUAL_SOURCE_NODE_ID, query.SourceX, query.SourceY);
                    Node virtualDestinationNode = new Node(VIRTUAL_DESTINATION_NODE_ID, query.DestinationX, query.DestinationY);

                    List<Edge> addedEdges = new List<Edge>();
                    graph.EmplaceNode(virtualSourceNode);
                    graph.AdjacencyList[VIRTUAL_SOURCE_NODE_ID] = new List<Edge>();
                    foreach (Node sourceNode in sourceNodes)
                    {
                        double walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
                        var edge = new Edge(VIRTUAL_SOURCE_NODE_ID, sourceNode.Id, walkingDistance, WALKING_SPEED, true);
                        graph.EmplaceEdge(edge);
                        addedEdges.Add(edge);
                    }

                    graph.EmplaceNode(virtualDestinationNode);
                    graph.AdjacencyList[VIRTUAL_DESTINATION_NODE_ID] = new List<Edge>();
                    foreach (Node destinationNode in destinationNodes)
                    {
                        double walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
                        var edge = new Edge(destinationNode.Id, VIRTUAL_DESTINATION_NODE_ID, walkingDistance, WALKING_SPEED, true);
                        graph.EmplaceEdge(edge);
                        addedEdges.Add(edge);
                    }




                    // Find the best route
                    var result = HandleBestRoute.getOutputInfo(graph.Nodes.Count, query, graph);

                    results.Add((queryIndex, result.path, result.shortestTime, result.pathLength, result.walkingDistance, result.roadsLength));
                    ResultWriter.WriteResultsAndTimingTesting(result.path, result.shortestTime, result.pathLength, result.walkingDistance, result.roadsLength);


                    // Cleanup: Remove virtual nodes and edges
                    graph.deleteNode(virtualSourceNode);
                    graph.deleteNode(virtualDestinationNode);
                    foreach (var edge in addedEdges)
                    {
                        graph.RemoveEdge(edge.From, edge.To, edge.IsWalking);
                    }

                    queryIndex++;
                }



                TimerHandler.EndLogicTimer();

                TimerHandler.EndTotalProgramTimer();

               

                Console.WriteLine(TimerHandler.GetTotalLogicTimeInMilliseconds());
                Console.WriteLine(TimerHandler.GetTotalTimeInMilliseconds());

                ResultWriter.WriteResultsAndTiming(results, TimerHandler.GetTotalLogicTimeInMilliseconds(), TimerHandler.GetTotalTimeInMilliseconds(), @"C:\\Users\\L E N O V O\\Desktop\\algo\\Map-Routing-Algorithm-master\\Map-Routing-Algorithm-master\\map Routing Solution\\Map_Routing\\myOutput\\results.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }


        }
    }
}

