
using MapRouting.Handler;
using ShortestPathFinder.MapRouting;
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MapRouting
{
    public class Program
    {
        public static double WALKING_SPEED = 5.0; 
        public static int VIRTUAL_SOURCE_NODE_ID = -2; 
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
                    graphLines = File.ReadAllLines(@"C:\Users\L E N O V O\Downloads\Map-Routing-Algorithm(2)\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLMap.txt");
                    queriesLines = File.ReadAllLines(@"C:\Users\L E N O V O\Downloads\Map-Routing-Algorithm(2)\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLQueries.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when reading files: {ex.Message}");
                    return;
                }

                TimerHandler.StartLogicTimer();

                Graph graph = BuildGraph.constructGraph(graphLines);
                List<Query> queries = BuildGraph.constructQueries(queriesLines);

                var results = new ConcurrentBag<(int index, string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)>();
                var allPaths = new ConcurrentBag<(int index, List<int> pathNodes)>();
                var allEdges = graph.AdjacencyList.Values.SelectMany(list => list).ToList();

                string outputDir = @"C:\Users\L E N O V O\Downloads\Map-Routing-Algorithm(2)\Map-Routing-Algorithm\map Routing Solution\Map_Routing\myOutput";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                    Console.WriteLine($"Created output directory: {outputDir}");
                }

                Parallel.ForEach(queries.Select((query, index) => new { query, index }), item =>
                {
                    try
                    {
                        var query = item.query;
                        int index = item.index;

                        (List<Node> sourceNodes, List<Node> destinationNodes) = HelperFunctions.GetNearbyNodes(query, graph.Nodes, query.MaxWalkingDistance);
                        if (sourceNodes.Count == 0 || destinationNodes.Count == 0)
                        {
                            results.Add((index, "No Path found", 0, 0, 0, 0));
                            allPaths.Add((index, new List<int>()));
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

                        var result = HandleBestRoute.getOutputInfo(graph.Nodes.Count, query, graph, overlayNodes, overlayEdges);

                        List<int> pathNodes = result.path == "No Path found"
                            ? new List<int>()
                            : result.path.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(int.Parse)
                                     .ToList();

                        results.Add((index, result.path, result.shortestTime, result.pathLength, result.walkingDistance, result.roadsLength));
                        allPaths.Add((index, pathNodes));
                    }
                    catch (Exception ex)
                    {
                        results.Add((item.index, $"Error processing query: {ex.Message}", 0, 0, 0, 0));
                        allPaths.Add((item.index, new List<int>()));
                    }
                });

                TimerHandler.EndLogicTimer();

                var orderedResults = results.OrderBy(r => r.index)
                                            .Select(r => (r.path, r.shortestTime, r.pathLength, r.walkingDistance, r.roadsLength))
                                            .ToList();

                ResultWriter.WriteResultsAndTiming(orderedResults, TimerHandler.GetTotalLogicTimeInMilliseconds(),
                    $@"{outputDir}\results.txt");

                var orderedPaths = allPaths.OrderBy(p => p.index)
                                           .Select(p => p.pathNodes)
                                           .ToList();

                for (int i = 0; i < queries.Count; i++)
                {
                    var query = queries[i];
                    var pathNodes = orderedPaths[i];
                    var queryPoint = (query.SourceX, query.SourceY, query.DestinationX, query.DestinationY);
                  

                    (List<Node> sourceNodes, List<Node> destinationNodes) = HelperFunctions.GetNearbyNodes(query, graph.Nodes, query.MaxWalkingDistance);
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

                    var edgesForQuery = allEdges.Concat(overlayEdges).ToList();
                    string outputImagePath = $@"{outputDir}\query_{i + 1}_map.png";
                    MapVisualizer.VisualizeMap(graph.Nodes, edgesForQuery, pathNodes, queryPoint, outputImagePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Uncomment to compare output files if needed
            /*
            var myOut = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\myOutput\results.txt");
            var hisOut = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Output\SFOutput.txt");
            bool areEqual = myOut.SequenceEqual(hisOut);

            if (areEqual)
            {
                Console.WriteLine("The files are equal.");
            }
            else
            {
                Console.WriteLine("The files are NOT equal.");
            }
            */
        }
    }
}






