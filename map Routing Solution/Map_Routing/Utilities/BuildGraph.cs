
using ShortestPathFinder.MapRouting.Engine;
using ShortestPathFinder.MapRouting.Models;
namespace ShortestPathFinder.MapRouting.Utilities
{
    public static class BuildGraph
    {  
        
        
       
        public static Graph constructGraph(string []lines)
        {
            
            int nodeCount = int.Parse(lines[0]);
            Graph graph = new Graph();

            for (int i = 1; i <= nodeCount; i++)
            {
                var allParts = lines[i].Split(' ');
                int id = int.Parse(allParts[0]);
                double x = double.Parse(allParts[1]);
                double y = double.Parse(allParts[2]);
                graph.EmplaceNode(new Node(id, x, y));
            }

            int begining0fEdgeIndex = nodeCount + 1;
            int countEdge = int.Parse(lines[begining0fEdgeIndex]);
            int linesLength = lines.Length;
            for (int i = begining0fEdgeIndex + 1; i < linesLength; i++)
            {
                var allParts = lines[i].Split(' ');
                int from = int.Parse(allParts[0]);
                int to = int.Parse(allParts[1]);
                double distance = double.Parse(allParts[2]); // KM
                double speed = double.Parse(allParts[3]); // KM/H
                graph.EmplaceEdge(new Edge(from, to, distance, speed , false));
                graph.EmplaceEdge(new Edge(to, from, distance, speed, false));
                
            }

            return graph;
        }



       
        public static List<Query> constructQueries(string []lines)
        {
            const double convertionToKm = 1000.0; // 1 km = 1000 m
            int queryCount = int.Parse(lines[0]);

            List<Query> queries = new List<Query>();

            for (int i = 1; i <= queryCount; i++)
            {
                var allParts = lines[i].Split(' ');
                double sourceX = double.Parse(allParts[0]);
                double sourceY = double.Parse(allParts[1]);
                double destinationX = double.Parse(allParts[2]);
                double destinationY = double.Parse(allParts[3]);
                double maxWalkingDistance = double.Parse(allParts[4]) / convertionToKm; ///Meters->KM !!!!

                queries.Add(new Query(sourceX, sourceY, destinationX, destinationY, maxWalkingDistance));
            }

            return queries;
        }

    }
}
