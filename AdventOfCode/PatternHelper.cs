namespace AdventOfCode
{
    public static class PatternHelper
    {
        public static IEnumerable<char[]> EnumeratePatterns(char[] pattern, char wildcard, char[] wildcardValues)
        {
            foreach (var pat in EnumeratePatterns(pattern, wildcard, wildcardValues, 0))
                yield return pat;
        }

        static IEnumerable<char[]> EnumeratePatterns(char[] pattern, char wildcard, char[] wildcardValues, int pos)
        {
            if (pos == pattern.Length)
                yield return pattern;
            else
            {
                if (pattern[pos] != wildcard)
                {
                    foreach (var pat in EnumeratePatterns(pattern, wildcard, wildcardValues, pos + 1))
                        yield return pat;
                }
                else
                {
                    foreach (char wild in wildcardValues)
                    {
                        char[] newPat = new char[pattern.Length];
                        Array.Copy(pattern, newPat, pattern.Length);

                        newPat[pos] = wild;

                        foreach (var pat in EnumeratePatterns(newPat, wildcard, wildcardValues, pos + 1))
                            yield return pat;
                    }
                }
            }
        }
    }
}
