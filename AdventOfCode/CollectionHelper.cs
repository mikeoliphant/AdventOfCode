namespace AdventOfCode
{
    public static class CollectionHelper
    {
        public static int Count<T>(this IEnumerable<T> input, T value)
        {
            return input.Count(i => i.Equals(value));
        }

        public static IEnumerable<List<T>> Partition<T>(this IList<T> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }
    }

    public static class PermutationHelper<T>
    {
        public static IEnumerable<T[]> GetAllPermutations(IEnumerable<T> sequence)
        {
            T[] seqArray = sequence.ToArray();

            int length = seqArray.Length;

            int numPermutations = length * (length - 1);

            int[][] permutations = new int[numPermutations][];

            yield return seqArray.ToArray();

            int[] c = new int[length];

            for (int i = 0; i < length;)
            {
                if (c[i] < i)
                {
                    if ((i % 2) == 0)
                    {
                        T tmp = seqArray[0];
                        seqArray[0] = seqArray[i];
                        seqArray[i] = tmp;
                    }
                    else
                    {
                        T tmp = seqArray[c[i]];
                        seqArray[c[i]] = seqArray[i];
                        seqArray[i] = tmp;
                    }

                    yield return seqArray.ToArray();

                    c[i]++;
                    i = 0;
                }
                else
                {
                    c[i] = 0;
                    i++;
                }
            }
        }
    }
}
