
using Microsoft.VisualBasic.Logging;
using System.Runtime.CompilerServices;

namespace AdventOfCode._2025
{
    internal class Day2 : Day
    {
        List<(string Start, string End)> ranges = new();
        void ReadRanges()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                foreach (string range in line.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] minMax = range.Split('-');

                    ranges.Add((minMax[0], minMax[1]));
                }
            }
        }

        public override long Compute()
        {
            ReadRanges();

            long sum = 0;

            foreach (var range in ranges)
            {
                Console.WriteLine(range.Start + "-" + range.End);

                long startVal = long.Parse(range.Start);
                long endVal = long.Parse(range.End);

                string currentStr = range.Start;

                if ((currentStr.Length % 2) != 0)
                    currentStr = "1" + new string('0', currentStr.Length);

                while (true)
                {
                    long currentVal = long.Parse(currentStr);

                    if (currentVal > endVal)
                        break;

                    int halfLen = currentStr.Length / 2;

                    int halfPow = (int)Math.Pow(10, halfLen);

                    string firstHalfStr = currentStr.Substring(0, halfLen);

                    long firstHalf = long.Parse(firstHalfStr);

                    for (long f = firstHalf; f < halfPow; f++)
                    {
                        long val = (f * halfPow) + f;

                        if (val < startVal)
                            continue;

                        if (val > endVal)
                            break;

                        Console.WriteLine(val);

                        sum += val;
                    }

                    currentStr = "1" + new string('0', currentStr.Length + 2);
                }
            }

            return sum;
        }

        bool IsInvalid(string id)
        {
            int half = id.Length / 2;

            for (int repeatLength = 1; repeatLength <= half; repeatLength++)
                if (IsInvalid(id, repeatLength))
                    return true;

            return false;
        }

        bool IsInvalid(string id, int repeatLength)
        {
            if ((id.Length % repeatLength) != 0)
                return false;

            int repeatSize = id.Length / repeatLength;

            for (int pos = 0; pos < repeatLength; pos++)
            {
                for (int repeat = 1; repeat < repeatSize; repeat++)
                {
                    if (id[pos] != id[(repeat * repeatLength) + pos])
                        return false;
                }
            }

            return true;
        }

        public override long Compute2()
        {
            ReadRanges();

            long sum = 0;

            foreach (var range in ranges)
            {
                Console.WriteLine(range.Start + "-" + range.End);

                long startVal = long.Parse(range.Start);
                long endVal = long.Parse(range.End);

                for (long val = startVal; val <= endVal; val++)
                {
                    if (IsInvalid(val.ToString()))
                    {
                        Console.WriteLine(val);

                        sum += val;
                    }
                }
            }

            return sum;
        }
    }
}
