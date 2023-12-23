namespace AdventOfCode._2023
{
    internal class Day12 : Day
    {
        Dictionary<string, long> matchDict = new();

        long hits = 0;
        long misses = 0;

        long GetNumMatches(Span<char> condition, Span<int> groups)
        {
            if (condition.Length == 0)
                return (groups.Length == 0) ? 1 : 0;

            if (condition[0] == '.')
                return GetNumMatches(condition.Slice(1), groups);

            if (condition[0] == '#')
            {
                if (groups.Length == 0)
                    return 0;

                int pos = 0;

                for (; pos < groups[0]; pos++)
                {
                    if (pos == condition.Length)
                        return 0;

                    if (condition[pos] == '.')
                        return 0;
                }

                if (pos == condition.Length)
                    return (groups.Length == 1) ? 1 : 0;

                if (condition[pos] == '#')
                    return 0;

                return GetNumMatches(condition.Slice(pos + 1), groups.Slice(1));
            }

            string key = new string(condition);
            foreach (int group in groups)
                key += "-" + group;

            if (matchDict.ContainsKey(key))
            {
                hits++;

                return matchDict[key];
            }

            misses++;

            long matches = 0;

            char[] tmp = condition.ToArray();
            tmp[0] = '.';

            matches += GetNumMatches(tmp, groups);

            tmp[0] = '#';

            matches += GetNumMatches(tmp, groups);

            matchDict[key] = matches;

            return matches;
        }

        public override long Compute()
        {
            long numMatch = 0;

            foreach (var line in File.ReadLines(DataFile))
            {
                string[] split = line.Split(' ');

                char[] condition = split[0].ToCharArray();
                int[] groups = split[1].ToInts(',').ToArray();

                numMatch += GetNumMatches(condition, groups);
            }

            return numMatch;
        }

        public override long Compute2()
        {
            long numMatch = 0;

            foreach (var line in File.ReadLines(DataFile))
            {
                string[] split = line.Split(' ');

                char[] condition = split[0].ToCharArray();
                int[] groups = split[1].ToInts(',').ToArray();

                var expandedCondtion = condition;
                var expandedGroups = groups;

                for (int i = 0; i < 4; i++)
                {
                    expandedCondtion = expandedCondtion.Append('?').Concat(condition).ToArray();
                    expandedGroups = expandedGroups.Concat(groups).ToArray();
                }

                numMatch += GetNumMatches(expandedCondtion, expandedGroups);
            }

            return numMatch;
        }
    }
}
