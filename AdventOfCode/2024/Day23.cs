namespace AdventOfCode._2024
{
    internal class Day23 : Day
    {
        Dictionary<string, List<string>> connections = new();
        List<string> computers = null;

        bool IsConnectedTo(string a, string b)
        {
            return connections[a].Contains(b);
        }

        void AddPair(string a, string b)
        {
            if (!connections.ContainsKey(a))
            {
                connections[a] = new List<string>();
            }

            connections[a].Add(b);
        }

        void ReadData()
        {
            foreach (string[] pair in File.ReadLines(DataFile).Select(p => p.Split('-')))
            {
                AddPair(pair[0], pair[1]);
                AddPair(pair[1], pair[0]);
            }

            computers = connections.Keys.ToList();
        }

        public override long Compute()
        {
            ReadData();

            var computers = connections.Keys.ToList();

            List<(string, string, string)> triads = new List<(string, string, string)>();

            for (int c1 = 0; c1 < computers.Count; c1++)
            {
                for (int c2 = c1 + 1; c2 < computers.Count; c2++)
                {
                    if (IsConnectedTo(computers[c1], computers[c2]))
                    {
                        for (int c3 = c2 + 1; c3 < connections.Count; c3++)
                        {
                            if (IsConnectedTo(computers[c1], computers[c3]) && IsConnectedTo(computers[c2], computers[c3]))
                            {
                                triads.Add((computers[c1], computers[c2], computers[c3]));
                            }
                        }
                    }
                }
            }

            var withT = triads.Where(t => t.Item1.StartsWith('t') || t.Item2.StartsWith('t') || t.Item3.StartsWith('t')).ToList();

            return withT.Count;
        }

        bool ContainsGroup(IEnumerable<IEnumerable<string>> groups, IEnumerable<string> group)
        {
            foreach (var g in groups)
            {
                if (g.SequenceEqual(group))
                {
                    return true;
                }
            }

            return false;
        }

        IEnumerable<List<string>> ExpandGroups(IEnumerable<IEnumerable<string>> currentGroups)
        {
            foreach (var g in currentGroups)
            {
                foreach (var other in computers)
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
                            var newList = new List<string>(g) { other };

                            newList.Sort();

                            yield return newList;
                        }
                    }
                }
            }
        }

        int CompareLists(List<string> a, List<string> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                int c = a[i].CompareTo(b[i]);

                if (c != 0)
                {
                    return c;
                }
            }

            return 0;
        }

        IEnumerable<List<string>> RemoveDupes(List<List<string>> groups)
        {
            groups.Sort(CompareLists);

            List<string>? last = null;

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

        public override long Compute2()
        {
            ReadData();

            var groups = computers.Select(c => new List<string> { c }).ToList();

            int size = 1;

            do
            {
                var newGroups = RemoveDupes(ExpandGroups(groups).ToList()).ToList();

                if (newGroups.Count == 0)
                {
                    break;
                }

                groups = newGroups;

                size++;
                Console.WriteLine("Size: " + size + " - " + groups.Count);
            }
            while (true);

            foreach (var g in groups)
            {
                Console.WriteLine();
                Console.WriteLine(string.Join(",", g));
            }

            return 0;
        }
    }
}
