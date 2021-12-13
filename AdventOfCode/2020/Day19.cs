using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day19
    {
        Dictionary<int, int[][]> rules = new Dictionary<int, int[][]>();
        Dictionary<int, char> terminalRules = new Dictionary<int, char>();

        Dictionary<int, int> ruleVisits = new Dictionary<int, int>();

        string[] messages;

        void ReadInput()
        {
            string[] data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day19Fixed.txt").SplitParagraphs();

            foreach (string rule in data[0].SplitLines())
            {
                string[] lr = rule.Split(": ");

                if (lr[1][0] == '"')
                {
                    terminalRules[int.Parse(lr[0])] = lr[1][1];
                }
                else
                {
                    rules[int.Parse(lr[0])] = (from intList in lr[1].Split(" | ") select intList.ToInts(' ').ToArray()).ToArray();
                }
            }

            messages = data[1].SplitLines();
        }

        List<string> GetPatternCombinations(int rule, int[] subset)
        {
            if (subset.Length == 1)
            {
                if (subset[0] == rule)
                    return new List<string> { "(" + rule.ToString() + ")" };

                return ComputeAllPatterns(subset[0]);
            }

            List<string> combinations = new List<string>();

            foreach (string s1 in ((subset[0] == rule) ? new List<string> { "(" + rule.ToString() + ")" } : ComputeAllPatterns(subset[0])))
            {
                foreach (string s2 in GetPatternCombinations(rule, subset.Skip(1).ToArray()))
                {
                    combinations.Add(s1 + s2);
                }
            }

            return combinations;
        }

        Dictionary<int, List<string>> allPatterns = new Dictionary<int, List<string>>();

        List<string> ComputeAllPatterns(int rule)
        {
            if (allPatterns.ContainsKey(rule))
                return allPatterns[rule];

            if (terminalRules.ContainsKey(rule))
            {
                allPatterns[rule] = new List<string> { terminalRules[rule].ToString() };

                return allPatterns[rule];
            }

            List<string> patterns = new List<string>();

            foreach (int[] subset in rules[rule])
            {
                patterns.AddRange(GetPatternCombinations(rule, subset));
            }

            allPatterns[rule] = patterns;

            return patterns;
        }


        bool Match(string message, string pattern)
        {
            if (message == pattern)
                return true;

            //Match match = Regex.Match(message, "^" + Regex.Replace(pattern, @"\(.*\)", "(.*)") + "$");

            //if (match.Success)
            //{
            //    return true;
            //}

            return false;
        }

        bool Match(string message, List<string> patterns)
        {
            foreach (string pattern in patterns)
            {
                if (Match(message, pattern))
                    return true;
            }

            return false;
        }

        public long Compute()
        {
            ReadInput();

            ComputeAllPatterns(0);

            int patternLength = allPatterns[42][0].Length;

            List<string> matches = new List<string>();

            foreach (string message in messages)
            {
                int num31 = 0;

                for (int pos = message.Length - patternLength; pos >=0; pos -= patternLength)
                {
                    if (Match(message.Substring(pos, patternLength), allPatterns[31]))
                    {
                        num31++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (num31 < 1)
                    continue;

                int num42 = 0;

                for (int pos = 0; pos < message.Length; pos += patternLength)
                {
                    if (Match(message.Substring(pos, patternLength), allPatterns[42]))
                    {
                        num42++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (((num42 + num31) == (message.Length / patternLength)) && (num42 > num31))
                {
                    matches.Add(message);
                }
            }

            return matches.Count;
        }
    }
}
