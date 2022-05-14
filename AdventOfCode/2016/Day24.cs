namespace AdventOfCode._2016
{
    internal class Day24
    {
        Grid<char> grid = new Grid<char>();

        int endMask;
        (int X, int Y) startPos;

        void ReadInput()
        {
            grid.CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day24.txt"));

            //grid.PrintToConsole();

            endMask = 0;

            foreach (char g in grid.GetAllValues())
            {
                if (char.IsDigit(g))
                {
                    int offset = (g - '0');

                    endMask |= 1 << offset;
                }
            }

            startPos = grid.FindValue('0').First();

        }

        IEnumerable<KeyValuePair<((int X, int Y) Pos, int Mask), float>> GetNeighbors(((int X, int Y) Pos, int Mask) state)
        {
            foreach (var neighbor in grid.ValidNeighbors(state.Pos.X, state.Pos.Y))
            {
                int newMask = state.Mask;

                char n = grid[neighbor];

                if (n == '#')
                    continue;

                if (char.IsDigit(n))
                {
                    int offset = (n - '0');

                    newMask |= 1 << offset;
                }

                yield return new KeyValuePair<((int X, int Y) Pos, int Mask), float>((neighbor, newMask), 1);
            }
        }

        public long Compute()
        {
            ReadInput();

            DijkstraSearch<((int X, int Y) Pos, int Mask)> search = new DijkstraSearch<((int X, int Y) Pos, int Mask)>(GetNeighbors);

            List<((int X, int Y) Pos, int Mask)> path;
            float cost;

            if (search.GetShortestPath((startPos, 1), delegate (((int X, int Y) Pos, int Mask) state) { return state.Mask == endMask; }, out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }

        public long Compute2()
        {
            ReadInput();

            DijkstraSearch<((int X, int Y) Pos, int Mask)> search = new DijkstraSearch<((int X, int Y) Pos, int Mask)>(GetNeighbors);

            List<((int X, int Y) Pos, int Mask)> path;
            float cost;

            if (search.GetShortestPath((startPos, 1), (startPos, endMask), out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }
    }
}
