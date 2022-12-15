namespace AdventOfCode._2022
{
    internal class Day12 : Day
    {
        Grid<char> grid;

        IEnumerable<(int X, int Y)> GetNeighbors((int X, int Y) pos)
        {
            char current = grid[pos];

            foreach (var neighbor in grid.ValidNeighbors(pos.X, pos.Y))
            {
                if (((int)(grid[neighbor]) - current) < 2)
                {
                    yield return neighbor;
                }
            }
        }

        public override long Compute()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            //grid.PrintToConsole();

            var start = grid.FindValue('S').First();
            var end = grid.FindValue('E').First();

            grid[start] = 'a';
            grid[end] = 'z';

            DijkstraSearch<(int X, int Y)> search = new DijkstraSearch<(int X, int Y)>(GetNeighbors);

            var result = search.GetShortestPath(start, end);

            if (result.Path == null)
            {
                throw new Exception();
            }

            return (long)result.Cost;
        }

        public override long Compute2()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            //grid.PrintToConsole();

            var start = grid.FindValue('S').First();
            var end = grid.FindValue('E').First();

            grid[start] = 'a';
            grid[end] = 'z';

            float minCost = float.MaxValue;

            foreach (var aPos in grid.FindValue('a'))
            {
                DijkstraSearch<(int X, int Y)> search = new DijkstraSearch<(int X, int Y)>(GetNeighbors);

                var result = search.GetShortestPath(aPos, end);

                if (result.Path != null)
                {
                    if (result.Cost < minCost)
                    {
                        minCost = result.Cost;
                    }
                }
            }

            return (long)minCost;
        }
    }
}
