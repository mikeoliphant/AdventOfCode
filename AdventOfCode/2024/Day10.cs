namespace AdventOfCode._2024
{
    internal class Day10 : Day
    {
        Grid<char> grid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        public override long Compute()
        {
            ReadData();

            //grid.PrintToConsole();

            Dictionary<(int X, int Y), HashSet<(int X, int Y)>> paths = new();

            foreach (var pos in grid.FindValue('9'))
            {
                paths[pos] = new HashSet<(int X, int Y)> { pos };
            }

            for (char height = '8'; height >= '0'; height--)
            {
                foreach (var pos in grid.FindValue(height))
                {
                    paths[pos] = new();

                    foreach (var trailEnds in grid.ValidNeighbors(pos.X, pos.Y).Where(n => (grid[n] == (height + 1))).Select(n => paths[n]))
                    {
                        paths[pos].UnionWith(trailEnds);
                    }
                }
            }

            //var pathGrid = new Grid<int>(grid.Width, grid.Height);

            //foreach (var pos in paths.Keys)
            //{
            //    pathGrid[pos] = paths[pos].Count;
            //}

            //pathGrid.PrintToConsole(delegate (int data) { return data.ToString("|00|"); });

            long totPaths = grid.FindValue('0').Sum(g => paths[g].Count);

            return totPaths;
        }

        public override long Compute2()
        {
            ReadData();

            Dictionary<(int X, int Y), long> paths = new();

            foreach (var pos in grid.FindValue('9'))
            {
                paths[pos] = 1;
            }

            for (char height = '8'; height >= '0'; height--)
            {
                foreach (var pos in grid.FindValue(height))
                {
                    paths[pos] = grid.ValidNeighbors(pos.X, pos.Y).Where(n => (grid[n] == (height + 1))).Sum(n => paths[n]);
                }
            }

            long totPaths = grid.FindValue('0').Sum(g => paths[g]);

            return totPaths;
        }
    }
}
