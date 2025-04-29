
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Utilities
{
    public static class InputReader
    {  
        
        
        /// Reads the graph from a file and constructs the graph object
        public static Graph MapFileReader(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int nodeCount = int.Parse(lines[0]);
            Graph graph = new Graph();

            for (int i = 1; i <= nodeCount; i++)
            {
                var parts = lines[i].Split(' ');
                int id = int.Parse(parts[0]);
                double x = double.Parse(parts[1]);
                double y = double.Parse(parts[2]);
                graph.EmplaceNode(new Node(id, x, y));
            }

            int begining0fEdgeIndex = nodeCount + 1;
            int countEdge = int.Parse(lines[begining0fEdgeIndex]);
            int linesLength = lines.Length;
            for (int i = begining0fEdgeIndex + 1; i < linesLength; i++)
            {
                var parts = lines[i].Split(' ');
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                double distance = double.Parse(parts[2]); // KM
                double speed = double.Parse(parts[3]); // KM/H
                graph.EmplaceEdge(new Edge(from, to, distance, speed));
            }

            return graph;
        }



        /// Reads the queries from a file and constructs the list of queries
        public static List<Query> QueriesFileReader(string filePath)
        {
            const double convertionToKm = 1000.0; // 1 km = 1000 m
            var lines = File.ReadAllLines(filePath);
            int queryCount = int.Parse(lines[0]);

            List<Query> queries = new List<Query>();

            for (int i = 1; i <= queryCount; i++)
            {
                var parts = lines[i].Split(' ');
                double sourceX = double.Parse(parts[0]);
                double sourceY = double.Parse(parts[1]);
                double destinationX = double.Parse(parts[2]);
                double destinationY = double.Parse(parts[3]);
                double maxWalkingDistance = double.Parse(parts[4]) / convertionToKm; ///Meters->KM !!!!

                queries.Add(new Query(sourceX, sourceY, destinationX, destinationY, maxWalkingDistance));
            }

            return queries;
        }

    }
}
