using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Heap;
using dsHeap = Heap.Heap;

namespace Pathfinding
{
    public class AStar<NodeData, EData>
    {
        public static Node[] FindPath(Node start, Node goal, Graphs.DirectedGraph<NodeData, EData> graph, Func<Node, Node, double> heuristic)
        {
            List<Node> visited = new List<Node>();
            dsHeap frontier = new dsHeap(new List<HeapNode>(), dsHeap.HeapProperty.MinHeap);

            start.IntermediateCost = 0;
            start.EstimatedCost = 0;
            start.Parent = null;
            frontier.HeapInsert(start);

            while (frontier.Count > 0)
            {
                Node current = (Node)frontier.HeapExtractRoot();

                Console.WriteLine(current);

                if (current == goal)
                {
                    return ReconstructPath(current);
                }

                visited.Add(current);
                foreach (var edgeTo in graph.GetEdges(current))
                {
                    Node to = (Node)edgeTo.Key;

                    if (visited.Contains(to))
                    {
                        continue;
                    }

                    double tentativeScore = current.IntermediateCost + ((EdgeData)edgeTo.Value).cost;
                    if (tentativeScore < to.IntermediateCost)
                    {
                        to.IntermediateCost = tentativeScore;
                        to.EstimatedCost = tentativeScore + heuristic(to, goal);
                        to.Parent = current;
                    }

                    if (!frontier.Contains(to))
                    {
                        frontier.HeapInsert(to);
                    }

                }
            }

            return null;
        }

        private static Node[] ReconstructPath(Node end)
        {
            Stack<Node> path = new Stack<Node>();

            Node current = end;
            while (current.Parent != null)
            {
                path.Push(current);
                current = current.Parent;
            }

            return path.ToArray();
        }

        public class Node : HeapNode, Graphs.DirectedGraphNode<NodeData>
        {
            public Node Parent;
            public double IntermediateCost = double.MaxValue;
            public double EstimatedCost = double.MaxValue;
            public NodeData data { get; set; }

            public double key
            {
                get
                {
                    return EstimatedCost;
                }

                set
                {
                    EstimatedCost = value;
                }
            }

            public Node(NodeData data)
            {
                this.data = data;
            }

            public int CompareTo(object obj)
            {
                Node n = (Node)obj;

                return key.CompareTo(n.key);
            }

            public override string ToString()
            {
                return "Node - IntermediateCost: " + IntermediateCost + ", EstimatedCost: " + EstimatedCost + " data: " + data.ToString();
            }
        }

        public class EdgeData : Graphs.DirectedGraphEdge<EData>
        {
            public EData data { get; set; }
            public double cost;
            
            public EdgeData(EData data, double cost)
            {
                this.data = data;
                this.cost = cost;
            }

            public override string ToString()
            {
                return "Edge - cost: " + cost + ", data: " + data.ToString();
            }
        }
    }
}
