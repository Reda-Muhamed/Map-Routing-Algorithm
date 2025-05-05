using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MapRouting.Handler;
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;


namespace MapRouting 
{                   
    class Program
    {               /*
                     *   بسم الله الرحمن الرحيم
                     *   اللهم صل على محمد وآل محمد
                     *   اللهم علمنا ما ينفعنا وانفعنا بما علمتنا انك انت العليم الحكيم
                     *   سبحانك اللهم وبحمدك اشهد ان لا اله الا انت استغفرك و اتوب اليك                    
                     */
        static async Task Main(string[] args)
        {
            try
            {
                // timer for all program should start Now
                TimerHandler.StartTotalProgramTimer();
                string[] graphLines;
                string[] queriesLines;
                try
                {
                    graphLines =  File.ReadAllLines(@".\TestCases\Medium Cases\[2] Medium Cases\Input\OLMap.txt");
                    queriesLines =  File.ReadAllLines(@".\TestCases\Medium Cases\[2] Medium Cases\Input\OLQueries.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when reading files: {ex.Message}");
                    return;
                }
                // timer for logic code should start Now
                TimerHandler.StartLogicTimer();
                Graph graph = BuildGraph.constructGraph(graphLines);
                List<Query> queries = BuildGraph.constructQueries(queriesLines);
                int nodesCount = graph.Nodes.Count;

                // Process queries in parallel
                ConcurrentBag<(string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)> results = new ConcurrentBag<(string, double, double, double, double)>();
                Parallel.ForEach(queries, query =>
                {
                    var result = HandleBestRoute.findBestStartAndEndNode(nodesCount, query, graph);
                    results.Add(result);
                });

                TimerHandler.EndLogicTimer();
                try
                {
                    ResultWriter.WriteResultsAndTiming(
                        results,
                        TimerHandler.GetTotalLogicTimeInMilliseconds(),
                        TimerHandler.GetTotalTimeInMilliseconds(),
                        @".\myOutput\results.txt"
                    );
                    TimerHandler.EndTotalProgramTimer();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error writing output file: {ex.Message}");
                    return;
                }
               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

