using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode._2024
{
    internal class Day22 : Day
    {
        long IterateSecret(long value)
        {
            value = (value ^ (value * 64)) % 16777216;
            value = (value ^ (value / 32)) % 16777216;
            value = (value ^ (value * 2048)) % 16777216;

            return value;
        }

        long GetSecrect(long value, long count)
        {
            for (int i = 0; i < count; i++)
            {
                value = IterateSecret(value);
            }

            return value;
        }

        public override long Compute()
        {
            // long value = GetSecrect(1, 2000);

            long sum = 0;

            foreach (long secret in File.ReadLines(DataFile).ToLongs())
            {
                sum += GetSecrect(secret, 2000);
            }

            return sum;
        }

        List<List<int>> prices = new();
        List<List<int>> deltas = new();

        (List<int>, List<int>) GetDeltas(long value, long count)
        {
            List<int> delta = new();
            List<int> price = new();

            int? lastDigit = null;

            for (int i = 0; i < count; i++)
            {
                int digit = (int)(value % 10);

                price.Add(digit);

                if (lastDigit != null)
                {
                    delta.Add(digit - lastDigit.Value);
                }

                lastDigit = digit;

                value = IterateSecret(value);
            }

            return (price, delta);
        }

        IEnumerable<IEnumerable<int>> GetAllSequences()
        {
            for (int i1 = -9; i1 < 10; i1++)
            {
                for (int i2 = -9; i2 < 10; i2++)
                {
                    for (int i3 = -9; i3 < 10; i3++)
                    {
                        for (int i4 = -9; i4 < 10; i4++)
                        {
                            yield return (new int[] { i1, i2, i3, i4 });
                        }
                    }
                }
            }
        }

        IEnumerable<int[]> FindAllSequences()
        {
            Dictionary<(int, int, int, int), int> sequences = new();

            foreach (var delta in deltas)
            {
                for (int i = 0; i < delta.Count - 3; i++)
                {
                    var key = (delta[i], delta[i + 1], delta[i + 2], delta[i + 3]);

                    if (!sequences.ContainsKey(key))
                    {
                        sequences.Add(key, 1);
                    }
                    else
                    {
                        sequences[key]++;
                    }
                }
            }

            return sequences.Where(s => s.Value > 200).Select(s => new int[] { s.Key.Item1, s.Key.Item2, s.Key.Item3, s.Key.Item4 });
        }

        long GetPrice(int[] sequence)
        {
            int totPrice = 0;

            for (int d = 0; d < deltas.Count; d++)
            {
                var delta = deltas[d];

                int price = 0;

                for (int i = 0; i < delta.Count - 3; i++)
                {
                    if (delta.Slice(i, 4).SequenceEqual(sequence))
                    {
                        price = prices[d][i + 4];

                        break;
                    }
                }

                totPrice += price;
            }

            return totPrice;
        }

        public override long Compute2()
        {
            //var blah = GetDeltas(123, 2000);

            foreach (long secret in File.ReadLines(DataFile).ToLongs())
            {
                var (price, delta) = GetDeltas(secret, 2000);

                deltas.Add(delta);
                prices.Add(price);
            }

            //long test = GetPrice(new int[] { -2, 1, -1, 3 });

            var sequences = FindAllSequences().ToList();

            long maxPrice = 0;

            int seq = 0;

            foreach (var sequence in sequences)
            {
                seq++;

                long price = GetPrice(sequence);

                if (price > maxPrice)
                {
                    maxPrice = price;
                }
            }

            return maxPrice;
        }
    }
}
