
using MapRouting.Handler;
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace MapRouting
{
    public class Program
    {
        public static double WALKING_SPEED = 5.0; // KM/H
        public static int VIRTUAL_SOURCE_NODE_ID = -2; // used two virtual nodes to represent the source and destination to avoid multiple sources to destinations 😒
        public static int VIRTUAL_DESTINATION_NODE_ID = -3;

        static void Main(string[] args)
        {
            try
            {
                TimerHandler.StartTotalProgramTimer();

                string[] graphLines;
                string[] queriesLines;

                try
                {
                    //graphLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLMap.txt");
                    //queriesLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLQueries.txt");
                    graphLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFMap.txt");
                    queriesLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFQueries.txt");



                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when reading files: {ex.Message}");
                    return;
                }

                TimerHandler.StartLogicTimer();

                Graph graph = BuildGraph.constructGraph(graphLines); // shared graph
                List<Query> queries = BuildGraph.constructQueries(queriesLines);

                var results = new ConcurrentBag<(int index, string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)>();

                Parallel.ForEach(queries.Select((query, index) => new { query, index }), item =>
                {
                    try
                    {
                        var query = item.query;
                        int index = item.index;

                        // Get nearby nodes
                        (List<Node> sourceNodes, List<Node> destinationNodes) = HelperFunctions.GetNearbyNodes(query, graph.Nodes, query.MaxWalkingDistance);
                        if (sourceNodes.Count == 0 || destinationNodes.Count == 0)
                        {
                            results.Add((index, "No Path found", 0, 0, 0, 0));
                            return;
                        }

                        Node virtualSourceNode = new Node(VIRTUAL_SOURCE_NODE_ID, query.SourceX, query.SourceY);
                        Node virtualDestinationNode = new Node(VIRTUAL_DESTINATION_NODE_ID, query.DestinationX, query.DestinationY);

                        var overlayNodes = new List<Node> { virtualSourceNode, virtualDestinationNode };
                        var overlayEdges = new List<Edge>();

                        double walkingDistance;

                        foreach (Node sourceNode in sourceNodes)
                        {
                            walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
                            overlayEdges.Add(new Edge(VIRTUAL_SOURCE_NODE_ID, sourceNode.Id, walkingDistance, WALKING_SPEED, true));
                        }

                        foreach (Node destinationNode in destinationNodes)
                        {
                            walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
                            overlayEdges.Add(new Edge(destinationNode.Id, VIRTUAL_DESTINATION_NODE_ID, walkingDistance, WALKING_SPEED, true));
                        }

                        // Call routing logic (overlay-aware version)
                        var result = HandleBestRoute.getOutputInfo(graph.Nodes.Count, query, graph, overlayNodes, overlayEdges);

                        results.Add((index, result.path, result.shortestTime, result.pathLength, result.walkingDistance, result.roadsLength));
                    }
                    catch (Exception ex)
                    {
                        results.Add((item.index, $"Error processing query: {ex.Message}", 0, 0, 0, 0));
                    }
                });

                TimerHandler.EndLogicTimer();
                // order the results by index cuz it may be unordered due to parallel processing
                var orderedResults = results.OrderBy(r => r.index)
                                            .Select(r => (r.path, r.shortestTime, r.pathLength, r.walkingDistance, r.roadsLength))
                                            .ToList();

                ResultWriter.WriteResultsAndTiming(orderedResults, TimerHandler.GetTotalLogicTimeInMilliseconds(),
                    @"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\myOutput\results.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            //var myOut  = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\myOutput\results.txt");
            //var hisOut = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Output\SFOutput.txt");
            //bool areEqual = myOut.SequenceEqual(hisOut);

            //if (areEqual)
            //{
            //    Console.WriteLine("The files are equal.");
            //}
            //else
            //{
            //    Console.WriteLine("The files are NOT equal.");
            //}





        }
    }
}
