using System;
using System.Diagnostics;
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
            for (int i = 0; i <=5 ; i++)
            {
                Console.Write(i + " : ");
                List<Edge> edges = graph.AdjacencyList[i];
                foreach (var edge in edges)
                    {
                        Console.Write(edge);
                        Console.Write(" ");
                    }
                Console.WriteLine();

            }

        }
    }
}