using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode._2024
{
    internal class Day19 : Day
    {
        Dictionary<char, List<char[]>> patterns;
        List<char[]> designs;

        void AddPattern(char[] pattern)
        {
            if (!patterns.ContainsKey(pattern[0]))
            {
                patterns[pattern[0]] = new();
            }

            patterns[pattern[0]].Add(pattern);
        }

        void ReadData()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            patterns = new();

            foreach (var pattern in split[0].Split(", "))
            {
                AddPattern(pattern.ToArray());   
            }

            designs = split[1].SplitLines().Select(p => p.ToCharArray()).ToList();
        }

        bool CanMake(ReadOnlySpan<char> design)
        {
            if (!patterns.ContainsKey(design[0]))
                return false;

            foreach (var pattern in patterns[design[0]])
            {
                if (design.Length < pattern.Length)
                    continue;

                if (design.Slice(0, pattern.Length).SequenceEqual(pattern))
                {
                    if (design.Length == pattern.Length)
                        return true;

                    if (CanMake(design.Slice(pattern.Length)))
                        return true;
                }
            }

            return false;
        }

        public override long Compute()
        {
            ReadData();

            long count = 0;

            foreach (var design in designs)
            {
                if (CanMake(design))
                    count++;
            }

            return count;
        }

        static Dictionary<string, long> countCache = new();
        static Dictionary<string, long>.AlternateLookup<ReadOnlySpan<char>> lookup = countCache.GetAlternateLookup<ReadOnlySpan<char>>();

        long MakeCount(ReadOnlySpan<char> design)
        {
            if (!patterns.ContainsKey(design[0]))
                return 0;

            if (lookup.ContainsKey(design))
                return lookup[design];

            long canMake = 0;

            foreach (var pattern in patterns[design[0]])
            {
                if (design.Length < pattern.Length)
                    continue;

                if (design.Length < pattern.Length)
                    continue;

                if (design.Slice(0, pattern.Length).SequenceEqual(pattern))
                {
                    if (design.Length == pattern.Length)
                        canMake++;
                    else
                        canMake += MakeCount(design.Slice(pattern.Length));
                }
            }

            countCache[new string(design)] = canMake;

            return canMake;
        }

        public override long Compute2()
        {
            ReadData();

            long count = 0;

            foreach (var design in designs)
            {
                count += MakeCount(design);
            }

            return count;
        }
    }
}
