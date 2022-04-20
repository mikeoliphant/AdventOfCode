namespace AdventOfCode._2018
{
    internal class Day17
    {
        VisualSparseGrid<char> grid = new VisualSparseGrid<char>(1000, 1000);
        Point waterSource = new Point(500, 0);
        int minX;
        int maxX;
        int minY;
        int maxY;

        void ReadInput()
        {
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

            return (c == '|') || (c == ' ') || (c == '+');
        }

        bool TrySettle(Point pos, int dx, out Point settlePos)
        {
            if (dx == 0)
            {
                while (CanFlow(pos.X, pos.Y + 1))
                {
                    pos.Y++;

                    grid[pos.X, pos.Y] = '|';

                    if (pos.Y >= maxY)
                    {
                        settlePos = Point.Empty;

                        return false;
                    }
                }

                Point leftPos;
                Point rightPos;

                bool settleLeft = TrySettle(pos, -1, out leftPos);
                bool settleRight = TrySettle(pos, 1, out rightPos);

                if (settleLeft && settleRight)
                {
                    if ((leftPos.Y < rightPos.Y) || ((leftPos.Y == rightPos.Y) && (Math.Abs(leftPos.X - pos.X)) > Math.Abs(rightPos.X - pos.X)))
                    {
                        settlePos = leftPos;
                    }
                    else
                    {
                        settlePos = rightPos;
                    }

                    return true;
                }

                if (settleLeft && (leftPos.Y > pos.Y))
                {
                    settlePos = leftPos;

                    return true;
                }
                else if (settleRight && (rightPos.Y > pos.Y))
                {
                    settlePos = rightPos;

                    return true;
                }

                settlePos = Point.Empty;

                return false;
            }
            else
            {
                while (CanFlow(pos.X + dx, pos.Y))
                {
                    pos.X += dx;

                    grid[pos.X, pos.Y] = '|';

                    if (CanFlow(pos.X, pos.Y + 1))
                    {
                        return TrySettle(pos, 0, out settlePos);
                    }
                }

                settlePos = pos;

                return true;
            }
        }

        bool AddWater()
        {
            Point pos = waterSource;

            if (!CanFlow(pos.X, pos.Y))
            {
                return false;
            }

            if (!TrySettle(pos, 0, out pos))
                return false;

            grid[pos.X, pos.Y] = '~';

            return true;
        }

        public long Compute()
        {
            ReadInput();

            do
            {
                if (!AddWater())
                    break;

                grid.ReDraw();

                //grid.PrintToConsole();
                //Console.ReadLine();
            }
            while (true);

            grid.PrintToConsole();

            return grid.GetAllValues().Count(g => (g == '~') || (g == '|'));
        }

    }
}
