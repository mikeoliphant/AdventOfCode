namespace AdventOfCode._2023
{
    internal class Day1 : Day
    {
        string[] numStrs = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        public override long Compute()
        {
            long sum = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                Match match = Regex.Match(line, @"^[^\d]*(\d)");
                Match match2 = Regex.Match(line, @".*(\d)[^\d]*$");

                sum += int.Parse(match.Groups[1].Value) * 10;
                sum += int.Parse(match2.Groups[1].Value);
            }

            return sum;
        }

        (int, int) GetNums(Span<char> str)
        {
            int first = 0;
            int last = 0;

            for (int i = 0; i < str.Length; i++)
            {
                for (int pos = 0; pos < numStrs.Length; pos++)
                {
                    if (str.Slice(i).StartsWith(numStrs[pos]))
                    {
                        if (first == 0)
                            first = (pos % 9) + 1;
                        else
                            last = (pos % 9) + 1;
                    }
                }
            }

            return (last == 0) ? (first, first) : (first, last);
        }

        public override long Compute2()
        {
            long sum = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                char[] chars = line.ToCharArray();

                var nums = GetNums(chars);

                sum += (nums.Item1 * 10) + nums.Item2;

                Console.WriteLine(line + " => " + nums.Item1.ToString() + nums.Item2.ToString());
            }

            return sum;
        }
    }
}
