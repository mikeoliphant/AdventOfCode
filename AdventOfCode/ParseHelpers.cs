namespace AdventOfCode
{
    public static class ParseHelpers
    {
        public static string[] SplitLines(this string input)
        {
            return input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string[] SplitParagraphs(this string input)
        {
            return input.TrimEnd().Split(new string[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
        }

        public static IEnumerable<int> ToInts(this string input, char delimeter)
        {
            return from intStr in input.Split(delimeter) select int.Parse(intStr);
        }

        public static IEnumerable<int> ToInts(this IEnumerable<string> input)
        {
            return from intStr in input select int.Parse(intStr);
        }

        public static IEnumerable<int> ToInts(this string input, string delimeter)
        {
            return input.Split(delimeter).ToInts();
        }

        public static IEnumerable<long> ToLongs(this string input, char delimeter)
        {
            return from intStr in input.Split(delimeter) select long.Parse(intStr);
        }

        public static IEnumerable<long> ToLongs(this IEnumerable<string> input)
        {
            return from intStr in input select long.Parse(intStr);
        }

        public static IEnumerable<long> ToLongs(this string input, string delimeter)
        {
            return input.Split(delimeter).ToLongs();
        }

        public static IEnumerable<float> ToFloats(this IEnumerable<string> input)
        {
            return from floatStr in input select float.Parse(floatStr);
        }

        public static IEnumerable<float> ToFloats(this string input, string delimeter)
        {
            return input.Split(delimeter).ToFloats();
        }

        public static IEnumerable<IEnumerable<int>> ReadZeroToNineGrid(string input)
        {
            return from line in input.TrimEnd().SplitLines() select (from c in line.ToCharArray() select (int)(c - '0'));
        }

        public static Point ToPoint(this string pointStr)
        {
            string[] split = pointStr.Split(',');

            return new Point(int.Parse(split[0]), int.Parse(split[1]));
        }
    }
}
