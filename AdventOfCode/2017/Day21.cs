namespace AdventOfCode._2017
{
    internal class Day21
    {
        List<(Grid<char> Src, Grid<char> Dest)> twoToThreeRules = new List<(Grid<char> Src, Grid<char> Dest)>();
        List<(Grid<char> Src, Grid<char> Dest)> threeToFourRules = new List<(Grid<char> Src, Grid<char> Dest)>();
        Grid<char> grid = null;

        void ReadInput()
        {
            foreach (string ruleStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day21.txt"))
            {
                string[] split = ruleStr.Split(" => ");

                Grid<char> srcGrid = new Grid<char>().CreateDataFromRows(split[0].Split('/'));
                Grid<char> destGrid = new Grid<char>().CreateDataFromRows(split[1].Split('/'));

                if (srcGrid.Width == 2)
                    twoToThreeRules.Add((srcGrid, destGrid));
                else
                    threeToFourRules.Add((srcGrid, destGrid));
            }

            grid = new Grid<char>().CreateDataFromRows(new string[] { ".#.", "..#", "###" });
        }

        void Iterate()
        {
            int subSize = ((grid.Width % 2) == 0) ? 2 : 3;
            int newSubSize = subSize + 1;
            int newSize = (grid.Width / subSize) * newSubSize;

            Grid<char> newGrid = new Grid<char>(newSize, newSize);

            var rules = (subSize == 2) ? twoToThreeRules : threeToFourRules;

            for (int subX = 0; subX < (grid.Width / subSize); subX++)
            {
                for (int subY = 0; subY < (grid.Height / subSize); subY++)
                {
                    bool matchedRule = false;

                    foreach (var rule in rules)
                    {
                        int rotation;
                        bool flipX;
                        bool flipY;

                        if (grid.MatchSquareWithTransforms(subX * subSize, subY * subSize, rule.Src, out rotation, out flipX, out flipY))
                        {
                            for (int y = 0; y < newSubSize; y++)
                            {
                                for (int x = 0; x < newSubSize; x++)
                                {
                                    newGrid[(subX * newSubSize) + x, (subY * newSubSize) + y] = rule.Dest.GetValue(x, y); // GetTransformedValue(x, y, rotation, flipX, flipY);
                                }
                            }

                            matchedRule = true;

                            break;
                        }
                    }

                    if (!matchedRule)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            grid = newGrid;
        }

        public long Compute()
        {
            ReadInput();

            for (int i = 0; i < 18; i++)
            {
                Iterate();
            }

            return grid.CountValue('#');
        }
    }
}
