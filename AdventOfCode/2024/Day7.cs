namespace AdventOfCode._2024
{
    internal class Day7 : Day
    {
        List<(long Value, List<long> Operands)> equations = new();

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] split = line.Split(": ");

                equations.Add((long.Parse(split[0]), split[1].ToLongs(' ').ToList()));
            }
        }

        IEnumerable<long> DoOps(IEnumerable<long> operands)
        {
            if (operands.Count() == 1)
            {
                yield return operands.First();
            }
            else
            {
                foreach (long recurse in DoOps(operands.SkipLast(1)))
                {
                    yield return recurse * operands.Last();
                    yield return recurse + operands.Last();
                }
            }
        }

        public override long Compute()
        {
            ReadData();

            long count = 0;

            foreach (var equation in equations)
            {
                if (DoOps(equation.Operands).Contains(equation.Value))
                    count += equation.Value;
            }

            return count;
        }

        int GetDigits(long num)
        {
            long val = num / 10;

            if (val == 0)
                return 1;

            return GetDigits(val) + 1;
        }

        long AppendNumbers(long num1, long num2)
        {
            return num2 + (num1 * (long)Math.Pow(10, GetDigits(num2)));
        }

        IEnumerable<long> DoOps2(IEnumerable<long> operands)
        {
            if (operands.Count() == 1)
            {
                yield return operands.First();
            }
            else
            {
                foreach (long recurse in DoOps2(operands.SkipLast(1)))
                {
                    yield return recurse * operands.Last();
                    yield return recurse + operands.Last();
                    yield return AppendNumbers(recurse, operands.Last());
                }
            }
        }

        public override long Compute2()
        {
            ReadData();

            long count = 0;

            foreach (var equation in equations)
            {
                if (DoOps2(equation.Operands).Contains(equation.Value))
                    count += equation.Value;
            }

            return count;
        }
    }
}
