namespace AdventOfCode._2023
{
    internal class Day25 : Day
    {
        Dictionary<string, List<string>> components = new();

        void Link(string component1, string component2)
        {
            if (!components.ContainsKey(component1))
            {
                components[component1] = new List<string>();
            }

            components[component1].Add(component2);
        }

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] fromTo = line.Split(": ");

                string[] to = fromTo[1].SplitWhitespace();

                foreach (string toComponent in to)
                {
                    Link(fromTo[0], toComponent);
                    Link(toComponent, fromTo[0]);
                }
            }
        }

        IEnumerable<KeyValuePair<string, float>> GetNeighbors(string state)
        {
            foreach (string other in components[state])
            {
                yield return new KeyValuePair<string, float>(other, 1);
            }
        }

        Dictionary<(string, string), long> pairCounts = new();

        void AddPair((string, string) pair)
        {
            if (pair.Item1.CompareTo(pair.Item2) > 0)
            {
                pair = (pair.Item2, pair.Item1);
            }

            if (!pairCounts.ContainsKey(pair))
            {
                pairCounts[pair] = 1;
            }
            else
            {
                pairCounts[pair]++;
            }
        }

        public override long Compute()
        {
            ReadData();

            //long tot = components.Values.Select(l => l.Count).Sum();

            //long max = components.Values.Select(l => l.Count).Max();

            //long min = components.Values.Select(l => l.Count).Min();

            DijkstraSearch<string> search = new(GetNeighbors);

            List<string> allComponents = components.Keys.ToList();

            long count = 0;

            for (int i = 0; i < allComponents.Count; i++)
            {
                for (int i2 = i + 1; i2 < allComponents.Count; i2++)
                {
                    var path = search.GetShortestPath(allComponents[i], allComponents[i2]);

                    foreach (var pair in path.Path.Zip(path.Path.Skip(1)))
                    {
                        AddPair(pair);
                    }

                    count++;

                    if (count > 50000)
                    {
                        i = allComponents.Count;

                        break;
                    }
                }
            }

            var sorted = pairCounts.OrderByDescending(p => p.Value).Take(3).ToList();

            //using (StreamWriter writer = new StreamWriter(@"C:\tmp\sorted.txt"))
            //{
            //    foreach (var pair in sorted)
            //    {
            //        writer.WriteLine(pair.ToString());
            //    }
            //}

            return sorted.Count;
        }
    }
}
