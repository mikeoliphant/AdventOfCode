using SkiaSharp;

namespace AdventOfCode._2018
{
    internal class Day17
    {
        VisualSparseGrid<char> grid = new VisualSparseGrid<char>(1845, 1000);
        Point waterSource = new Point(500, 0);
        int minX;
        int maxX;
        int minY;
        int maxY;

        void ReadInput()
        {
            Dictionary<char, SKColor> colors = new Dictionary<char, SKColor>();
            colors['~'] = SKColors.Blue;
            colors['|'] = SKColors.LightBlue;
            colors['*'] = SKColors.Red;

            grid.Colors = colors;

            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day17.txt"))
            {
                string[] split = line.Split(", ");

                Rectangle rect = Rectangle.Empty;

                foreach (string coord in split)
                {
                    string[] varRange = coord.Split('=');

                    int min = 0;
                    int max = 0;

                    if (varRange[1].Contains('.'))
                    {
                        int[] minMax = varRange[1].ToInts("..").ToArray();

                        min = minMax[0];
                        max = minMax[1];
                    }
                    else
                    {
                        min = max = int.Parse(varRange[1]);
                    }

                    if (varRange[0][0] == 'x')
                    {
                        rect.X = min;
                        rect.Width = (max - min) + 1;
                    }
                    else
                    {
                        rect.Y = min;
                        rect.Height = (max - min) + 1;
                    }
                }

                foreach (var pos in grid.GetRectangle(rect))
                {
                    grid[pos.X, pos.Y] = '#';
                }
            }

            //grid.CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day17Test2.txt"));
            //var pos = grid.GetAll().Where(g => grid[g.X, g.Y] == '+').First();
            //waterSource = new Point(pos.X, pos.Y);

            grid.GetBounds(out minX, out minY, out maxX, out maxY);
        }

        bool CanFlow(int x, int y)
        {
            char c;

            if (!grid.TryGetValue(x, y, out c))
            {
                return true;
            }

            return (c == ' ') || (c == '|');
        }

        bool IsStream(int x, int y)
        {
            char c;

            if (!grid.TryGetValue(x, y, out c))
            {
                return false;
            }

            return (c == '|');
        }

        int skip = 0;

        bool PushWater(Point startPos, int dx, out Point pos)
        {
            pos = startPos;

            grid[pos.X, pos.Y] = '|';

            if (dx == 0)
            {
                while (CanFlow(pos.X, pos.Y + 1))
                {
                    pos.Y++;

                    if (IsStream(pos.X, pos.Y))
                    {
                        return false;
                    }

                    if (pos.Y > maxY)
                    {
                        return false;
                    }

                    grid[pos.X, pos.Y] = '|';
                }

                do
                {
                    Point leftPos;
                    Point rightPos;

                    bool pushLeft = PushWater(pos, -1, out leftPos);
                    bool pushRight = PushWater(pos, 1, out rightPos);

                    if (pushLeft && pushRight)
                    {
                        for (int x = leftPos.X; x <= rightPos.X; x++)
                        {
                            grid[x, pos.Y] = '~';
                        }

                        if ((skip++ % 100) == 0)
                            grid.ReDraw();
                    }
                    else
                    {
                        return false;
                    }

                    pos.Y--;
                }
                while ((pos.Y >= startPos.Y) && !CanFlow(pos.X, pos.Y + 1));

                return true;
            }

            while (CanFlow(pos.X + dx, pos.Y))
            {
                pos.X += dx;

                if (IsStream(pos.X, pos.Y))
                    return false;

                grid[pos.X, pos.Y] = '|';

                if (CanFlow(pos.X, pos.Y + 1))
                {
                    if (!PushWater(new Point(pos.X, pos.Y + 1), 0, out pos))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public long Compute()
        {
            ReadInput();

            Point pos;

            int startY = waterSource.Y + 1;

            if (startY < minY)
                startY = minY;

            PushWater(new Point(waterSource.X, startY), 0, out pos);

            grid.ReDraw();

            //grid.PrintToConsole();

            //return grid.GetAllValues().Count(g => (g == '~') || (g == '|'));
            return grid.GetAllValues().Count(g => (g == '~'));
        }

    }
}
