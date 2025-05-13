
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