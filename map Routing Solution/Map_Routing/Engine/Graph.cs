using ShortestPathFinder.MapRouting.Models;
using System.Collections.Generic;

namespace ShortestPathFinder.MapRouting.Engine
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        public Dictionary<int, List<Edge>> AdjacencyList { get; set; }

        public Graph()
        {
            Nodes = new List<Node>();
            AdjacencyList = new Dictionary<int, List<Edge>>();
        }

        public Graph(Graph other)
        {
            Nodes = new List<Node>(other.Nodes.Count);
            foreach (var node in other.Nodes)
            {
                Nodes.Add(new Node(node.Id, node.XPoint, node.YPoint));
            }

            AdjacencyList = new Dictionary<int, List<Edge>>();
            foreach (var kvp in other.AdjacencyList)
            {
                var edges = new List<Edge>();
                foreach (var edge in kvp.Value)
                {
                    edges.Add(new Edge(edge.From, edge.To, edge.Length, edge.TokenTime * edge.Length, edge.IsWalking));
                }
                AdjacencyList[kvp.Key] = edges;
            }
        }

        public void EmplaceNode(Node node)
        {
            if (node == null) return;
            Nodes.Add(node);
            if (!AdjacencyList.ContainsKey(node.Id))
            {
                AdjacencyList[node.Id] = new List<Edge>();
            }
        }

        public void EmplaceEdge(Edge edge)
        {
            if (edge == null) return;
            if (!AdjacencyList.ContainsKey(edge.From))
            {
                AdjacencyList[edge.From] = new List<Edge>();
            }
            AdjacencyList[edge.From].Add(edge);
        }

        public void deleteNode(Node node)
        {
            if (node == null) return;
            Nodes.RemoveAll(n => n.Id == node.Id);
            AdjacencyList.Remove(node.Id);
            foreach (var edges in AdjacencyList.Values)
            {
                edges.RemoveAll(e => e.To == node.Id);
            }
        }

        public void RemoveEdge(int from, int to, bool isWalking)
        {
            if (AdjacencyList.ContainsKey(from))
            {
                AdjacencyList[from].RemoveAll(e => e.To == to && e.IsWalking == isWalking);
            }
        }
    }
}