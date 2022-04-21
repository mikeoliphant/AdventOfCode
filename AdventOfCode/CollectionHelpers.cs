namespace AdventOfCode
{
    public static class CollectionHelpers
    {
        public static int Count<T>(this IEnumerable<T> input, T value)
        {
            return input.Count(i => i.Equals(value));
        }
    }
}
