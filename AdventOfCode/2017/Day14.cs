namespace AdventOfCode._2017
{
    internal class Day14
    {
        string key = "hfdlxzhv";

        int size = 128;
        Grid<char> grid = null;

        void ReadInput()
        {
            grid = new Grid<char>(size, size);

            for (int row = 0; row < size; row++)
            {
                string rowHash = KnotHash.ComputeHash(key + "-" + row);

                string binaryString = String.Join("", rowHash.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                for (int col = 0; col < binaryString.Length; col++)
                {
                    grid[col, row] = (binaryString[col] == '1') ? '#' : '.';
                }
            }
        }

        void PropogageGroup(int x, int y)
        {
            List<(int X, int Y)> toProp = new List<(int X, int Y)>();

            foreach (var neighbor in grid.ValidNeighbors(x, y))
            {
                if (grid[neighbor] == '#')
                {
                    toProp.Add(neighbor);

                    grid[neighbor] = 'G';
                }
            }

            foreach (var neighbor in toProp)
            {
                PropogageGroup(neighbor.X, neighbor.Y);
            }
        }

        public long Compute2()
        {
            ReadInput();

            string hash = KnotHash.ComputeHash("");

            int numGroups = 0;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    char value = '\0';

                    grid.TryGetValue(x, y, out value);

                    if (value == '#')
                    {
                        numGroups++;

                        grid[x, y] = 'G';

                        PropogageGroup(x, y);
                    }
                }
            }

            return numGroups;
        }
    }
}
