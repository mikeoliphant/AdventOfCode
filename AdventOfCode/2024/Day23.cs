namespace AdventOfCode._2024
{
    internal class Day23 : Day
    {
        Graph<string> graph = new();

        void ReadData()
        {
            foreach (string[] pair in File.ReadLines(DataFile).Select(p => p.Split('-')))
            {
                graph.Connect(pair[0], pair[1]);
            }
        }

        public override long Compute()
        {
            ReadData();

            var triads = graph.GetCliques(3);

            var withT = triads.Where(t => t.Where(c => c.StartsWith('t')).Any()).ToList();

            return withT.Count;
        }

        public override long Compute2()
        {
            ReadData();

            var groups = graph.GetCliques(1);

            int size = 1;

            do
            {
                var newGroups = graph.ExpandCliques(groups);

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
