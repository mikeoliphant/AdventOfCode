namespace AdventOfCode._2021
{
    public class Day6
    {
        long[] fishHistogram = new long[9];

        void ReadInput()
        {
            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2021\DaySixInput.txt"))
            {
                string line = reader.ReadLine();

                foreach (int fish in (from string intStr in line.Split(',') select int.Parse(intStr)))
                {
                    fishHistogram[fish]++;
                }
            }
        }

        public void Compute()
        {
            ReadInput();

            int numDays = 256;

            for (int i = 0; i < numDays; i++)
            {
                long[] newHistogram = new long[9];

                newHistogram[8] = fishHistogram[0];

                long newFish = fishHistogram[0];

                for (int pos = 1; pos < 9; pos++)
                {
                    newHistogram[pos - 1] = fishHistogram[pos];
                }

                newHistogram[6] += newFish;

                fishHistogram = newHistogram;
            }

            long numFish = 0;

            for (int pos = 0; pos < 9; pos++)
            {
                numFish += fishHistogram[pos];
            }
        }
    }
}
