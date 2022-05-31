namespace AdventOfCode._2015
{
    internal class Day17 : Day
    {
        //int[] containers = new int[] { 20, 15, 10, 5, 5 };
        int[] containers = null;

        IEnumerable<int[]> NumWaysToFill(int[] fills, int pos, int left)
        {
            if (pos == (containers.Length - 1))
            {
                if (containers[pos] == left)
                {
                    fills[pos] = left;

                    yield return fills;
                }

                if (left == 0)
                {
                    fills[pos] = 0;

                    yield return fills;
                }
            }
            else
            {
                fills[pos] = 0;

                foreach (var result in NumWaysToFill(fills, pos + 1, left))
                {
                    yield return result;
                }

                fills[pos] = left;

                foreach (var result in NumWaysToFill(fills, pos + 1, left - containers[pos]))
                {
                    yield return result;
                }
            }
        }

        public override long Compute()
        {
            containers = File.ReadAllLines(DataFile).ToInts().ToArray();

            int toStore = 150;

            return NumWaysToFill(new int[containers.Length], 0, toStore).Count();
        }

        public override long Compute2()
        {
            containers = File.ReadAllLines(DataFile).ToInts().ToArray();

            int toStore = 150;

            var result = NumWaysToFill(new int[containers.Length], 0, toStore);

            int min = result.Min(r => r.Where(c => c != 0).Count());

            return result.Where(r => r.Where(c => c != 0).Count() == min).Count();
        }
    }
}
