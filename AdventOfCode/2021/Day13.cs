namespace AdventOfCode._2021
{
    public class Day13
    {
        int width;
        int height;
        List<Point> points = new List<Point>();
        string[] folds;
        char[,] grid;

        void ReadInput()
        {
            string[] sections = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day13.txt").SplitParagraphs();

            foreach (string line in sections[0].SplitLines())
            {
                string[] pair = line.Split(',');

                points.Add(new Point(int.Parse(pair[0]), int.Parse(pair[1])));
            }

            folds = sections[1].SplitLines();

            width = (from point in points select point.X).Max() + 1;
            height = (from point in points select point.Y).Max() + 1;

            grid = new char[width, height];
        }

        void PlotGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = '.';
                }
            }

            foreach (Point p in points)
            {
                grid[p.X, p.Y] = '#';
            }
        }

        void PrintGrid()
        {

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(grid[x, y]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public long Compute()
        {
            ReadInput();

            //PrintGrid();
            PlotGrid();

            foreach (string fold in folds)
            {
                string[] split = fold.Split('=');

                char xy = split[0][split[0].Length - 1];
                int val = int.Parse(split[1]);

                for (int pos = 0; pos < points.Count; pos++)
                {
                    Point p = points[pos];

                    if (xy == 'x')
                    {
                        if (p.X > val)
                        {
                            points[pos] = new Point(val - (p.X - val), p.Y);
                        }

                        width = val;
                    }
                    else if (xy == 'y')
                    {
                        if (p.Y > val)
                        {
                            points[pos] = new Point(p.X, val - (p.Y - val));
                        }

                        height = val;
                    }
                }

                //PrintGrid();
                PlotGrid();

                //break;
            }

            long numPoints = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y] == '#')
                        numPoints++;
                }
            }

            PrintGrid();

            return numPoints;
        }
    }
}
