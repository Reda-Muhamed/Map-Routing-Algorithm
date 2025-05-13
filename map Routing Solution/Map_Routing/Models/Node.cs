
namespace ShortestPathFinder.MapRouting.Models
{
    public class Node
    {

            public int Id { get; set; }
            public double XPoint { get; set; }
            public double YPoint { get; set; }

        public Node(int id, double x, double y)
            {
                Id = id;
                XPoint = x;
                YPoint = y;
            }

    }
}
