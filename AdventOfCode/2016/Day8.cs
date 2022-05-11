namespace AdventOfCode._2016
{
    internal class Day8
    {
        public long Compute()
        {
            //Grid<char> grid = new Grid<char>(7, 3);
            Grid<char> grid = new Grid<char>(50, 6);

            foreach (string cmd in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day8.txt"))
            {
                var match = Regex.Match(cmd, "rect (.*)x(.*)");

                if (match.Success)
                {
                    foreach (var g in Grid<int>.GetRectangle(new Rectangle(0, 0, int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value))))
                    {
                        grid.SetValue(g.X, g.Y, '#');
                    }
                }
                else
                {
                    match = Regex.Match(cmd, "rotate column x=(.*) by (.*)");

                    if (match.Success)
                    {
                        int x = int.Parse(match.Groups[1].Value);
                        int rot = int.Parse(match.Groups[2].Value);

                        Grid<char> newGrid = new Grid<char>(grid);

                        for (int y = 0; y < grid.Height; y++)
                        {
                            newGrid[x, (y + rot) % grid.Height] = grid[x, y];
                        }

                        grid = newGrid;
                    }
                    else
                    {
                        match = Regex.Match(cmd, "rotate row y=(.*) by (.*)");

                        if (match.Success)
                        {
                            int y = int.Parse(match.Groups[1].Value);
                            int rot = int.Parse(match.Groups[2].Value);

                            Grid<char> newGrid = new Grid<char>(grid);

                            for (int x = 0; x < grid.Width; x++)
                            {
                                newGrid[(x + rot) % grid.Width, y] = grid[x, y];
                            }

                            grid = newGrid;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                
            }

            grid.PrintToConsole();

            return grid.CountValue('#');
        }
    }
}
