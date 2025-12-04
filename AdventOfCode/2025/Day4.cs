
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

        public override long Compute()
        {
            var grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            long numAccessible = 0;

            foreach (var cell in grid.GetAll())
            {
                if (CanRemove(grid, cell.X, cell.Y))
                    numAccessible++;
            }

            grid.PrintToConsole();

            return numAccessible;
        }

        (int X, int Y)? ToRemove(Grid<char> grid)
        {
            var available = grid.GetAll().Where(c => CanRemove(grid, c.X, c.Y));

            if (!available.Any())
                return null;

            return available.First();
        }

        public override long Compute2()
        {
            var grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            long numRemoved = 0;

            do
            {
                var toRemove = ToRemove(grid);

                if (toRemove == null)
                    break;

                grid[toRemove.Value] = '.';

                numRemoved++;
            }
            while (true);

            return numRemoved;
        }
    }
}
