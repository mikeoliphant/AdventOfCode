namespace AdventOfCode._2016
{
    internal class Day10
    {
        class Bot
        {
            public string[] LowHigh { get; set; } = null;
            public int[] Chips { get; set; } = new int[] { -1, -1 };

            public void AddChip(int val)
            {
                if (Chips[0] == -1)
                    Chips[0] = val;
                else
                    Chips[1] = val;
            }
        }

        Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
        Dictionary<int, int> output = new Dictionary<int, int>();

        Bot GetBot(int bot)
        {
            if (!bots.ContainsKey(bot))
                bots[bot] = new Bot();

            return bots[bot];
        }

        void AddChip(string dest, int val)
        {
            string[] split = dest.Split(' ');

            if (split[0] == "output")
            {
                output[int.Parse(split[1])] = val;
            }
            else
            {
                Bot bot = GetBot(int.Parse(split[1]));
                bot.AddChip(val);

                CheckBot(bot);
            }
        }

        void CheckBot(Bot bot)
        {
            if ((bot.LowHigh != null) && (bot.Chips[1] != -1))
            {
                AddChip(bot.LowHigh[0], bot.Chips.Min());
                AddChip(bot.LowHigh[1], bot.Chips.Max());
            }
        }

        public long Compute()
        {
            foreach (string cmd in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day10.txt"))
            {
                var match = Regex.Match(cmd, "bot (.*) gives low to (.*) and high to (.*)");

                if (match.Success)
                {
                    Bot bot = GetBot(int.Parse(match.Groups[1].Value));

                    bot.LowHigh = new string[] { match.Groups[2].Value, match.Groups[3].Value };

                    CheckBot(bot);
                }
                else
                {
                    match = Regex.Match(cmd, "value (.*) goes to bot (.*)");

                    if (match.Success)
                    {
                        Bot bot = GetBot(int.Parse(match.Groups[2].Value));

                        int val = int.Parse(match.Groups[1].Value);

                        bot.AddChip(val);

                        CheckBot(bot);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            //return bots.Where(b => b.Value.Chips.Contains(61) && b.Value.Chips.Contains(17)).First().Key;

            return output[0] * output[1] * output[2];
        }
    }
}
