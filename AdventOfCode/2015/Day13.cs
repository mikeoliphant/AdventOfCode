namespace AdventOfCode._2015
{
    internal class Day13 : Day
    {
        Dictionary<string, Dictionary<string, int>> happyDict = new Dictionary<string, Dictionary<string, int>>();

        long DeltaHappy(string p1, string p2)
        {
            if ((p1 == "me") || (p2 == "me"))
            {
                return 0;
            }

            return happyDict[p1][p2] + happyDict[p2][p1];
        }

        long DeltaHappy(string[] order)
        {
            long deltaHappy = 0;

            for (int i = 0; i < order.Length; i++)
            {
                deltaHappy += DeltaHappy(order[i], order[ModHelper.PosMod(i + 1, order.Length)]);
                deltaHappy += DeltaHappy(order[i], order[ModHelper.PosMod(i - 1, order.Length)]);
            }

            return deltaHappy;
        }

        void ReadInput()
        {
            foreach (string rule in File.ReadLines(DataFile))
            {
                var match = Regex.Match(rule, "(.*) would (.*) (.*) happiness units by sitting next to (.*).");

                if (match.Success)
                {
                    string p1 = match.Groups[1].Value;
                    string p2 = match.Groups[4].Value;

                    int happy = int.Parse(match.Groups[3].Value);

                    if (!happyDict.ContainsKey(p1))
                        happyDict[p1] = new Dictionary<string, int>();

                    happyDict[p1][p2] = (match.Groups[2].Value == "gain") ? happy : -happy;
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public override long Compute()
        {
            ReadInput();

            long maxDelta = long.MinValue;
            string[] maxPerm = null;

            foreach (var perm in PermutationHelper<string>.GetAllPermutations(happyDict.Keys))
            {
                long delta = DeltaHappy(perm);

                if (delta > maxDelta)
                {
                    maxDelta = delta;
                    maxPerm = perm;
                }
            }

            return maxDelta / 2;
        }

        public override long Compute2()
        {
            ReadInput();

            long maxDelta = long.MinValue;
            string[] maxPerm = null;

            List<string> people = happyDict.Keys.ToList();

            people.Add("me");

            foreach (var perm in PermutationHelper<string>.GetAllPermutations(people))
            {
                long delta = DeltaHappy(perm);

                if (delta > maxDelta)
                {
                    maxDelta = delta;
                    maxPerm = perm;
                }
            }

            return maxDelta / 2;
        }
    }
}
