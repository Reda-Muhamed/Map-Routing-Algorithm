using MapRouting;
using ShortestPathFinder.MapRouting.Models;
using ShortestPathFinder.MapRouting.Utilities;
namespace ShortestPathFinder.MapRouting.Engine
{
    public static class OptimalAlgorithm
    {
        static public (Dictionary<int, int> fromNodeId, Dictionary<int, double> times)
            optimizedDijkstraAlgorithm(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList) // [node Id]->{edge1, edge2 , edge3 }
        {
            var visited = new HashSet<int>();
            var times = new Dictionary<int, double>();
            var fromNodeId = new Dictionary<int, int>(); // to detect path
            var indexedPQ = new IndexedPriorityQueue();

            var keys = adjList.Keys.ToList();
            foreach (var nodeId in keys)
            {
                times[nodeId] = double.MaxValue;
                fromNodeId[nodeId] = -1;
            }
            if (!times.ContainsKey(sourceId))
            {
                times[sourceId] = double.MaxValue;
                fromNodeId[sourceId] = -1;
            }
            if (!times.ContainsKey(destinationId))
            {
                times[destinationId] = double.MaxValue;
                fromNodeId[destinationId] = -1;
            }

            times[sourceId] = 0.0;//cost to my self is 0
            indexedPQ.Insert(sourceId, 0.0);

            while (!indexedPQ.IsEmpty)
            {
                var (index, minTime) = indexedPQ.Pull();
                if (times[index] < minTime || visited.Contains(index))
                    continue;
                if (index == destinationId)
                    break;

                visited.Add(index);

                if (!adjList.ContainsKey(index))
                    continue;

                foreach (Edge edge in adjList[index])
                {
                    if (!visited.Contains(edge.To))
                    {
                        double newBetterTime = times[index] + edge.TokenTime;
                        if (newBetterTime < times[edge.To])
                        {
                            times[edge.To] = newBetterTime;
                            fromNodeId[edge.To] = index;
                            if (!indexedPQ.Contains(edge.To))
                                indexedPQ.Insert(edge.To, newBetterTime);
                            else
                                indexedPQ.DecreaseKey(edge.To, newBetterTime);
                        }
                    }
                }
            }

            return (fromNodeId, times);
        }

        public static (string path, double optimalTime, double allDistance, double walkingDistance, double pathDistance)
            detectShortestPath(int n, int sourceId, int destinationId, Dictionary<int, List<Edge>> adjList)
        {
            List<int> path = new List<int>();
            var (fromNodeId, times) = optimizedDijkstraAlgorithm(n, sourceId, destinationId, adjList);

            double optimalTime = times.ContainsKey(destinationId) ? times[destinationId] : double.MaxValue;
            if (optimalTime == double.MaxValue)
                return ("No Path found", 0.0, 0.0, 0.0, 0.0);

            double allDistance = 0.0;
            double walkingDistance = 0.0;
            double pathDistance = 0.0;

            int currentNodeId = destinationId;
            while (currentNodeId != sourceId && fromNodeId.ContainsKey(currentNodeId))
            {
                if (currentNodeId != Program.VIRTUAL_SOURCE_NODE_ID && currentNodeId != Program.VIRTUAL_DESTINATION_NODE_ID)
                    path.Add(currentNodeId);
                int prevNodeId = fromNodeId[currentNodeId];
                if (prevNodeId != -1 && adjList.ContainsKey(prevNodeId))
                {
                    var edge = adjList[prevNodeId].FirstOrDefault(e => e.To == currentNodeId);
                    if (edge != null)
                    {
                        allDistance += edge.Length;
                        if (edge.IsWalking)
                            walkingDistance += edge.Length;
                        else
                            pathDistance += edge.Length;
                    }
                }

                currentNodeId = prevNodeId;
            }
            path.Reverse();

            return (string.Join(" ", path), optimalTime, allDistance, walkingDistance, pathDistance);
        }


    }

}
