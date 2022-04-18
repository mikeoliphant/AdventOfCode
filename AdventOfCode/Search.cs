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
        Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors;

        public DijkstraSearch(Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors)
        {
            this.getNeighbors = getNeighbors;
        }

        public bool GetShortestPath(T start, T end, out List<T> path, out float cost)
        {
            return GetShortestPath(start, delegate (T t) { return t.Equals(end); }, out path, out cost);
        }

        public bool GetShortestPath(T start, Func<T, bool> endCheck, out List<T> path, out float cost)
        {
            if (endCheck(start))
            {
                path = new List<T>();
                cost = 0;

                return true;
            }

            PriorityQueue<GraphEdge<T>, float> searchQueue = new PriorityQueue<GraphEdge<T>, float>();
            Dictionary<T, T> pred = new Dictionary<T, T>(); 

            path = null;
            cost = float.MaxValue;

            foreach (var neighbor in getNeighbors(start))
            {
                searchQueue.Enqueue(new GraphEdge<T> { From = start, To = neighbor.Key }, neighbor.Value);
            }

            while (searchQueue.Count > 0)
            {
                GraphEdge<T> toSearch;

                searchQueue.TryDequeue(out toSearch, out cost);

                if (!pred.ContainsKey(toSearch.To))
                {
                    pred[toSearch.To] = toSearch.From;

                    if (endCheck(toSearch.To))
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

                        //path.Insert(0, start);

                        return true;
                    }

                    foreach (var neighbor in getNeighbors(toSearch.To))
                    {
                        searchQueue.Enqueue(new GraphEdge<T> { From = toSearch.To, To = neighbor.Key }, cost + neighbor.Value);
                    }
                }
            }

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
