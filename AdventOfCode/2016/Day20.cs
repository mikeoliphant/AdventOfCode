namespace AdventOfCode._2016
{
    internal class Day20
    {
        long maxIP = 4294967295;
        List<(long Min, long Max)> ranges = new List<(long Min, long Max)>();

        void ReadInput()
        {
            foreach (string range in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day20.txt"))
            {
                long[] vals = range.ToLongs('-').ToArray();

                ranges.Add((vals[0], vals[1]));
            }
        }

        public long Compute()
        {
            ReadInput();

            return ranges.Where(r => (r.Min > 0) && !ranges.Where(r2 => ((r.Min - 1) >= r2.Min) && ((r.Min - 1) <= r2.Max)).Any()).OrderBy(r => r.Max).First().Min - 1;
        }

        public long Compute2()
        {
            ReadInput();

            long numValid = 0;

            long currentMin = 0;

            while (currentMin <= maxIP)
            {
                var min = ranges.Where(r => (r.Min > currentMin) && !ranges.Where(r2 => ((r.Min - 1) >= r2.Min) && ((r.Min - 1) <= r2.Max)).Any()).OrderBy(r => r.Max);

                if (!min.Any())
                    break;

                currentMin = min.First().Min - 1;

                long maxValidInRange = ranges.Where(r => r.Min > currentMin).OrderBy(r => r.Min).First().Min - 1;

                numValid += (maxValidInRange - currentMin) + 1;

                currentMin = maxValidInRange + 1;
            }

            return numValid;
        }
    }
}
