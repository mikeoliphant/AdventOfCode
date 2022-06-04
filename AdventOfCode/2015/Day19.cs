namespace AdventOfCode._2015
{
    internal class Day19 : Day
    {
        List<(string From, string To)> rules = new List<(string From, string To)>();

        string medicine = null;

        void ReadInput()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            foreach (string rule in split[0].SplitLines())
            {
                string[] lr = rule.Split(" => ");

                rules.Add((lr[0], lr[1]));
            }

            medicine = split[1];
        }

        bool Match(string str, string rule, int startPos)
        {
            if ((str.Length - startPos) < rule.Length)
                return false;

            for (int pos = 0; pos < rule.Length; pos++)
            {
                if (str[startPos + pos] != rule[pos])
                    return false;
            }

            return true;
        }

        string Replace(string str, string from, string to, int startPos)
        {
            return str.Substring(0, startPos) + to + str.Substring(startPos + from.Length);
        }

        public override long Compute()
        {
            ReadInput();

            Dictionary<string, bool> rewrites = new Dictionary<string, bool>();

            for (int pos = 0; pos < medicine.Length; pos++)
            {
                foreach (var rule in rules)
                {
                    if (Match(medicine, rule.From, pos))
                        rewrites[Replace(medicine, rule.From, rule.To, pos)] = true;
                }
            }

            return rewrites.Count;
        }

        IEnumerable<KeyValuePair<string, float>> GetNeighbors(string state)
        {
            for (int pos = 0; pos < state.Length; pos++)
            {
                foreach (var rule in rules)
                {
                    if (Match(state, rule.From, pos))
                    {
                        string replace = Replace(state, rule.From, rule.To, pos);

                        yield return new KeyValuePair<string, float>(replace, 1);

                    }
                }
            }
        }

        IEnumerable<string> GetNeighbors2(string state)
        {
            for (int pos = state.Length - 1; pos >= 0; pos--)
            {
                foreach (var rule in rules)
                {
                    if (Match(state, rule.From, pos))
                    {
                        string replace = Replace(state, rule.From, rule.To, pos);

                        yield return replace;
                    }
                }
            }
        }

        public override long Compute2()
        {
            ReadInput();

            List<(string From, string To)> reversed = new List<(string From, string To)>();

            foreach (var rule in rules)
            {
                reversed.Add((rule.To, rule.From));
            }

            rules = reversed.OrderByDescending(r => r.From.Length).ToList();

            string startMolecule = "e";

            //DijkstraSearch<string> search = new DijkstraSearch<string>(GetNeighbors);
            DepthFirstSearch<string> search = new DepthFirstSearch<string>(GetNeighbors2, null);

            List<string> path;
            float cost;

            if (search.FindFirstPath(medicine, startMolecule, out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }
    }
}
