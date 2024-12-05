namespace AdventOfCode._2024
{
    internal class Day5 : Day
    {
        HashSet<(int, int)> before = new();
        List<List<int>> lists = new();

        void ReadData()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            foreach (string line in split[0].SplitLines())
            {
                int[] vals = line.Split('|').ToInts().ToArray();

                before.Add((vals[0], vals[1]));
            }

            lists = split[1].SplitLines().Select(l => l.ToInts(',').ToList()).ToList();
        }

        public override long Compute()
        {
            ReadData();

            long result = 0;

            foreach (var list in lists)
            {
                var list2 = new List<int>(list);

                list2.Sort((a, b) => before.Contains((a, b)) ? -1 : (before.Contains((b, a)) ? 1 : 0));

                if (list.SequenceEqual(list2))
                {
                    int mid = list.Count / 2;

                    result += list[mid];
                }
            }

            return result;
        }

        public override long Compute2()
        {
            ReadData();

            long result = 0;

            foreach (var list in lists)
            {
                var list2 = new List<int>(list);

                list2.Sort((a, b) => before.Contains((a, b)) ? -1 : (before.Contains((b, a)) ? 1 : 0));  

                if (!list.SequenceEqual(list2))
                {
                    int mid = list.Count / 2;

                    result += list2[mid];
                }
            }

            return result;
        }
    }
}
