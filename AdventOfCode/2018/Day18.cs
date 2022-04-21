namespace AdventOfCode._2018
{
    internal class Day18
    {
        Grid<char> grid = null;

        void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day18.txt"));

        }

        void Cycle()
        {
            Grid<char> newGrid = new Grid<char>(grid.Width, grid.Height);

            foreach (var pos in grid.GetAll())
            {
                char c = grid[pos.X, pos.Y];

                var neighbors = grid.ValidNeighborValues(pos.X, pos.Y, includeDiagonal: true);

                switch (c)
                {
                    case '.':
                        if (neighbors.Count('|') > 2)
                        {
                            newGrid[pos.X, pos.Y] = '|';
                        }
                        else
                        {
                            newGrid[pos.X, pos.Y] = '.';
                        }
                        break;

                    case '|':
                        if (neighbors.Count('#') > 2)
                        {
                            newGrid[pos.X, pos.Y] = '#';
                        }
                        else
                        {
                            newGrid[pos.X, pos.Y] = '|';
                        }
                        break;

                    case '#':
                        if ((neighbors.Count('|') > 0) && (neighbors.Count(n => n == '#') > 0))
                        {
                            newGrid[pos.X, pos.Y] = '#';
                        }
                        else
                        {
                            newGrid[pos.X, pos.Y] = '.';
                        }
                        break;
                }
            }

            grid = newGrid;
        }

        public long Compute()
        {
            ReadInput();

            //grid.PrintToConsole();

            for (int cycle = 0; cycle < 10; cycle++)
            {
                Cycle();

                //grid.PrintToConsole();
            }

            return grid.CountValue('|') * grid.CountValue('#');
        }

        public long Compute2()
        {
            Dictionary<long, long> history = new Dictionary<long, long>();

            ReadInput();

            int histInARow = 0;

            //grid.PrintToConsole();

            long maxCycle = 664; // 1000000000;

            for (long cycle = 0; cycle < maxCycle; cycle++)
            {
                long resources = grid.CountValue('|') * grid.CountValue('#');

                if (history.ContainsKey(resources))
                {
                    histInARow++;

                    if (histInARow == 100)
                    {
                        long loopSize = cycle - history[resources];

                        long offset = (maxCycle - cycle) % loopSize;

                        long dupeCycle = cycle + offset;
                    }
                }
                else
                {
                    histInARow = 0;
                }

                history[resources] = cycle;

                Cycle();

                //grid.PrintToConsole();
            }

            return grid.CountValue('|') * grid.CountValue('#');
        }
    }
}
