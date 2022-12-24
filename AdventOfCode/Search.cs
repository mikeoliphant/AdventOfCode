namespace AdventOfCode
{
    public class SearchBase<T> where T : IEquatable<T>
    {
        protected Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors;
        protected Func<T, IEnumerable<T>> getNeighborsNoCost;

        public SearchBase(Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors)
        {
            this.getNeighbors = getNeighbors;
        }

        public SearchBase(Func<T, IEnumerable<T>> getNeighborsNoCost)
        {
            this.getNeighbors = GetFixedCostNeighbors;
            this.getNeighborsNoCost = getNeighborsNoCost;
        }

        protected IEnumerable<KeyValuePair<T, float>> GetFixedCostNeighbors(T currentState)
        {
            foreach (T state in getNeighborsNoCost(currentState))
            {
                yield return new KeyValuePair<T, float>(state, 1.0f);
            }
        }

        public bool GetShortestPath(T start, T end, out List<T> path, out float cost)
        {
            return GetShortestPath(start, delegate (T t) { return t.Equals(end); }, out path, out cost);
        }

        public (List<T> Path, float Cost) GetShortestPath(T start, T end)
        {
            List<T> path = null;
            float cost = float.MaxValue;

            GetShortestPath(start, delegate (T t) { return t.Equals(end); }, out path, out cost);

            return (path, cost);
        }

        public (List<T> Path, float Cost) GetShortestPath(T start, Func<T, bool> endCheck)
        {
            List<T> path = null;
            float cost = float.MaxValue;

            GetShortestPath(start, endCheck, out path, out cost);

            return (path, cost);
        }

        public virtual bool GetShortestPath(T start, Func<T, bool> endCheck, out List<T> path, out float cost)
        {
            throw new NotImplementedException();
        }
    }

    public class GraphEdge<T>
    {
        public T From { get; set; }
        public T To { get; set; }

        public override string ToString()
        {
            return "(" + From + ")->(" + To + ")";
        }
    }

    public class DijkstraSearch<T> : SearchBase<T> where T : IEquatable<T>
    {
        Dictionary<T, T> pred = new Dictionary<T, T>();
        PriorityQueue<GraphEdge<T>, float> searchQueue = new PriorityQueue<GraphEdge<T>, float>();

        public DijkstraSearch(Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors)
            : base(getNeighbors)
        {
        }

        public DijkstraSearch(Func<T, IEnumerable<T>> getNeighborsNoCost)
            : base(getNeighborsNoCost)
        {
        }

        public void SetSearchQueue(PriorityQueue<GraphEdge<T>, float> searchQueue)
        {
            this.searchQueue = searchQueue;
        }

        public override bool GetShortestPath(T start, Func<T, bool> endCheck, out List<T> path, out float cost)
        {
            if (endCheck(start))
            {
                path = new List<T>();
                cost = 0;

                return true;
            }

            pred.Clear();
            searchQueue.Clear();

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

    public class DepthFirstSearch<T> : SearchBase<T> where T : IEquatable<T>
    {
        public DepthFirstSearch(Func<T, IEnumerable<KeyValuePair<T, float>>> getNeighbors)
            : base(getNeighbors)
        {
        }

        public DepthFirstSearch(Func<T, IEnumerable<T>> getNeighborsNoCost)
            : base(getNeighborsNoCost)
        {
        }

        public bool FindFirstPath(T start, T end, out List<T> path, out float cost)
        {
            if (start.Equals(end))
            {
                path = new List<T> { end };
                cost = 0;

                return true;
            }

            foreach (var neighbor in getNeighbors(start))
            {
                List<T> neighborPath;

                float neighborCost;

                if (FindFirstPath(neighbor.Key, end, out neighborPath, out neighborCost))
                {
                    neighborCost += neighbor.Value;

                    path = neighborPath;
                    cost = neighborCost;

                    path.Insert(0, start);

                    return true;
                }
            }

            path = null;
            cost = 0;

            return false;
        }

        public override bool GetShortestPath(T start, Func<T, bool> endCheck, out List<T> path, out float cost)
        {
            if (endCheck(start))
            {
                path = new List<T>();
                cost = 0;

                return true;
            }

            List<T> minCostPath = null;
            float minCost = float.MaxValue;

            foreach (var neighbor in getNeighbors(start))
            {
                List<T> neighborPath;
                float neighborCost;

                if (GetShortestPath(neighbor.Key, endCheck, out neighborPath, out neighborCost))
                {
                    neighborCost += neighbor.Value;

                    if (neighborCost < minCost)
                    {
                        minCostPath = neighborPath;
                        minCost = neighborCost;
                    }
                }
            }

            path = minCostPath;
            cost = minCost;

            if (path == null)
                return false;

            path.Insert(0, start);

            cost = minCost;

            return true;
        }
    }
}
