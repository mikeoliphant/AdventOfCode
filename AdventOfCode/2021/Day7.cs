namespace AdventOfCode._2021
{
    public class Day7
    {
        int[] crabs;

        void ReadInput()
        {
            crabs = (from crabStr in File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day7.txt").Split(',') select int.Parse(crabStr)).ToArray();
        }

        public long Compute()
        {
            ReadInput();

            int maxPos = crabs.Max();

            long minDist = int.MaxValue;
            long minPos = -1;

            for (int pos = 0; pos <= maxPos; pos++)
            {
                int dist = 0;

                for (int i = 0; i < crabs.Length; i++)
                {
                    dist += Math.Abs(pos - crabs[i]);
                }

                if (dist < minDist)
                {
                    minDist = dist;
                    minPos = pos;
                }
            }

            return minDist;
        }

        public long Compute2()
        {
            ReadInput();

            int maxPos = crabs.Max();

            long minDist = (from pos in Enumerable.Range(0, maxPos) select (from crab in crabs select ((Math.Abs(pos - crab) * (Math.Abs(pos - crab) + 1)) / 2)).Sum()).Min();

            //long minDist = int.MaxValue;
            //long minPos = -1;

            //for (int pos = 0; pos <= maxPos; pos++)
            //{
            //    long dist = 0;

            //    for (int i = 0; i < crabs.Length; i++)
            //    {
            //        int crabDist = Math.Abs(pos - crab);

            //        dist += (crabDist * (crabDist + 1)) / 2;
            //    }

            //    if (dist < minDist)
            //    {
            //        minDist = dist;
            //        minPos = pos;
            //    }
            //}

            return minDist;
        }
    }
}
