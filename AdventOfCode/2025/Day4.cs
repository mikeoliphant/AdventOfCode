
namespace AdventOfCode._2025
{
    internal class Day4 : Day
    {
        bool CanRemove(Grid<char> grid, int x, int y)
        {
            if (grid[x, y] == '@')
            {
                int numNeighbors = grid.ValidNeighbors(x, y, includeDiagonal: true).Where(n => grid[n] == '@').Count();

                if (numNeighbors < 4)
                {
                    return true;
                }
            }

            return false;
        }

        IEnumerable<(int X, int)> AvailableToRemove(Grid<char> grid)
        {
            return grid.GetAll().Where(c => CanRemove(grid, c.X, c.Y));
        }

        public override long Compute()
        {
            var grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            return AvailableToRemove(grid).Count();
        }

        public override long Compute2()
        {
            var grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            long numRemoved = 0;

            do
            {
                var toRemove = AvailableToRemove(grid).ToList();

                if (!toRemove.Any())
                    break;

                numRemoved += toRemove.Count();

                foreach (var cell in toRemove)
                {
                    grid[cell] = '.';
                }
            }
            while (true);

            grid.PrintToConsole();

            return numRemoved;
        }
    }
}
