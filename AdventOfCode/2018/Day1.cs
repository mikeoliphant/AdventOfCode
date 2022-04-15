namespace AdventOfCode._2018
{
    internal class Day1
    {
        public long Compute()
        {
            int[] freqChanges = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day1.txt").Select(x => int.Parse(x)).ToArray();

            long currentFrequency = 0;

            foreach (int freq in freqChanges)
            {
                currentFrequency += freq;
            }

            return currentFrequency;
        }

        public long Compute2()
        {
            Dictionary<long, bool> freqHist = new Dictionary<long, bool>();

            int[] freqChanges = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day1.txt").Select(x => int.Parse(x)).ToArray();

            long currentFrequency = 0;

            do
            {
                foreach (int freq in freqChanges)
                {
                    if (freqHist.ContainsKey(currentFrequency))
                    {
                        return currentFrequency;
                    }

                    freqHist[currentFrequency] = true;

                    currentFrequency += freq;
                }
            }
            while (true);
        }
    }
}
