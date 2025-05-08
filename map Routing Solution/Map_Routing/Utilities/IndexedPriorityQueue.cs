using System;
using System.Collections.Generic;

namespace ShortestPathFinder.MapRouting.Utilities
{
    // A priority queue with index tracking for efficient updates, optimized for Dijkstra's algorithm
    public class IndexedPriorityQueue
    {
        public List<int> heap; // Stores node indices
        private Dictionary<int, int> positions; // nodeId -> position in heap
        private Dictionary<int, double> priorities; // nodeId -> priority

        public IndexedPriorityQueue()
        {
            heap = new List<int>();
            positions = new Dictionary<int, int>();
            priorities = new Dictionary<int, double>();
        }

        public bool IsEmpty => heap.Count == 0;

        public void Insert(int nodeId, double priority)
        {
            if (Contains(nodeId))
                return;

            priorities[nodeId] = priority;
            heap.Add(nodeId);
            int pos = heap.Count - 1;
            positions[nodeId] = pos;
            HeapifyUp(pos);
        }

        public void DecreaseKey(int nodeId, double newPriority)
        {
            if (!Contains(nodeId) || newPriority >= priorities[nodeId])
                return;

            priorities[nodeId] = newPriority;
            HeapifyUp(positions[nodeId]);
        }

        public (int, double) Pull()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");

            int minNode = heap[0];
            double minPriority = priorities[minNode];

            Swap(0, heap.Count - 1);
            heap.RemoveAt(heap.Count - 1);

            positions.Remove(minNode);
            priorities.Remove(minNode);

            if (heap.Count > 0)
                HeapifyDown(0);

            return (minNode, minPriority);
        }

        public bool Contains(int nodeId)
        {
            return positions.ContainsKey(nodeId);
        }

        public double GetPriority(int nodeId)
        {
            return priorities.TryGetValue(nodeId, out double value) ? value : double.PositiveInfinity;
        }

        public double GetMinKey()
        {
            if (IsEmpty)
                return double.MaxValue;
            return priorities[heap[0]];
        }

        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (priorities[heap[i]] >= priorities[heap[parent]])
                    break;
                Swap(i, parent);
                i = parent;
            }
        }

        private void HeapifyDown(int i)
        {
            int left, right, smallest;
            while (true)
            {
                left = 2 * i + 1;
                right = 2 * i + 2;
                smallest = i;

                if (left < heap.Count && priorities[heap[left]] < priorities[heap[smallest]])
                    smallest = left;

                if (right < heap.Count && priorities[heap[right]] < priorities[heap[smallest]])
                    smallest = right;

                if (smallest == i) break;

                Swap(i, smallest);
                i = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            int temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;

            positions[heap[i]] = i;
            positions[heap[j]] = j;
        }
    }
}