namespace AdventOfCode._2024
{
    internal class Day18 : Day
    {
        List<IntVec2> bytes = null;
        Grid<char> grid;

        void ReadData()
        {
            bytes = new();

            foreach (string line in File.ReadLines(DataFile))
            {
                bytes.Add(new IntVec2(line.ToInts(',').ToArray()));
            }
        }

        void AddBytes(int numBytes)
        {
            for (int i = 0; i < numBytes; i++)
            {
                grid[bytes[i]] = '#';
            }
        }

        IEnumerable<IntVec2> GetNeighbors(IntVec2 pos)
        {
            return grid.ValidNeighbors(pos.X, pos.Y).Where(n => grid[n] != '#').Select(n => new IntVec2(n));
        }

        public override long Compute()
        {
            ReadData();

            //grid = new Grid<char>(7, 7);
            grid = new Grid<char>(71, 71);
            grid.Fill('.');

            AddBytes(1024);

            grid.PrintToConsole();

            var search = new DijkstraSearch<IntVec2>(GetNeighbors);

            var path = search.GetShortestPath(new IntVec2(0, 0), new IntVec2(grid.Width - 1, grid.Height - 1));
 
            return (long)path.Cost;
        }

        public override long Compute2()
        {
            ReadData();

            //grid = new Grid<char>(7, 7);
            grid = new Grid<char>(71, 71);
            grid.Fill('.');

            bool havePath = false;

            int bytePos = 0;
            IntVec2 drop;

            var search = new DijkstraSearch<IntVec2>(GetNeighbors);

            do
            {
                drop = bytes[bytePos++];

                grid[drop] = '#';

                var path = search.GetShortestPath(new IntVec2(0, 0), new IntVec2(grid.Width - 1, grid.Height - 1));

                havePath = path.Path != null;
            }
            while (havePath);

            return 0;
        }
    }
}
