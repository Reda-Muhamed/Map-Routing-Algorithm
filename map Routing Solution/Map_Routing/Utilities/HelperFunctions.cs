using ShortestPathFinder.MapRouting.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Utilities
{
    public class HelperFunctions
    {
        private const double WalkingSpeedKmH = 5.0;


        // calculate the distance between two points in km
        public static double calculateDistanceBetween2PointsInKm(double x1, double y1, double x2, double y2)
        {
            double dx = x1 - x2;
            double dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static double calculateDistanceBetween2PointsInKm(double x1, double y1, Node node)
        {
            double dx = x1 - node.XPoint;
            double dy = y1 - node.YPoint;
            return Math.Sqrt(dx * dx + dy * dy);
        }



        // Get all nodes in range of the given point
        public static List<Node> GetNearbyNodes(double x, double y, List<Node> allNodes, double maxWalkingDistance)//maxWalkingDistance : should be in KM
        {
            return allNodes.Where(n => calculateDistanceBetween2PointsInKm(n.XPoint, n.YPoint, x, y) <= maxWalkingDistance).ToList();
        }



        // Calculate the walking time in minute
        public static double CalculateWalkingTimeInH(double distanceInKM)
        {
            return distanceInKM / WalkingSpeedKmH;
        }
    }
}
