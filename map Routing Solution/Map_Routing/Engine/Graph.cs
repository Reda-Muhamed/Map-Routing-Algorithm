using ShortestPathFinder.MapRouting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Engine
{
    public class Graph
    {
        
        public List<Node> Nodes { get; set; }
        public Dictionary<int, List<Edge>> AdjacencyList { get; set; } // node[Id] => { edge1 : { from , to, length, speed }, ..... }
        public Graph()
        {
            Nodes = new List<Node>();
            AdjacencyList = new Dictionary<int, List<Edge>>();
        }
        public void EmplaceNode(Node node)
        {
            if (node==null) return;
            Nodes.Add(node);
            if (!AdjacencyList.ContainsKey(node.Id))
            {
                AdjacencyList[node.Id] = new List<Edge>();
            }
        }
        public void EmplaceEdge(Edge edge)  // { from , to, length, speed }
        { 
            if (edge == null) return;
            if (!AdjacencyList.ContainsKey(edge.From))
            {
                AdjacencyList[edge.From] = new List<Edge>();
            }
            AdjacencyList[edge.From].Add(edge);
        }

    }
}
