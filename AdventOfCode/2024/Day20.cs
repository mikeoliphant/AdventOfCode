namespace AdventOfCode._2024
{
    internal class Day20 : Day
    {
        Grid<char> grid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        IEnumerable<((int X, int Y) Pos, int Dist)> GetRange((int X, int Y) pos, int range)
        {
            for (int x = pos.X - range; x <= pos.X + range; x++)
            {
                for (int y = pos.Y - range; y <= pos.Y + range; y++)
                {
                    int dist = Math.Abs(x - pos.X) + Math.Abs(y - pos.Y);

                    if (dist <= range)
                    {
                        if (grid.IsValid(x, y) && grid[x, y] != '#')
                            yield return ((x, y), dist);
                    }
                }
            }
        }

        public override long Compute()
        {
            ReadData();

            GridSearch<char> search = new GridSearch<char>(grid)
            {
                WallValues = new char[] { '#' }
            };

            var path = search.GetShortestPath('S', 'E');

            //search.DrawPath(path, 'O');

            //grid.PrintToConsole();

            int range = 20;

            int savings = 100;

            HashSet<((int X, int Y), (int X, int Y))> cheats = new();

            for (int i = 0; i < path.Count; i++)
            {
                foreach (var other in GetRange(path[i], range))
                {
                    if (!cheats.Contains((path[i], other.Pos)))
                    {
                        int otherPos = path.IndexOf(other.Pos);

                        if ((otherPos - i) >= (savings + other.Dist))
                            cheats.Add((path[i], other.Pos));
                    }    
                }
            }

            return cheats.Count;
        }
    }
}
