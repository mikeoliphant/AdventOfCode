namespace AdventOfCode._2018
{
    internal class Day10
    {
        List<Point> points = new List<Point>();
        List<Point> velocities = new List<Point>();

        void ReadInput()
        {
            foreach (string coordStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day10.txt"))
            {
                Match match = Regex.Match(coordStr, "position=\\<(.*?)\\> velocity=\\<(.*?)\\>");

                points.Add(match.Groups[1].Value.ToPoint());
                velocities.Add(match.Groups[2].Value.ToPoint());
            }

        }

        public long Compute()
        {
            ReadInput();

            int numSeconds = 0;

            do
            {
                SparseGrid<char> grid = new SparseGrid<char>();

                foreach (Point point in points)
                {
                    grid[point.X, point.Y] = '#';
                }

                int numContiguous = 0;

                foreach (var pos in grid.GetAll())
                {
                    char c;

                    if (grid.ValidNeighbors(pos.X, pos.Y).Any())
                        numContiguous++;
                }

                if (((float)numContiguous / (float)grid.Count) > 0.9f)
                {
                    grid.PrintToConsole();

                    break;
                }

                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = new Point(points[i].X + velocities[i].X, points[i].Y + velocities[i].Y);
                }

                numSeconds++;
            }
            while (true);
           
            return numSeconds;
        }
    }
}
