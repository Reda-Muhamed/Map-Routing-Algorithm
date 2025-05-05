using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
namespace ShortestPathFinder.MapRouting.Engine
{
    public class OptimalAlgorithm
    {
        // still looking for the best algoritm 
        public static (string path ,double optimalTime,double allDistance) detectShortestPath(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList)
        {
            List<int> path = new List<int>();
            (int[] fromNodeId, double[] times) = optimizedDijkstraAlgorithm(n, sourceId, destinationId, adjList);
            //foreach(var parent in fromNodeId)
            //{
            //    Console.WriteLine(parent); //all is -1 :(
            //}
            //foreach(var parent in times)
            //{
            //    Console.WriteLine(parent); //all is maxVal :(
            //}
            double optimalTime = times[destinationId];
            if (times[destinationId] == double.MaxValue) return ("No Path found" , 0.0,0.0);

            int currentNodeId = destinationId;
            double allDistance = 0.0;
            while (currentNodeId != -1)
            {
                path.Add(currentNodeId);
                int prevNodeId = fromNodeId[currentNodeId];
                if (prevNodeId != -1)
                {
                    // Get edge length from prevNodeId -> currentNodeId
                    var edge = adjList[prevNodeId].FirstOrDefault(e => e.To == currentNodeId);
                    if (edge != null)
                    {
                        allDistance += edge.LengthInKm;
                    }
                }
                currentNodeId = fromNodeId[currentNodeId];

            }         
            path.Reverse();
            return (string.Join(" " , path) , optimalTime , allDistance) ;
        }

        // n -> #No. 0f nodes
        static public (int[] fromNodeId, double[] times) optimizedDijkstraAlgorithm(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList)
        {
            bool[] visited = new bool[n + 1];
            double[] times = new double[n + 1];
            int[] fromNodeId = new int[n + 1];
            IndexedPriorityQueue indexedPQ = new IndexedPriorityQueue();
            for (int i = 0; i <= n; i++)
            {
                visited[i] = false;
                times[i] = double.MaxValue;
                fromNodeId[i] = -1;
            }
            times[sourceId] = 0.0;// to myself cost 0 
            indexedPQ.Insert(sourceId, 0.0);
            while (!indexedPQ.IsEmpty)
            {
                (int index, double minTime) = indexedPQ.Pull();
                if (times[index] < minTime || visited[index])
                    continue;
                if (index == destinationId) break;

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
