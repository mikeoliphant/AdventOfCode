using System.Collections.Generic;

namespace AdventOfCode._2024
{
    internal class Day2 : Day
    {
        bool IsSafe(IEnumerable<int> ints)
        {
            var deltas = ints.Zip(ints.Skip(1), (current, next) => next - current);

            int sign = 0;

            foreach (int delta in deltas)
            {
                if ((Math.Sign(delta) * sign) == -1)
                    return false;

                if (Math.Abs(delta) > 3)
                    return false;

                if (delta == 0)
                    return false;

                sign = Math.Sign(delta);
            }

            return true;
        }

        public override long Compute()
        {
            long numSafe = 0;

            foreach (string line in File.ReadLines(DataFileTest))
            {
                var ints = line.ToInts(' ');

                if (IsSafe(ints))
                    numSafe++;
            }

            return numSafe;
        }

        public override long Compute2()
        {
            long numSafe = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                var ints = line.ToInts(' ');

                bool isSafe = false;

                if (IsSafe(ints))
                {
                    isSafe = true;
                }
                else
                {
                    int len = ints.Count();

                    for (int pos = 0; pos < len; pos++)
                    {
                        List<int> newInts = ints.ToList();

                        newInts.RemoveAt(pos);

                        if (IsSafe(newInts))
                        {
                            isSafe = true;

                            break;
                        }
                    }
                }

                if (isSafe)
                    numSafe++;
            }

            return numSafe;
        }
    }
}
