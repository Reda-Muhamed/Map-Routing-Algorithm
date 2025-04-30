using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
namespace ShortestPathFinder.MapRouting.Engine
{
    public class OptimalAlgorithm
    {
        // still looking for the best algoritm 
        public static (string path ,double optimalTime) detectShortestPath(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList)
        {
            List<int> path = new List<int>();
            (List<int> fromNodeId, List<double> times ) = optimizedDijkstraAlgorithm(n, sourceId, destinationId, adjList);
            //foreach(var parent in fromNodeId)
            //{
            //    Console.WriteLine(parent); //all is -1 :(
            //}
            //foreach(var parent in times)
            //{
            //    Console.WriteLine(parent); //all is -1 :(
            //}
            double optimalTime = times[destinationId];
            if (times[destinationId] == double.MaxValue) return ("No Path found" , 0.0);

            int from = destinationId;
            
            while (from != -1)
            {
                path.Add(from);
                from = fromNodeId[from];
            }
            path.Reverse();
            return (string.Join(" " , path) , optimalTime) ;
        }

        // n -> #No. 0f nodes
        static public (List<int> fromNodeId, List<double> dest ) optimizedDijkstraAlgorithm(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList)
        {
            List<bool> visited = Enumerable.Repeat(false, n+1).ToList();
            List<double> times = Enumerable.Repeat(double.MaxValue, n+1).ToList();
            List<int> fromNodeId = Enumerable.Repeat(-1, n+1).ToList();// to detect the path
            IndexedPriorityQueue indexedPQ = new IndexedPriorityQueue();
            times[sourceId] = 0.0;
            indexedPQ.Insert(sourceId, 0.0);
            while (!indexedPQ.IsEmpty)
            {
                (int index, double minTime) = indexedPQ.Pull();
                if (times[index] < minTime)
                    continue;
                visited[index] = true;

                foreach (Edge edge in adjList[index])//adjList[index] -> list of edges
                {
                    if (!visited[edge.To])
                    {
                        double newBetterTime = times[index] + edge.TokenTime;
                        if (newBetterTime < times[edge.To])
                        {
                            times[edge.To] = newBetterTime;
                            fromNodeId[edge.To] = index;
                            if (!indexedPQ.Contains(edge.To)) indexedPQ.Insert(edge.To, newBetterTime);
                            else indexedPQ.DecreaseKey(edge.To, newBetterTime);
                        
                        }

                    }
                }

            }
            return (fromNodeId, times);

        }
    }
}
