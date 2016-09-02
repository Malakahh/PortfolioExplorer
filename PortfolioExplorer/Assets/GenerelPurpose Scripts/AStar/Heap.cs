using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Heap
{
    class Heap
    {
        public enum HeapProperty { MaxHeap, MinHeap }

        List<HeapNode> nodes;
        int heapProperty;

        public int Count
        {
            get { return nodes.Count; }
        }

        public Heap(IEnumerable<HeapNode> nodes, HeapProperty heapProperty)
        {
            this.nodes = new List<HeapNode>(nodes);
            this.heapProperty = (heapProperty == HeapProperty.MaxHeap) ? 1 : -1;
        }

        // Copy constructor
        public Heap(Heap toCopy)
        {
            this.nodes = new List<HeapNode>(toCopy.nodes);
            this.heapProperty = toCopy.heapProperty;
        }

        int Parent(int i)
        {
            return i / 2;
        }

        int Left(int i)
        {
            return i * 2;
        }

        int Right(int i)
        {
            return i * 2 + 1;
        }

        /// <summary>
        ///     Maintains the heap property.
        ///     Assumes Left(i) and Right(i) maintain the heap property heaps.
        /// </summary>
        /// <param name="i">Index into nodes. This node might not satisfy the heap property.</param>
        void Heapify(int i)
        {
            int l = Left(i);
            int r = Right(i);

            int largest = i;

            if (l < nodes.Count && nodes[l].CompareTo(nodes[largest]) * heapProperty > 0)
            {
                largest = l;
            }

            if (r < nodes.Count && nodes[r].CompareTo(nodes[largest]) * heapProperty > 0)
            {
                largest = r;
            }

            if (largest != i)
            {
                Swap(i, largest);
                Heapify(largest);
            }
        }

        /// <summary>
        /// Iterates through all non-leaves of the tree, and runs Heapify on each one.
        /// </summary>
        public void BuildHeap()
        {
            for (int i = (nodes.Count - 1) / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        /// <summary>
        /// Alters a key's value whilst maintaining the heap property
        /// </summary>
        /// <param name="i">Index of key to increase</param>
        /// <param name="key">new value of key</param>
        void HeapAlterKey(int i, double key)
        {
            if (key.CompareTo(nodes[i].key) * heapProperty < 0)
            {
                throw new ArgumentException("new key is not valid"); //This should be handled better at some point. Just call buildheap again?
            }

            nodes[i].key = key;
            while (i > 0 && nodes[Parent(i)].CompareTo(nodes[i]) * heapProperty < 0)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        /// <summary>
        /// Inserts a new node into the heap whilst maintaining the heap property
        /// </summary>
        /// <param name="newNode"></param>
        public void HeapInsert(HeapNode newNode)
        {
            nodes.Add(newNode);
            HeapAlterKey(nodes.Count - 1, newNode.key);
        }


        /// <summary>
        /// Extracts the root node from the Heap. Maintains the heap property
        /// </summary>
        /// <returns>root node</returns>
        public HeapNode HeapExtractRoot()
        {
            if (nodes.Count < 1)
            {
                throw new IndexOutOfRangeException("No nodes in heap");
            }

            HeapNode root = nodes[0];
            nodes[0] = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);

            Heapify(0);

            return root;
        }

        /// <summary>
        /// Determines whether an element is in the heap
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(HeapNode node)
        {
            return nodes.Contains(node);
        }

        /// <summary>
        /// Verifies the Heap property
        /// </summary>
        /// <returns>Whether it is a max heap or a min heap</returns>
        public bool VerifyHeapProperty(HeapProperty hp)
        {
            bool satisfyHeapProperty = true;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[Parent(i)].CompareTo(nodes[i]) * ((hp == HeapProperty.MaxHeap) ? 1 : -1) < 0)
                {
                    satisfyHeapProperty = false;
                }
            }

            return satisfyHeapProperty;
        }

        /// <summary>
        /// Swaps two nodes' position.
        /// </summary>
        /// <param name="a">Index of first node.</param>
        /// <param name="b">Index of second node.</param>
        void Swap(int a, int b)
        {
            HeapNode temp = nodes[b];
            nodes[b] = nodes[a];
            nodes[a] = temp;
        }

        public override string ToString()
        {
            string s = "Nodes: ";

            for (int i = 0; i < nodes.Count; i++)
            {
                s += nodes[i].key + ", ";
            }

            return s.Remove(s.Length - 2);
        }

    }

    interface HeapNode : IComparable
    {
        double key { get; set; }
    }
}
