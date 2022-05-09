namespace AdventOfCode._2016
{
    internal class Day6
    {

        public long Compute()
        {
            Dictionary<char, int>[] posHist = null;

            foreach (string msg in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day6.txt"))
            {
                if (posHist == null)
                {
                    posHist = new Dictionary<char, int>[msg.Length];

                    for (int pos = 0; pos < posHist.Length; pos++)
                    {
                        posHist[pos] = new Dictionary<char, int>();
                    }
                }

                for (int pos = 0; pos < posHist.Length; pos++)
                {
                    if (!posHist[pos].ContainsKey(msg[pos]))
                    {
                        posHist[pos][msg[pos]] = 1;
                    }
                    else
                    {
                        posHist[pos][msg[pos]]++;
                    }
                }
            }
            

            string message = "";

            for (int pos = 0; pos < posHist.Length; pos++)
            {
                message += posHist[pos].OrderByDescending(p => p.Value).First().Key;
            }

            return 0;
        }

        public long Compute2()
        {
            Dictionary<char, int>[] posHist = null;

            foreach (string msg in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day6.txt"))
            {
                if (posHist == null)
                {
                    posHist = new Dictionary<char, int>[msg.Length];

                    for (int pos = 0; pos < posHist.Length; pos++)
                    {
                        posHist[pos] = new Dictionary<char, int>();
                    }
                }

                for (int pos = 0; pos < posHist.Length; pos++)
                {
                    if (!posHist[pos].ContainsKey(msg[pos]))
                    {
                        posHist[pos][msg[pos]] = 1;
                    }
                    else
                    {
                        posHist[pos][msg[pos]]++;
                    }
                }
            }


            string message = "";

            for (int pos = 0; pos < posHist.Length; pos++)
            {
                message += posHist[pos].OrderBy(p => p.Value).First().Key;
            }

            return 0;
        }
    }
}