//==============================================================================================================
//                                           TEST
//==============================================================================================================





//using MapRouting.Handler;
//using ShortestPathFinder.MapRouting.Engine;
//using ShortestPathFinder.MapRouting.Models;
//using ShortestPathFinder.MapRouting.Utilities;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MapRouting
//{
//    public class Program
//    {
//        public static double WALKING_SPEED = 5.0; // KM/H
//        public static int VIRTUAL_SOURCE_NODE_ID = -2; // used two virtual nodes to represent the source and destination to avoid multiple sources to destinations 😒
//        public static int VIRTUAL_DESTINATION_NODE_ID = -3;

//        static void Main(string[] args)
//        {
//            try
//            {
//                TimerHandler.StartTotalProgramTimer();

//                string[] graphLines;
//                string[] queriesLines;

//                try
//                {
//                    //graphLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLMap.txt");
//                    //queriesLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Medium Cases\[2] Medium Cases\Input\OLQueries.txt");
//                    graphLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFMap.txt");
//                    queriesLines = File.ReadAllLines(@"E:\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Input\SFQueries.txt");



//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error when reading files: {ex.Message}");
//                    return;
//                }

//                TimerHandler.StartLogicTimer();

//                Graph graph = BuildGraph.constructGraph(graphLines); // shared graph
//                List<Query> queries = BuildGraph.constructQueries(queriesLines);
//                int nodesCount = graph.Nodes.Count;
//                var results = new ConcurrentBag<(int index, string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)>();
//                foreach(Query query in queries)
//                {
//                    (List<Node> sourceNodes, List<Node> destinationNodes) = HelperFunctions.GetNearbyNodes(query, graph.Nodes, query.MaxWalkingDistance);
//                    if (sourceNodes.Count == 0 || destinationNodes.Count == 0)
//                    {
//                        return;
//                    }
//                    double walkingDistance;
//                    IndexedPriorityQueue forwardPQ = new IndexedPriorityQueue();
//                     Dictionary<int, double> walkingDistanceForSources = new Dictionary<int, double>();
//                     Dictionary<int, double> forwardTime = new Dictionary<int, double>();


//                    foreach (Node sourceNode in sourceNodes)
//                    {
//                        walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, sourceNode);
//                        forwardPQ.Insert(sourceNode.Id,walkingDistance  / WALKING_SPEED);
//                        walkingDistanceForSources.Add(sourceNode.Id, walkingDistance);
//                        forwardTime.Add(sourceNode.Id, walkingDistance / WALKING_SPEED);
//                    }
//                    IndexedPriorityQueue backwardPQ = new IndexedPriorityQueue();
//                    Dictionary<int, double> walkingDistanceForDestinations = new Dictionary<int, double>();
//                    Dictionary<int, double> backwardTime = new Dictionary<int, double>();
//                    foreach (Node destinationNode in destinationNodes)
//                    {
//                        walkingDistance = HelperFunctions.calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, destinationNode);
//                        backwardPQ.Insert(destinationNode.Id, walkingDistance / WALKING_SPEED);
//                        walkingDistanceForDestinations.Add(destinationNode.Id, walkingDistance);
//                        backwardTime.Add(destinationNode.Id, walkingDistance / WALKING_SPEED);
//                    }

//                    Console.WriteLine(OptimalAlgorithm.multiSourceBidirectionalDijkstra(nodesCount, forwardPQ, walkingDistanceForSources, backwardPQ, walkingDistanceForDestinations, forwardTime, backwardTime, graph.AdjacencyList).minTime);


//                }


//                TimerHandler.EndLogicTimer();


//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Unexpected error: {ex.Message}");
//            }

//            //var myOut  = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\myOutput\results.txt");
//            //var hisOut = File.ReadAllLines(@"E:\COLLEGE MATERIAL\Algo\Algo Project\Map-Routing-Algorithm\map Routing Solution\Map_Routing\TestCases\Large Cases\[3] Large Cases\Output\SFOutput.txt");
//            //bool areEqual = myOut.SequenceEqual(hisOut);

//            //if (areEqual)
//            //{
//            //    Console.WriteLine("The files are equal.");
//            //}
//            //else
//            //{
//            //    Console.WriteLine("The files are NOT equal.");
//            //}

//        }
//    }
//}
