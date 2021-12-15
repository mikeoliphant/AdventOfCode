using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class GraphEdge<T>
    {
        public T From { get; set; }
        public T To { get; set; }

        public override string ToString()
        {
            return "(" + From + ")->(" + To + ")";
        }
    }

    public class DijkstraSearch<T> where T : IEquatable<T>
    {
        Func<T, T, float> EstimateCost { get; set; }

        Func<T, IEnumerable<T>> getNeighbors;
        Func<T, T, float> getNeighborCost;

        public DijkstraSearch(Func<T, IEnumerable<T>> getNeighbors, Func<T, T, float> getNeighborCost)
        {
            this.getNeighbors = getNeighbors;
            this.getNeighborCost = getNeighborCost;
        }

        public bool GetShortestPath(T start, T end, out List<T> path, out float cost)
        {
            PriorityQueue<GraphEdge<T>, float> searchQueue = new PriorityQueue<GraphEdge<T>, float>();
            Dictionary<T, T> pred = new Dictionary<T, T>(); 

            path = null;
            cost = float.MaxValue;

            foreach (T neighbor in getNeighbors(start))
            {
                searchQueue.Enqueue(new GraphEdge<T> { From = start, To = neighbor }, getNeighborCost(start, neighbor));
            }

            do
            {
                GraphEdge<T> toSearch;

                searchQueue.TryDequeue(out toSearch, out cost);

                if (!pred.ContainsKey(toSearch.To))
                {
                    pred[toSearch.To] = toSearch.From;

                    if (toSearch.To.Equals(end))
                    {
                        path = new List<T>();

                        T val = toSearch.To;

                        while (pred.ContainsKey(val))
                        {
                            path.Insert(0, val);

                            if (val.Equals(start))
                                break;

                            val = pred[val];
                        }

                        path.Insert(0, start);

                        return true;
                    }

                    foreach (T neighbor in getNeighbors(toSearch.To))
                    {
                        searchQueue.Enqueue(new GraphEdge<T> { From = toSearch.To, To = neighbor }, cost + getNeighborCost(toSearch.To, neighbor));
                    }
                }
            }
            while (searchQueue.Count > 0);

            return false;
        }
    }

    public class DepthFirstSearch<T> where T : IEquatable<T>
    {
        Func<T, IEnumerable<T>> getNeighbors;
        Func<T, T, float> getNeighborCost;

        public DepthFirstSearch(Func<T, IEnumerable<T>> getNeighbors, Func<T, T, float> getNeighborCost)
        {
            this.getNeighbors = getNeighbors;
            this.getNeighborCost = getNeighborCost;
        }

        public bool GetShortestPath(T start, T end, out List<T> path, out float cost)
        {
            if (start.Equals(end))
            {
                path = new List<T> { end };
                cost = 0;

                return true;
            }

            List<T> minCostPath = null;
            float minCost = float.MaxValue;

            foreach (T neighbor in getNeighbors(start))
            {
                List<T> neighborPath;
                float neighborCost;

                if (GetShortestPath(neighbor, end, out neighborPath, out neighborCost))
                {
                    neighborCost += getNeighborCost(start, neighbor);

                    if (neighborCost < minCost)
                    {
                        minCostPath = neighborPath;
                        minCost = neighborCost;
                    }
                }
            }

            path = minCostPath;
            path.Insert(0, start);

            cost = minCost;

            return true;
        }
    }
}
