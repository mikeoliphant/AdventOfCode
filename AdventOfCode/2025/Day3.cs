

namespace AdventOfCode._2025
{
    internal class Day3 : Day
    {
        long GetMaxJolts(string batteries, int numBatteries)
        {
            string maxStr = "";

            while (numBatteries > 0)
            {
                char max = batteries.Take(batteries.Length - (numBatteries - 1)).Max();

                int pos = batteries.IndexOf(max);

                maxStr += max;

                batteries = batteries.Substring(pos + 1);

                numBatteries--;
            }

            return long.Parse(maxStr);
        }

        public override long Compute()
        {
            long sum = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                long maxJolts = GetMaxJolts(line, 2);

                Console.WriteLine(line + ": " + maxJolts);

                sum += maxJolts;
            }
            
            return sum;
        }

        public override long Compute2()
        {
            long sum = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                long maxJolts = GetMaxJolts(line, 12);

                Console.WriteLine(line + ": " + maxJolts);

                sum += maxJolts;
            }

            return sum;
        }
    }
}
