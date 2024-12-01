namespace AdventOfCode._2023
{
    internal class Day19 : Day
    {
        Dictionary<string, Rule> rules = new();
        List<Dictionary<char, int>> parts = new();

        class Rule
        {
            public string Name;
            public List<RuleCondition> Conditions = new();
            public string DefaultRule;

            public string GetNextRule(Dictionary<char, int> part)
            {
                foreach (RuleCondition condition in Conditions)
                {
                    if (condition.MatchPart(part))
                        return condition.NextRule;
                }

                return DefaultRule;
            }

            public IEnumerable<(string Rule, Dictionary<char, Range> Range)> GetNextRules(Dictionary<char, Range> partRange)
            {
                foreach (RuleCondition condition in Conditions)
                {
                    var range = condition.GetMatchingPartRange(partRange);

                    if (range != null)
                    {
                        var newDict = partRange.ToDictionary();

                        newDict[condition.Category] = range.Value;

                        var existingRange = partRange[condition.Category];

                        partRange[condition.Category] = existingRange.Subtract(range.Value);

                        if (!range.Value.IsEmpty())
                           yield return (condition.NextRule, newDict);

                        // Don't need to continue if we have exhausted a range
                        if (partRange[condition.Category].IsEmpty())
                            break;
                    }
                }

                yield return (DefaultRule, partRange);
            }
        }

        class RuleCondition
        {
            public char Category;
            public char Op;
            public int CompareVal;
            public string NextRule;

            public override string ToString()
            {
                return Category.ToString() + Op.ToString() + CompareVal + ":" + NextRule;
            }

            public bool MatchPart(Dictionary<char, int> part)
            {
                if (!part.ContainsKey(Category))
                    return false;

                if (Op == '>')
                {
                    return part[Category] > CompareVal;
                }

                return part[Category] < CompareVal;
            }

            public Range? GetMatchingPartRange(Dictionary<char, Range> partRanges)
            {
                if (!partRanges.ContainsKey(Category))
                    return null;

                var range = partRanges[Category];

                if (Op == '>')
                {
                    return range.Above(CompareVal);
                }

                return range.Below(CompareVal);
            }
        }

        void ReadInput()
        {
            string[] sections = File.ReadAllText(DataFile).SplitParagraphs();

            foreach (string line in sections[0].SplitLines())
            {
                Rule rule = new Rule();

                var match = Regex.Match(line, @"^(.*)\{(.*)\}$");

                rule.Name = match.Groups[1].Value;

                string[] conditions = match.Groups[2].Value.Split(',');

                for (int i = 0; i < (conditions.Length - 1); i++)
                {
                    string[] condVals = conditions[i].Split(':');

                    RuleCondition condition = new RuleCondition()
                    {
                        Category = condVals[0][0],
                        Op = condVals[0][1],
                        CompareVal = int.Parse(condVals[0].Substring(2)),
                        NextRule = condVals[1]
                    };

                    rule.Conditions.Add(condition);
                }

                rule.DefaultRule = conditions[conditions.Length - 1];

                rules[rule.Name] = rule;
            }

            foreach (string line in sections[1].SplitLines())
            {
                var match = Regex.Match(line, @"{(.*)\}");

                Dictionary<char, int> part = new Dictionary<char, int>();

                foreach (var tuple in match.Groups[1].Value.ParseTuples(',', '='))
                {
                    part[tuple.Item1[0]] = int.Parse(tuple.Item2);
                }

                parts.Add(part);
            }
        }

        public bool IsAccepted(Dictionary<char, int> part, string rule)
        {
            string nextRule = rules[rule].GetNextRule(part);

            if (nextRule == "R")
                return false;
            else if (nextRule == "A")
                return true;

            return IsAccepted(part, nextRule);
        }

        public long NumAccepted(Dictionary<char, Range> partRanges, string rule)
        {
            long accepted = 0;

            foreach (var ranges in rules[rule].GetNextRules(partRanges))
            {
                if (ranges.Rule == "A")
                {
                    long mult = 1;

                    foreach (Range range in ranges.Range.Values)
                    {
                        mult *= range.Size();
                    }

                    accepted += mult;
                }
                else if (ranges.Rule != "R")
                {
                    accepted += NumAccepted(ranges.Range, ranges.Rule);
                }
            }

            return accepted;
        }


        public override long Compute()
        {
            ReadInput();

            long total = 0;

            foreach (var part in parts)
            {
                if (IsAccepted(part, "in"))
                {
                    total += part.Values.Sum();
                }
            }

            return total;
        }

        char[] categories = { 'x', 'm', 'a', 's' };

        public override long Compute2()
        {
            ReadInput();

            Dictionary<char, Range> partRanges = new();

            foreach (char c in categories)
            {
                partRanges[c] = new Range(1, 4000);
            }

            long accepted = NumAccepted(partRanges, "in");

            return accepted;
        }
    }
}
