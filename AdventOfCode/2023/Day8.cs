namespace AdventOfCode._2023
{
    internal class Day8 : Day
    {
        public override long Compute()
        {
            var data = File.ReadAllText(DataFile).SplitParagraphs();

            string directions = data[0];

            Dictionary<string, (string L, string R)> map = new();

            foreach (string nodeStr in data[1].SplitLines())
            {
                Match match = Regex.Match(nodeStr, @"(.*) = \((.*), (.*)\)");

                map[match.Groups[1].Value] = (match.Groups[2].Value, match.Groups[3].Value);
            }

            string node = "AAA";
            int dirPos = 0;

            int steps = 0;

            while (node != "ZZZ")
            {
                node = (directions[dirPos] == 'L') ? map[node].L : map[node].R;

                dirPos = (dirPos + 1) % directions.Length;
                steps++;
            }

            return steps;
        }

        public override long Compute2()
        {
            var data = File.ReadAllText(DataFile).SplitParagraphs();

            string directions = data[0];

            Dictionary<string, (string L, string R)> map = new();

            foreach (string nodeStr in data[1].SplitLines())
            {
                Match match = Regex.Match(nodeStr, @"(.*) = \((.*), (.*)\)");

                map[match.Groups[1].Value] = (match.Groups[2].Value, match.Groups[3].Value);
            }

            List<string> currentNodes = map.Keys.Where(k => k.EndsWith('A')).ToList();

            List<long> nodeSteps = new();

            foreach (string startNode in currentNodes)
            {
                string node = startNode;

                int dirPos = 0;

                long steps = 0;

                Dictionary<string, long> visitedZs = new();

                while (true)
                {
                    if (node.EndsWith('Z'))
                    {
                        if (visitedZs.ContainsKey(node))
                        {
                            Console.WriteLine(node + ": initial " + visitedZs[node] + "  loop: " + (steps - visitedZs[node]));

                            nodeSteps.Add(visitedZs[node]);

                            break;
                        }
                        else
                        {
                            visitedZs[node] = steps;
                        }
                    }

                    node = (directions[dirPos] == 'L') ? map[node].L : map[node].R;

                    dirPos = (dirPos + 1) % directions.Length;
                    steps++;
                }
            }

            long lcm = 1;

            foreach (long stepCount in nodeSteps)
            {
                lcm = FactorHelper.LeastCommonMultiple(lcm , stepCount);
            }

            return lcm;
        }
    }
}
