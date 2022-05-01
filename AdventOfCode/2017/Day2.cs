namespace AdventOfCode._2017
{
    internal class Day2
    {
        public long Compute()
        {
            int checksum = 0;

            foreach (string row in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day2.txt"))
            {
                var rowValues = row.SplitWhitespace().ToInts();

                checksum += rowValues.Max() - rowValues.Min();
            }

            return checksum;
        }

        public long Compute2()
        {
            int checksum = 0;

            foreach (string row in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day2.txt"))
            {
                int[] rowValues = row.SplitWhitespace().ToInts().ToArray();

                for (int pos1 = 0; pos1 < rowValues.Length; pos1++)
                {
                    for (int pos2 = pos1 + 1; pos2 < rowValues.Length; pos2++)
                    { 
                        int min;
                        int max;

                        if (rowValues[pos1] > rowValues[pos2])
                        {
                            min = rowValues[pos2];
                            max = rowValues[pos1];
                        }
                        else
                        {
                            min = rowValues[pos1];
                            max = rowValues[pos2];
                        }

                        if (((max / min) * min) == max)
                        {
                            checksum += (max / min);

                            break;
                        }
                    }
                }
            }

            return checksum;
        }
    }
}
