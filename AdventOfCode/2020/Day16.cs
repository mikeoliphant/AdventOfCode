namespace AdventOfCode._2020
{
    public class Day16
    {
        Dictionary<string, KeyValuePair<int, int>[]> rules = new Dictionary<string, KeyValuePair<int, int>[]>();
        int[] myTicket;
        int[][] otherTickets;

        void ReadInput()
        {
            string[] sections = ParseHelper.SplitParagraphs(File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day16.txt"));

            foreach (string ruleStr in ParseHelper.SplitLines(sections[0]))
            {
                string[] lr = ruleStr.Split(": ");

                string[] ranges = lr[1].Split(" or ");

                var rule = new KeyValuePair<int, int>[2];

                for (int i = 0; i < 2; i++)
                {
                    string[] vals = ranges[i].Split('-');

                    rule[i] = new KeyValuePair<int, int>(int.Parse(vals[0]), int.Parse(vals[1]));
                }

                rules[lr[0]] = rule;
            }

            myTicket = sections[1].SplitLines()[1].ToInts(',').ToArray();

            otherTickets = (from ticketStr in sections[2].SplitLines().Skip(1) select ticketStr.ToInts(',').ToArray()).ToArray();
        }

        bool isValid(int[] ticket)
        {
            foreach (int value in ticket)
            {
                if (!isValid(value))
                    return false;
            }

            return true;
        }

        bool isValid(int value)
        {
            foreach (var rule in rules.Values)
            {
                if (MatchesRule(rule, value))
                    return true;
            }

            return false;
        }

        bool MatchesRule(KeyValuePair<int, int>[] rule, int value)
        {
            foreach (var range in rule)
            {
                if ((value >= range.Key) && (value <= range.Value))
                {
                    return true;
                }
            }

            return false;
        }


        public long Compute()
        {
            ReadInput();

            long sum = 0;

            foreach (int[] ticket in otherTickets)
            {
                foreach (int value in ticket)
                {
                    if (!isValid(value))
                        sum += value;
                }
            }

            return sum;
        }

        public long Compute2()
        {
            ReadInput();

            int ticketLength = myTicket.Length;

            int[][] validTickets = (from ticket in otherTickets where isValid(ticket) select ticket).ToArray();

            Dictionary<string, List<int>> matchingPositions = new Dictionary<string, List<int>>();

            foreach (var rule in rules)
            {
                matchingPositions[rule.Key] = new List<int>();

                for (int pos = 0; pos < ticketLength; pos++)
                {
                    bool ruleMatchesPos = true;

                    foreach (int[] ticket in validTickets)
                    {
                        if (!MatchesRule(rule.Value, ticket[pos]))
                        {
                            ruleMatchesPos = false;

                            break;
                        }
                    }

                    if (ruleMatchesPos)
                    {
                        matchingPositions[rule.Key].Add(pos);
                    }
                }
            }

            Dictionary<int, string> positionRules = new Dictionary<int, string>();

            do
            {
                string toRemove = null;

                foreach (string rule in matchingPositions.Keys)
                {
                    if (matchingPositions[rule].Count == 1)
                    {
                        toRemove = rule;

                        break;
                    }
                }

                int position = matchingPositions[toRemove][0];
                matchingPositions.Remove(toRemove);

                positionRules[position] = toRemove;

                foreach (string rule in matchingPositions.Keys)
                {
                    matchingPositions[rule].Remove(position);
                }
            }
            while (positionRules.Count < ticketLength);

            long mult = 1;

            foreach (var map in positionRules)
            {
                if (map.Value.StartsWith("departure"))
                {
                    mult *= myTicket[map.Key];
                }
            }

            return mult;
        }
    }
}
