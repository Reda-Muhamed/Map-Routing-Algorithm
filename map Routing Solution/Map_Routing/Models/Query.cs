using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Models
{
    public class Query
    {
        public double SourceX { get; set; }          
        public double SourceY { get; set; }          
        public double DestinationX { get; set; }    
        public double DestinationY { get; set; }    
        public double MaxWalkingDistance { get; set; }
        public Query() { }
        public Query(double sourceX, double sourceY, double destinationX, double destinationY, double maxWalkingDistance)
        {
            SourceX = sourceX;
            SourceY = sourceY;
            DestinationX = destinationX;
            DestinationY = destinationY;
            MaxWalkingDistance = maxWalkingDistance;
        }

        
    }
}
