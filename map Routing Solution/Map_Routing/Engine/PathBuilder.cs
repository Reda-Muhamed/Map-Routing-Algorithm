using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Engine
{
public static class PathBuilder
{
    public static void appendQueryresultInTheFile(List<int> pathNodes,double shortestTime, double totalDistance,
                                          double walkingDistance, double vehicleDistance)
    {
        string result = $"{string.Join(", ", pathNodes)}\n" +
                        $"{Math.Round(shortestTime ,2)} mins\n" +
                        $"{Math.Round(totalDistance,2)} km\n" +
                        $"{Math.Round(walkingDistance,2)} km\n" +
                        $"{Math.Round(vehicleDistance)} km\n";

        File.AppendAllText("output/results.txt", result + "\n");
        return;
    }

    private static double CalculateVehicleDistance(List<int> pathNodes, Graph graph)
    {
        double total = 0;
        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            var from = pathNodes[i];
            var to = pathNodes[i + 1];
            var edge = graph.AdjacencyList[from].FirstOrDefault(e => e.To == to || e.From == to);
            if (edge != null)
                total += edge.LengthInKm;
        }
        return total;
    }
}

}
