namespace AdventOfCode
{
    public static class FactorHelper
    {
        public static IEnumerable<(long, long)> PrimeFactors(long n)
        {
            long num2 = 0;

            while (n % 2 == 0)
            {
                num2++;

                n /= 2;
            }

            if (num2 > 0)
                yield return (2, num2);

            for (long i = 3; i <= Math.Sqrt(n); i += 2)
            {
                long numI = 0;

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

        public static IEnumerable<long> AllFactors(long n)
        {
            var factors = PrimeFactors(n).ToList();

            if (factors.Count == 0)
                return new List<long> { n };
            else
            {
                return GetFactorCombos(factors, 0);
            }
        }

        static IEnumerable<long> GetFactorCombos(List<(long Factor, long NumTimes)> factors, int pos)
        {
            if (pos == (factors.Count - 1))
            {
                for (int i = 0; i <= factors[pos].NumTimes; i++)
                {
                    yield return (long)Math.Pow(factors[pos].Factor, i);
                }
            }
            else
            {
                foreach (var combo in GetFactorCombos(factors, pos + 1))
                {
                    for (int i = 0; i <= factors[pos].NumTimes; i++)
                    {
                        yield return (long)Math.Pow(factors[pos].Factor, i) * combo;
                    }
                }
            }
        }

        public static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;

                b = a % b;
                a = temp;
            }

            return a;
        }

        public static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonFactor(a, b)) * b;
        }
    }
}
