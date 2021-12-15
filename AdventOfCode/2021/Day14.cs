using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class Day14
    {
        string polymer;
        Dictionary<string, char> rules = new Dictionary<string, char>();

        void ReadInput()
        {
            string[] data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day14.txt").SplitParagraphs();

            polymer = data[0];

            foreach (string rule in data[1].SplitLines())
            {
                string[] rl = rule.Split(" -> ");

                rules[rl[0]] = rl[1][0];
            }
        }

        void Cycle()
        {
            for (int pos = 0; pos < (polymer.Length - 1); pos++)
            {
                char newC = rules[polymer.Substring(pos, 2)];
            }
        }

        public long Compute()
        {
            ReadInput();

            for (int i = 0; i < 10; i++)
                Cycle();

            Dictionary<char, long> charHistogram = new Dictionary<char, long>();

            foreach (char c in polymer)
            {
                if (!charHistogram.ContainsKey(c))
                    charHistogram[c] = 1;
                else
                    charHistogram[c]++;
            }

            long max = charHistogram.Values.Max();
            long min = charHistogram.Values.Min();

            return max - min;
        }

        Dictionary<string, long> pairCounts = new Dictionary<string, long>();

        void Cycle2()
        {
            Dictionary<string, long> newCounts = new Dictionary<string, long>();

            foreach (string key in rules.Keys)
            {
                newCounts[key] = 0;
            }

            foreach (var pairCount in pairCounts)
            {
                char newC = rules[pairCount.Key];

                long count = pairCount.Value;

                newCounts[new string(new char[] { pairCount.Key[0], newC })] += count;
                newCounts[new string(new char[] { newC, pairCount.Key[1] })] += count;
            }

            pairCounts = newCounts;
        }

        public long Compute2()
        {
            ReadInput();

            foreach (string key in rules.Keys)
            {
                pairCounts[key] = 0;
            }

            for (int pos = 0; pos < (polymer.Length - 1); pos++)
            {
                string pair = polymer.Substring(pos, 2);

                pairCounts[pair]++;
            }

            for (int i = 0; i < 40; i++)
                Cycle2();

            Dictionary<char, long> charHistogram = new Dictionary<char, long>();

            foreach (var pairCount in pairCounts)
            {
                if (!charHistogram.ContainsKey(pairCount.Key[0]))
                    charHistogram[pairCount.Key[0]] = pairCount.Value;
                else
                    charHistogram[pairCount.Key[0]] += pairCount.Value;

                if (!charHistogram.ContainsKey(pairCount.Key[1]))
                    charHistogram[pairCount.Key[1]] = pairCount.Value;
                else
                    charHistogram[pairCount.Key[1]] += pairCount.Value;
            }

            var max = charHistogram.Values.Max();
            var min = charHistogram.Values.Min();

            long result = (long)(Math.Ceiling((double)max / 2.0) - Math.Ceiling((double)min / 2.0));

            return result;
        }
    }
}
