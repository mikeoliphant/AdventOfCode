using Microsoft.VisualBasic.Devices;
using System.Drawing;

namespace AdventOfCode
{
    public class Graph<T> where T : IComparable<T>
    {
        Dictionary<T, HashSet<T>> connections = new();

        public ICollection<T> Nodes { get { return connections.Keys; } }

        void AddConnection(T a, T b)
        {
            if (!connections.ContainsKey(a))
            {
                connections[a] = new HashSet<T>();
            }

            connections[a].Add(b);
        }

        public void Connect(T a, T b)
        {
            AddConnection(a, b);
            AddConnection(b, a);
        }

        public bool IsConnectedTo(T a, T b)
        {
            return connections[a].Contains(b);
        }

        int CompareLists(List<T> a, List<T> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                int compare = a[i].CompareTo(b[i]);

                if (compare != 0)
                {
                    return compare;
                }
            }

            return 0;
        }

        IEnumerable<List<T>> RemoveDupes(List<List<T>> groups)
        {
            groups.Sort(CompareLists);

            List<T>? last = null;

            foreach (var g in groups)
            {
                if (last != null)
                {
                    if (!g.SequenceEqual(last))
                    {
                        yield return g;
                    }
                }
                else
                {
                    yield return g;
                }

                last = g;
            }
        }

        IEnumerable<List<T>> ExpandGroups(IEnumerable<IEnumerable<T>> currentGroups)
        {
            foreach (var g in currentGroups)
            {
                foreach (var other in Nodes)
                {
                    if (!g.Contains(other))
                    {
                        bool canAdd = true;

                        foreach (var c in g)
                        {
                            if (!IsConnectedTo(c, other))
                            {
                                canAdd = false;
                                break;
                            }
                        }

                        if (canAdd)
                        {
                            var newList = new List<T>(g) { other };

                            newList.Sort();

                            yield return newList;
                        }
                    }
                }
            }
        }

        public List<List<T>> GetMaxSizeCliques()
        {
            var groups = Nodes.Select(c => new List<T> { c }).ToList();

            do
            {
                var newGroups = RemoveDupes(ExpandGroups(groups).ToList()).ToList();

                if (newGroups.Count == 0)
                {
                    return groups;
                }

                groups = newGroups;
            }
            while (true);
        }

        public List<List<T>> ExpandCliques(List<List<T>> cliques)
        {
            return RemoveDupes(ExpandGroups(cliques).ToList()).ToList();
        }

        public List<List<T>> GetCliques(int size)
        {
            var groups = Nodes.Select(c => new List<T> { c }).ToList();

            while (size > 1)
            {
                var newGroups = ExpandCliques(groups);

                if (newGroups.Count == 0)
                {
                    return new List<List<T>>();
                }

                groups = newGroups;

                size--;
            }

            return groups;
        }
    }
}
