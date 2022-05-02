namespace AdventOfCode._2017
{
    internal class Day6
    {
        public long Compute()
        {
            //int[] banks = new int[] { 0, 2, 7, 0 };

            int[] banks = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day6.txt").Trim().SplitWhitespace().ToInts().ToArray();

            Dictionary<string, bool> history = new Dictionary<string, bool>();

            int max = 0;
            int cycle = 0;
            int repeatCycle = 0;
            string repeat = null;

            do
            {
                string key = string.Join(',', banks);

                if (repeat != null)
                {
                    if (key == repeat)
                    {
                        return cycle - repeatCycle;
                    }
                }
                else
                {
                    if (history.ContainsKey(key))
                    {
                        //return cycle;

                        repeatCycle = cycle;
                        repeat = key;
                    }
                    else
                    {
                        history[key] = true;
                    }
                }

                do
                {
                    max = banks.Max();
                    int bank = Array.IndexOf(banks, max);

                    banks[bank++] = 0;

                    while (max > 0)
                    {
                        if (bank == banks.Length)
                            bank = 0;

                        banks[bank++]++;

                        max--;
                    }
                }
                while (max > 0);

                cycle++;
            }
            while (true);

            throw new InvalidOperationException();
        }
    }
}
