namespace AdventOfCode._2023
{
    internal class Day9 : Day
    {
        long GetNextValue(IEnumerable<long> sequence)
        {
            if (sequence.Any(v => v != 0))
            {
                long? last = null;

                List<long> diffs = new();

                foreach (long value in sequence)
                {
                    if (last.HasValue)
                    {
                        diffs.Add(value - last.Value);

                        last = value;
                    }
                    else
                    {
                        last = value;
                    }
                }

                return GetNextValue(diffs) + last.Value;
            }

            return 0;
        }

        public override long Compute()
        {
            long sum = 0;

            foreach (string sequence in File.ReadLines(DataFile))
            {
                sum += GetNextValue(sequence.ToLongs(' '));
            }

            return sum;
        }

        long GetLastValue(IEnumerable<long> sequence)
        {
            if (sequence.Any(v => v != 0))
            {
                long? last = null;

                List<long> diffs = new();

                foreach (long value in sequence)
                {
                    if (last.HasValue)
                    {
                        diffs.Add(value - last.Value);

                        last = value;
                    }
                    else
                    {
                        last = value;
                    }
                }

                return sequence.First() - GetLastValue(diffs);
            }

            return 0;
        }
        public override long Compute2()
        {
            long sum = 0;

            foreach (string sequence in File.ReadLines(DataFile))
            {
                sum += GetLastValue(sequence.ToLongs(' '));
            }

            return sum;
        }
    }
}
