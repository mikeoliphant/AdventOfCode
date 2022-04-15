namespace AdventOfCode._2021
{
    struct Line
    {
        public Point Start;
        public Point End;
    }

    public class Day5
    {
        int xSize = 0;
        int ySize = 0;

        List<Line> lines = new List<Line>();
        int[,] grid;

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\DayFiveInput.txt"))
            {
                string[] points = line.Split(" -> ");

                string[] point1Str = points[0].Split(',');
                string[] point2Str = points[1].Split(',');

                lines.Add(new Line { Start = new Point(int.Parse(point1Str[0]), int.Parse(point1Str[1])), End = new Point(int.Parse(point2Str[0]), int.Parse(point2Str[1])) } );
            }


            foreach (Line line in lines)
            {
                xSize = Math.Max(xSize, line.Start.X);
                xSize = Math.Max(xSize, line.End.X);

                ySize = Math.Max(ySize, line.Start.Y);
                ySize = Math.Max(ySize, line.End.Y);
            }

            grid = new int[xSize + 1, ySize + 1];
        }

        public void Compute()
        {
            ReadInput();

            foreach (Line line in lines)
            {
                if (line.Start.X == line.End.X)
                {
                    int delta = Math.Sign(line.End.Y - line.Start.Y);

                    for (int y = line.Start.Y; y != (line.End.Y + delta); y += delta)
                    {
                        grid[line.Start.X, y]++;
                    }
                }
                else if (line.Start.Y == line.End.Y)
                {
                    int delta = Math.Sign(line.End.X - line.Start.X);

                    for (int x = line.Start.X; x != (line.End.X + delta); x += delta)
                    {
                        grid[x, line.Start.Y]++;
                    }
                }
            }

            int numOverlaps = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (grid[x, y] > 1)
                        numOverlaps++;
                }
            }
        }

        public void Compute2()
        {
            ReadInput();

            foreach (Line line in lines)
            {
                if (line.Start.X == line.End.X)
                {
                    int delta = Math.Sign(line.End.Y - line.Start.Y);

                    for (int y = line.Start.Y; y != (line.End.Y + delta); y += delta)
                    {
                        grid[line.Start.X, y]++;
                    }
                }
                else if (line.Start.Y == line.End.Y)
                {
                    int delta = Math.Sign(line.End.X - line.Start.X);

                    for (int x = line.Start.X; x != (line.End.X + delta); x += delta)
                    {
                        grid[x, line.Start.Y]++;
                    }
                }
                else
                {
                    int deltaX = Math.Sign(line.End.X - line.Start.X);
                    int deltaY = Math.Sign(line.End.Y - line.Start.Y);

                    int x = line.Start.X;
                    int y = line.Start.Y;

                    grid[x, y]++;

                    do
                    {
                        x += deltaX;
                        y += deltaY;

                        grid[x, y]++;
                    }
                    while (x != line.End.X);
                }
            }

            int numOverlaps = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (grid[x, y] > 1)
                        numOverlaps++;
                }
            }
        }
    }
}
