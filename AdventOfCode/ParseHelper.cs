using System.Security.Cryptography;

namespace AdventOfCode
{
    public static class ParseHelper
    {
        public static string[] SplitLines(this string input)
        {
            return input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string[] SplitParagraphs(this string input)
        {
            return input.TrimEnd().Split(new string[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
        }

        public static string[] SplitWhitespace(this string input)
        {
            return input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
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

        public static IEnumerable<float> ToFloats(this string input, char delimeter)
        {
            return input.Split(delimeter).ToFloats();
        }

        public static IEnumerable<float> ToFloats(this string input, string delimeter)
        {
            return input.Split(delimeter).ToFloats();
        }

        public static IEnumerable<(string, string)> ParseTuples(this string input, char listDelimeter, char tupleDelimeter)
        {
            foreach (string tuple in input.Split(listDelimeter))
            {
                string[] vals = tuple.Split(tupleDelimeter);

                yield return (vals[0], vals[1]);
            }
        }

        public static bool StartsWith(this string str, string other, int startPosition)
        {
            for (int i = 0; i < other.Length; i++)
            {
                if (str[startPosition + i] != other[i])
                    return false;
            }

            return true;
        }

        public static IEnumerable<string> SplitTopLevel(string input, char splitChar, char openParen, char closeParen)
        {
            int nestLevel = 0;
            int lastPos = 0;

            for (int pos = 0; pos < input.Length; pos++)
            {
                if (input[pos] == openParen)
                {
                    nestLevel++;
                }
                else if (input[pos] == closeParen)
                {
                    nestLevel--;
                }
                else
                {
                    if (nestLevel == 0)
                    {
                        if (input[pos] == splitChar)
                        {
                            yield return input.Substring(lastPos, pos - lastPos);

                            lastPos = pos + 1;
                        }
                    }
                }                
            }

            if (lastPos < input.Length)
            {
                yield return input.Substring(lastPos, input.Length - lastPos);
            }
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
