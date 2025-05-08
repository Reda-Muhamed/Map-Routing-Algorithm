//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShortestPathFinder.MapRouting.Models
//{
//    public class Edge
//    {     
//            public int From { get; set; }   
//            public int To { get; set; }
//            public double LengthInKm { get; set; }
//            public double SpeedInKmPerH { get; set; }

//            public double TokenTime { get; set; }


//        public Edge(int from, int to, double lengthKm, double speedKmH)
//            {
//                From = from;
//                To = to;
//                LengthInKm = lengthKm;
//                SpeedInKmPerH = speedKmH;
//                TokenTime = lengthKm / speedKmH;
//            }
//            //public override string ToString()
//            //{
//            //    return $"{To}";
//            //}



//    }
//}





//=============================================================================================
//                                        Testing code                                        //
//=============================================================================================


namespace ShortestPathFinder.MapRouting.Models
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public double Length { get; set; } // Distance in km
        public double Speed { get; set; } // Speed in km/h
        public double Time => Length / Speed; // Time in hours
        public double TokenTime => Time * 60; 
        public bool IsWalking { get; set; } = false;// Assume edges indicate walking or path

        public Edge(int from, int to, double length, double speed , bool isWalking)
        {
            From = from;
            To = to;
            Length = length;
            Speed = speed;
            IsWalking = isWalking ;
        }
    }
}