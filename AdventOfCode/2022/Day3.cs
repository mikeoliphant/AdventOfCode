namespace AdventOfCode._2022
{
    internal class Day3 : Day
    {
        int Priority(char item)
        {
            if (item >= 'a')
                return (item - 'a') + 1;

            return (item - 'A') + 27;
        }

        public override long Compute()
        {
            long priSum = 0;

            foreach (string sack in File.ReadLines(DataFile))
            {
                string c1 = sack.Substring(0, sack.Length / 2);
                string c2 = sack.Substring(sack.Length / 2);

                foreach (char item in c1)
                {
                    if (c2.Contains(item))
                    {
                        priSum += Priority(item);

                        break;
                    }
                }
            }

            return priSum;
        }

        public override long Compute2()
        {
            long priSum = 0;

            foreach (var group in File.ReadLines(DataFile).ToArray().Partition(3))
            {
                foreach (char item in group[0])
                {
                    if (group[1].Contains(item) && (group[2].Contains(item)))
                    {
                        priSum += Priority(item);

                        break;
                    }
                }
            }

            return priSum;
        }
    }
}
