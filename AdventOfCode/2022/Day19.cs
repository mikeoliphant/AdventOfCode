using Microsoft.VisualBasic.Logging;

namespace AdventOfCode._2022
{
    internal class Day19 : Day
    {
        List<Blueprint> blueprints = new List<Blueprint>();
        Dictionary<State, int> cache = new Dictionary<State, int>();

        record struct Blueprint(int OrePerOreBot, int OrePerClayBot, int OrePerObsidianBot, int ClayPerObsidiantBot, int OrePerGeodeBot, int ObsidianPerGeodeBot)
        {
            public IEnumerable<State> GetTransitions(State state)
            {
                bool canBuild = false;

                // Build geode bot
                if ((state.Ore >= OrePerGeodeBot) && (state.Obsidian >= ObsidianPerGeodeBot))
                {
                    canBuild = true;

                    yield return new State(state.Ore - OrePerGeodeBot + state.OreBot, state.Clay + state.ClayBot, state.Obsidian - ObsidianPerGeodeBot + state.ObsidianBot, state.Geode + state.GeodeBot, state.OreBot, state.ClayBot, state.ObsidianBot, state.GeodeBot + 1, state.Minutes - 1);
                }
                else
                {
                    // Build obsidian bot
                    if ((state.Ore >= OrePerObsidianBot) && (state.Clay >= ClayPerObsidiantBot) && (state.ObsidianBot < ObsidianPerGeodeBot))
                    {
                        canBuild = true;

                        yield return new State(state.Ore - OrePerObsidianBot + state.OreBot, state.Clay - ClayPerObsidiantBot + state.ClayBot, state.Obsidian + state.ObsidianBot, state.Geode + state.GeodeBot, state.OreBot, state.ClayBot, state.ObsidianBot + 1, state.GeodeBot, state.Minutes - 1);
                    }
                    //else
                    //{
                        // Build clay bot
                        if ((state.Ore >= OrePerClayBot) && (state.ClayBot < ClayPerObsidiantBot))
                        {
                            canBuild = true;

                            yield return new State(state.Ore - OrePerClayBot + state.OreBot, state.Clay + state.ClayBot, state.Obsidian + state.ObsidianBot, state.Geode + state.GeodeBot, state.OreBot, state.ClayBot + 1, state.ObsidianBot, state.GeodeBot, state.Minutes - 1);
                        }

                        // Build ore bot
                        if ((state.Ore >= OrePerOreBot) && (state.OreBot < 4))
                        {
                            canBuild = true;

                            yield return new State(state.Ore - OrePerOreBot + state.OreBot, state.Clay + state.ClayBot, state.Obsidian + state.ObsidianBot, state.Geode + state.GeodeBot, state.OreBot + 1, state.ClayBot, state.ObsidianBot, state.GeodeBot, state.Minutes - 1);
                        }
                    //}
                }

                // Don't build anything
                if (!canBuild || (state.Ore <= 4))
                    yield return new State(state.Ore + state.OreBot, state.Clay + state.ClayBot, state.Obsidian + state.ObsidianBot, state.Geode + state.GeodeBot, state.OreBot, state.ClayBot, state.ObsidianBot, state.GeodeBot, state.Minutes - 1);
            }
        }

        record struct State(int Ore, int Clay, int Obsidian, int Geode, int OreBot, int ClayBot, int ObsidianBot, int GeodeBot, int Minutes);

        int GetMaxGeodes(Blueprint bp, State state)
        {
            if (state.Minutes == 0)
                return state.Geode;

            if (cache.ContainsKey(state))
                return cache[state];

            int max = bp.GetTransitions(state).Select(s => GetMaxGeodes(bp, s)).Max();

            cache[state] = max;

            return max;
        }

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {

                var match = Regex.Match(line, "Blueprint (.*): Each ore robot costs (.*) ore. Each clay robot costs (.*) ore. Each obsidian robot costs (.*) ore and (.*) clay. Each geode robot costs (.*) ore and (.*) obsidian.");

                if (!match.Success)
                    throw new Exception();

                blueprints.Add(new Blueprint(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value)));
            }
        }

        public override long Compute()
        {
            ReadData();

            //Blueprint bp = new Blueprint(4, 2, 3, 14, 2, 7);
            //return GetMaxGeodes(bp, new State(0, 0, 0, 0, 1, 0, 0, 0, 24));

            int id = 0;
            long sum = 0;

            foreach (Blueprint bp in blueprints)
            {
                id++;

                int max = GetMaxGeodes(bp, new State(0, 0, 0, 0, 1, 0, 0, 0, 24));

                cache.Clear();

                sum += (max * id);

                Console.WriteLine("BP " + id + ": " + max);
            }

            return sum;
        }

        public override long Compute2()
        {
            //Blueprint bp = new Blueprint(4, 2, 3, 14, 2, 7);
            ////Blueprint bp = new Blueprint(2, 3, 3, 8, 3, 12);
            //return GetMaxGeodes(bp, new State(0, 0, 0, 0, 1, 0, 0, 0, 32));

            ReadData();

            int id = 0;
            long prod = 1;

            foreach (Blueprint bp in blueprints.Take(3))
            {
                id++;

                int max = GetMaxGeodes(bp, new State(0, 0, 0, 0, 1, 0, 0, 0, 32));

                cache.Clear();

                prod *= max;

                Console.WriteLine("BP " + id + ": " + max);
            }

            return prod;
        }
    }
}
