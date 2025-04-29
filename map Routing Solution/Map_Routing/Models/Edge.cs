using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Models
{
    public class Edge
    {     
            public int From { get; set; }   
            public int To { get; set; }
            public double LengthInKm { get; set; }
            public double SpeedInKmPerH { get; set; }
            public Edge(int from, int to, double lengthKm, double speedKmH)
            {
                From = from;
                To = to;
                LengthInKm = lengthKm;
                SpeedInKmPerH = speedKmH;
            }
            //public override string ToString()
            //{
            //    return $"{To}";
            //}



    }
}
