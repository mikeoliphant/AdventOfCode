namespace AdventOfCode._2023
{
    internal class Day6 : Day
    {
        public override long Compute()
        {
            var lines = File.ReadLines(DataFile).ToArray();

            int[] times = Regex.Split(lines[0], @"\s+").Skip(1).ToInts().ToArray();
            int[] distances = Regex.Split(lines[1], @"\s+").Skip(1).ToInts().ToArray();

            int prod = 1;

            for (int i = 0; i < times.Length; i++)
            {
                int numWaysToWin = 0;

                for (int hold = 0; hold <= times[i]; hold++)
                {
                    int dist = hold * (times[i] - hold);

                    if (dist > distances[i])
                    {
                        numWaysToWin++;
                    }
                }

                prod *= numWaysToWin;
            }

            return prod;
        }

        public override long Compute2()
        {
            var lines = File.ReadLines(DataFile).ToArray();

            long time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
            long distance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

            long numWaysToWin = 0;

            for (long hold = 0; hold <= time; hold++)
            {
                long dist = hold * (time - hold);

                if (dist > distance)
                {
                    numWaysToWin++;
                }
            }

            return base.Compute2();
        }
    }
}
