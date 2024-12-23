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

        List<List<string>> ExpandGroups(IEnumerable<IEnumerable<string>> currentGroups)
        {
            List<List<string>> newGroups = new List<List<string>>();

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

                            if (!ContainsGroup(newGroups, newList))
                            {
                                newGroups.Add(newList);
                            }
                        }
                    }
                }
            }

            return newGroups;
        }

        bool CanLinkT(IEnumerable<string> group)
        {
            if (group.Where(c => c.StartsWith('t')).Any())
            {
                return true;
            }

            return group.Where(c => connections[c].Where(ct => ct.StartsWith('t')).Any()).Any();
        }

        public override long Compute2()
        {
            ReadData();

            var groups = computers.Select(c => new List<string> { c }).ToList();

            int size = 1;

            do
            {
                var newGroups = ExpandGroups(groups).ToList();

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
