namespace AdventOfCode
{
    public static class FactorHelper
    {
        public static IEnumerable<(int, int)> PrimeFactors(int n)
        {
            int num2 = 0;

            while (n % 2 == 0)
            {
                num2++;

                n /= 2;
            }

            if (num2 > 0)
                yield return (2, num2);

            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                int numI = 0;

                while (n % i == 0)
                {
                    numI++;

                    n /= i;
                }

                if (numI > 0)
                    yield return (i, numI);
            }

            if (n > 2)
                yield return (n, 1);
        }

        public static IEnumerable<int> AllFactors(int n)
        {
            var factors = PrimeFactors(n).ToList();

            if (factors.Count == 0)
                return new List<int> { n };
            else
            {
                return GetFactorCombos(factors, 0);
            }
        }

        static IEnumerable<int> GetFactorCombos(List<(int Factor, int NumTimes)> factors, int pos)
        {
            if (pos == (factors.Count - 1))
            {
                for (int i = 0; i <= factors[pos].NumTimes; i++)
                {
                    yield return (int)Math.Pow(factors[pos].Factor, i);
                }
            }
            else
            {
                foreach (var combo in GetFactorCombos(factors, pos + 1))
                {
                    for (int i = 0; i <= factors[pos].NumTimes; i++)
                    {
                        yield return (int)Math.Pow(factors[pos].Factor, i) * combo;
                    }
                }
            }
        }       
    }
}
