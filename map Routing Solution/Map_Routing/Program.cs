using System;
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
        static void Main(string[] args)
        {
            // timer for all program should start Now
            TimerHandler.StartTotalProgramTimer();
            var graphLines = File.ReadAllLines(@".\TestCases\Medium Cases\[2] Medium Cases\Input\OLMap.txt");
            var queriesLines = File.ReadAllLines(@".\TestCases\Medium Cases\[2] Medium Cases\Input\OLQueries.txt");
            // timer for logic code should start Now
            TimerHandler.StartLogicTimer();
            Graph graph = BuildGraph.constructGraph(graphLines);
            List<Query> queries = BuildGraph.constructQueries(queriesLines);
            int nodesCount = graph.Nodes.Count;
            List<string> paths = new List<string>();
            foreach (Query query in queries)
            {
                (string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)=HandleBestRoute.findBestStartAndEndNode(nodesCount, query, graph);
                TimerHandler.EndLogicTimer();
                ResultWriter.appendQueryResultInTheFile(path, shortestTime, pathLength, walkingDistance, roadsLength);
                TimerHandler.StartLogicTimer();
            }
            TimerHandler.EndLogicTimer();
            TimerHandler.EndTotalProgramTimer();
            ResultWriter.appendTimeInTheFile(TimerHandler.GetTotalLogicTimeInMilliseconds(),TimerHandler.GetTotalTimeInMilliseconds());
        }
    }
}