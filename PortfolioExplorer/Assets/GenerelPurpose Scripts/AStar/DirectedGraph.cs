using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Graphs
{
    public class DirectedGraph<NodeData, EdgeData>
    {
        public List<DirectedGraphNode<NodeData>> nodes = new List<DirectedGraphNode<NodeData>>();
        public Dictionary<DirectedGraphNode<NodeData>, Dictionary<DirectedGraphNode<NodeData>, DirectedGraphEdge<EdgeData>>> edges = new Dictionary<DirectedGraphNode<NodeData>, Dictionary<DirectedGraphNode<NodeData>, DirectedGraphEdge<EdgeData>>>();

        public DirectedGraphNode<NodeData> GetNode(NodeData data)
        {
            var nodeRes = nodes.Where(x => x.data.Equals(data));

            if (nodeRes.Count() == 0)
                return null;

            return nodeRes.First();
        }

        public DirectedGraphEdge<EdgeData> GetEdge(DirectedGraphNode<NodeData> from, DirectedGraphNode<NodeData> to)
        {
            if (edges.ContainsKey(from) && edges[from].ContainsKey(to))
            {
                return edges[from][to];
            }

            throw new ArgumentException("Edge does not exist");
        }
        
        public Dictionary<DirectedGraphNode<NodeData>, DirectedGraphEdge<EdgeData>> GetEdges(DirectedGraphNode<NodeData> from)
        {
            if (edges.ContainsKey(from))
            {
                return edges[from];
            }

            return null;
        }

        public void AddNode(DirectedGraphNode<NodeData> n)
        {
            if (!nodes.Contains(n))
            {
                nodes.Add(n);
            }
        }

        public void AddEdge(DirectedGraphNode<NodeData> from, DirectedGraphNode<NodeData> to, DirectedGraphEdge<EdgeData> data)
        {
            if (!nodes.Contains(from))
            {
                throw new ArgumentException("Node \"from\" does not exist in the graph.");
            }

            if (!nodes.Contains(to))
            {
                throw new ArgumentException("Node \"to\" does not exist in the graph.");
            }

            if (!edges.ContainsKey(from))
            {
                edges.Add(from, new Dictionary<DirectedGraphNode<NodeData>, DirectedGraphEdge<EdgeData>>());
            }

            if (!edges[from].ContainsKey(to))
            {
                edges[from].Add(to, data);
            }
        }

        public override string ToString()
        {
            string s = "Printing nodes:\n";

            foreach (DirectedGraphNode<NodeData> n in nodes)
            {
                s += n.ToString() + "\n";
            }

            s += "\n--------------------------------------------\nPrinting edges:\n";
            foreach (DirectedGraphNode<NodeData> from in edges.Keys)
            {
                s += "from " + from.ToString() + ":\n";

                foreach (DirectedGraphNode<NodeData> to in edges[from].Keys)
                {
                    s += "to: key=" + to.ToString() + ", value=" + edges[from][to].ToString() + "\n";
                }

                s += "\n";
            }

            return s;
        }
    }

    public interface DirectedGraphNode<NodeData>
    {
        NodeData data { get; set; }
    }

    public interface DirectedGraphEdge<EdgeData>
    {
        EdgeData data { get; set; }
    }
}
