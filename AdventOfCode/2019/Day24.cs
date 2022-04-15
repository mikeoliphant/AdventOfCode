namespace AdventOfCode._2019
{
    internal class Day24
    {
        Grid<char> Cycle(Grid<char> grid)
        {
            Grid<char> newGrid = new Grid<char>(grid.Width, grid.Height);

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    int neighbors = grid.ValidNeighborValues(x, y, includeDiagonal: false).Count(g => (g == '#'));

                    if (grid[x, y] == '#')
                    {
                        if (neighbors == 1)
                        {
                            newGrid[x, y] = '#';
                        }                        
                    }
                    else
                    {
                        if ((neighbors == 1) || (neighbors == 2))
                        {
                            newGrid[x, y] = '#';
                        }
                    }
                }
            }

            return newGrid;
        }

        long CalculateDiversity(Grid<char> grid)
        {
            long diversity = 0;
            int pos = 0;

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == '#')
                        diversity |= (long)1 << pos;

                    pos++;
                }
            }

            return diversity;
        }

        Dictionary<long, bool> pastDiversity = new Dictionary<long, bool>();

        public long Compute()
        {
            Grid<char> grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day24.txt"));

            do
            {
                grid = Cycle(grid);

                long diversity = CalculateDiversity(grid);

                if (pastDiversity.ContainsKey(diversity))
                    return diversity;

                pastDiversity[diversity] = true;
            }
            while (true);
        }

        List<Grid<char>> levels = new List<Grid<char>>();

        IEnumerable<char> GetMultilevelNeighbors(int x, int y, int level)
        {
            Grid<char> thisLevel = levels[level];

            foreach (char c in levels[level].ValidNeighborValues(x, y, includeDiagonal: false))
                yield return c;

            Grid<char> upLevel = null;
            if (level > 0)
                upLevel = levels[level - 1];

            Grid<char> downLevel = null;
            if (level < (levels.Count - 1))
                downLevel = levels[level + 1];

            if (upLevel != null)
            {
                if (x == 0)
                {
                    yield return upLevel[1, 2];
                }
                else if (x == 4)
                {
                    yield return upLevel[3, 2];
                }

                if (y == 0)
                {
                    yield return upLevel[2, 1];
                }
                else if (y == 4)
                {
                    yield return upLevel[2, 3];
                }
            }

            if (downLevel != null)
            {
                if ((x == 2) && (y == 1))
                {
                    for (int dx = 0; dx < 5; dx++)
                    {
                        yield return downLevel[dx, 0];
                    }
                }
                else if ((x == 2) && (y == 3))
                {
                    for (int dx = 0; dx < 5; dx++)
                    {
                        yield return downLevel[dx, 4];
                    }
                }
                else if ((y == 2) && (x == 1))
                {
                    for (int dy = 0; dy < 5; dy++)
                    {
                        yield return downLevel[0, dy];
                    }
                }
                else if ((y == 2) && (x == 3))
                {
                    for (int dy = 0; dy < 5; dy++)
                    {
                        yield return downLevel[4, dy];
                    }
                }
            }
        }

        Grid<char> Cycle2(int level)
        {
            Grid<char> grid = levels[level];
            Grid<char> newGrid = new Grid<char>(grid.Width, grid.Height);

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if ((x == 2) && (y == 2))
                    {
                        newGrid[x, y] = '?';
                        continue;
                    }

                    int neighbors = GetMultilevelNeighbors(x, y, level).Count(g => (g == '#'));

                    if (grid[x, y] == '#')
                    {
                        if (neighbors == 1)
                        {
                            newGrid[x, y] = '#';
                        }
                    }
                    else
                    {
                        if ((neighbors == 1) || (neighbors == 2))
                        {
                            newGrid[x, y] = '#';
                        }
                    }
                }
            }

            return newGrid;
        }

        public long Compute2()
        {
            Grid<char> startGrid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day24.txt"));

            levels.Add(startGrid);

            for (int cycle = 0; cycle < 200; cycle++)
            {
                List<Grid<char>> newLevels = new List<Grid<char>>();

                levels.Insert(0, new Grid<char>(startGrid.Width, startGrid.Height));
                levels.Add(new Grid<char>(startGrid.Width, startGrid.Height));

                for (int level = 0; level < levels.Count; level++)
                {
                    newLevels.Add(Cycle2(level));
                }

                levels = newLevels;
            }

            //for (int pos = 0; pos < levels.Count; pos++)
            //{
            //    Console.WriteLine("Depth: " + (pos - 10));
            //    levels[pos].PrintToConsole();
            //}

            long numBugs = 0;

            foreach (Grid<char> level in levels)
            {
                numBugs += level.GetAllValues().Count(g => g == '#');
            }

            return numBugs;
        }
    }
}
