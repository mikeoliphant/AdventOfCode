using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.FileIO;

namespace AdventOfCode._2023
{
    internal class Day14 : Day
    {
        class RockAutomata : Automata<char>
        {
            public RockAutomata(GridBase<char> grid) : base(grid)
            {
            }

            public override void Cycle()
            {
                Grid = Grid.Clone();

                ShiftNorth(Grid as Grid<char>);
                ShiftWest(Grid as Grid<char>);
                ShiftSouth(Grid as Grid<char>);
                ShiftEast(Grid as Grid<char>);
            }
        }

        static void ShiftNorth(Grid<char> grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == 'O')
                    {
                        int row = y;

                        while ((row > 0) && (grid[x, row - 1] == '.'))
                        {
                            grid[x, row] = '.';
                            grid[x, row - 1] = 'O';

                            row--;
                        }
                    }
                }
            }
        }

        static void ShiftSouth(Grid<char> grid)
        {
            for (int y = grid.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == 'O')
                    {
                        int row = y;

                        while ((row < (grid.Height - 1)) && (grid[x, row + 1] == '.'))
                        {
                            grid[x, row] = '.';
                            grid[x, row + 1] = 'O';

                            row++;
                        }
                    }
                }
            }
        }

        static void ShiftWest(Grid<char> grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[x, y] == 'O')
                    {
                        int col = x;

                        while ((col > 0) && (grid[col - 1, y] == '.'))
                        {
                            grid[col, y] = '.';
                            grid[col - 1, y] = 'O';

                            col--;
                        }
                    }
                }
            }
        }

        static void ShiftEast(Grid<char> grid)
        {
            for (int x = grid.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[x, y] == 'O')
                    {
                        int col = x;

                        while ((col < (grid.Width - 1)) && (grid[col + 1, y] == '.'))
                        {
                            grid[col, y] = '.';
                            grid[col + 1, y] = 'O';

                            col++;
                        }
                    }
                }
            }
        }

        long GetLoad(Grid<char> grid)
        {
            long load = 0;

            foreach (var rock in grid.FindValue('O'))
            {
                load += (grid.Height - rock.Y);
            }

            return load;
        }

        public override long Compute()
        {
            Grid<char> grid = new();

            grid.CreateDataFromRows(File.ReadLines(DataFile));

            ShiftNorth(grid);

            long load = GetLoad(grid);

            return load;
        }

        public override long Compute2()
        {
            Grid<char> grid = new();

            grid.CreateDataFromRows(File.ReadLines(DataFile));

            RockAutomata automata = new RockAutomata(grid);

            int cyclePos;
            int loopSize;

            long maxCycle = 1000000000;

            automata.FindLoop((int)maxCycle, out cyclePos, out loopSize, delegate { return (int)GetLoad(automata.Grid as Grid<char>); }, 1000);

            int offset = (int)(maxCycle - cyclePos) % loopSize;

            int dupeCycle = cyclePos + offset;

            automata.Reset();
            automata.Cycle(dupeCycle);

            long load = GetLoad(automata.Grid as Grid<char>);

            return load;
        }
    }
}
