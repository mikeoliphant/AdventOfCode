namespace AdventOfCode._2020
{
    public class Day9
    {
        long[] numbers;

        void ReadData()
        {
            numbers = (from numStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day9.txt") select long.Parse(numStr)).ToArray();
        }

        public long Compute()
        {
            ReadData();

            List<long> history = new List<long>();

            int maxHist = 25;

            foreach (long value in numbers)
            {
                if (history.Count == maxHist)
                {
                    bool isValid = false;

                    for (int pos1 = 0; pos1 < maxHist; pos1++)
                    {
                        for (int pos2 = 0; pos2 < maxHist; pos2++)
                        {
                            if (pos1 != pos2)
                            {
                                if ((history[pos1] + history[pos2]) == value)
                                {
                                    isValid = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (!isValid)
                        return value;

                    history.RemoveAt(0);
                }

                history.Add(value);
            }

            throw new Exception();
        }

        public long Compute2()
        {
            long firstInvalid = Compute();

            for (int startPos = 0; startPos < (numbers.Length - 1); startPos++)
            {
                long sum = 0;

                for (int pos = startPos; pos < numbers.Length; pos++)
                {
                    sum += numbers[pos];

                    if (sum == firstInvalid)
                    {
                        long minValue = long.MaxValue;
                        long maxValue = long.MinValue;

                        for (int i = startPos; i <= pos; i++)
                        {
                            minValue = Math.Min(minValue, numbers[i]);
                            maxValue = Math.Max(maxValue, numbers[i]);
                        }

                        return minValue + maxValue;
                    }
                    else if (sum > firstInvalid)
                        continue;
                }
            }

            return 0;
        }
    }
}
