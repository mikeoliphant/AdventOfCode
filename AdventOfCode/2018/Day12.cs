namespace AdventOfCode._2018
{
    internal class Day12
    {
        Dictionary<string, bool> potRules = new Dictionary<string, bool>();
        SparseGrid<char> pots = new SparseGrid<char>();

        public void ReadInput()
        {
            pots.DefaultValue = '.';

            IEnumerable<string> lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day12.txt");

            string initialState = lines.First().Split(" ")[2];

            for (int i = 0; i < initialState.Length; i++)
            {
                pots[i, 0] = initialState[i];
            }

            foreach (string rule in lines.Skip(2))
            {
                string[] split = rule.Split(" => ");

                if (split[1][0] == '#')
                    potRules[split[0]] = true;
            }
        }

        public long Compute()
        {
            ReadInput();

            for (int gen = 0; gen < 200; gen++)
            {
                SparseGrid<char> newPots = new SparseGrid<char>();
                newPots.DefaultValue = '.';

                int minX = pots.GetAll().Min(p => p.X) - 2;
                int maxX = pots.GetAll().Max(p => p.X) + 2;

                for (int potX = minX; potX <= maxX; potX++)
                {
                    string neighbors = "";

                    for (int x = potX - 2; x < potX + 3; x++)
                    {
                        char val = '.';

                        pots.TryGetValue(x, 0, out val);

                        neighbors += val;
                    }

                    if (potRules.ContainsKey(neighbors))
                    {
                        newPots[potX, 0] = '#';
                    }
                }

                pots = newPots;
                pots.PrintToConsole();
            }

            int sum = pots.GetAll().Sum(p => p.X);

            long finalSum = sum + ((50000000000 - 200) * 81);

            return sum;
        }
    }
}
