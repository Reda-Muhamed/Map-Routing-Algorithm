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
        public static (List<Node> nearBySourceNodes, List<Node>nearByDestinationNodes) GetNearbyNodes(Query query , List<Node> allNodes, double maxWalkingDistance)//maxWalkingDistance : should be in KM
        {
            List<Node> nearBySourceNodes = new List<Node>();
            List<Node> nearByDestinationNodes = new List<Node>();
            allNodes.Select(n => n).ToList();
            foreach (var node in allNodes)
            {
                double distanceToSource = calculateDistanceBetween2PointsInKm(query.SourceX, query.SourceY, node);
                double distanceToDestination = calculateDistanceBetween2PointsInKm(query.DestinationX, query.DestinationY, node);
                if (distanceToSource <= maxWalkingDistance)
                {
                    nearBySourceNodes.Add(node);
                }
                if (distanceToDestination <= maxWalkingDistance)
                {
                    nearByDestinationNodes.Add(node);
                }
            }
            return (nearBySourceNodes, nearByDestinationNodes);
        }



        // Calculate the walking time in minute
        public static double CalculateWalkingTimeInH(double distanceInKM)
        {
            return distanceInKM / WalkingSpeedKmH;
        }
    }
}
