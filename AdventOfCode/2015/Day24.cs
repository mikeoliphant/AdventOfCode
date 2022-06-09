namespace AdventOfCode._2015
{
    internal class Day24 : Day
    {
        int[] weights;

        long Product(int[] set)
        {
            long product = 1;

            foreach (int num in set)
            {
                product *= num;
            }

            return product;
        }

        IEnumerable<int[]> AllSubsetsSum(int n, List<int> v, int sum)
        {
            if (sum == 0)
            {
                yield return v.ToArray();
            }
            else if (n > 0)
            {
                foreach (var subset in AllSubsetsSum(n - 1, v, sum))
                    yield return subset;

                List<int> v1 = new List<int>(v);
                v1.Add(weights[n - 1]);

                foreach (var subset in AllSubsetsSum(n - 1, v1, sum - weights[n - 1]))
                    yield return subset;
            }
        }
        public override long Compute()
        {
            weights = File.ReadLines(DataFile).ToInts().ToArray();

            int equalWeight = weights.Sum() / 4;

            var subsets = AllSubsetsSum(weights.Length, new List<int>(), equalWeight).ToArray();

            var sorted = subsets.OrderBy(s => s.Length);

            int smallestSize = sorted.First().Length;

            List<int[]> smallest = new List<int[]>(smallestSize);

            foreach (var subset in sorted)
            {
                if (subset.Length > smallestSize)
                    break;

                smallest.Add(subset);
            }

            return Product(smallest.OrderBy(s => Product(s)).First());
        }
    }
}
