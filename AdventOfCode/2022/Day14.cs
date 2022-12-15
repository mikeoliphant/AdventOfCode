namespace AdventOfCode._2022
{
    internal class Day14 : Day
    {
        SparseGrid<char> grid = new SparseGrid<char>();
        int maxY = 0;
        bool doFloor = false;

        void ReadInput()
        {
            grid.DefaultValue = '.';

            foreach (string lineStr in File.ReadLines(DataFile))
            {
                string[] coords = lineStr.Split(" -> ");

                bool isFirst = true;

                int x = 0;
                int y = 0;

                foreach (string coord in coords)
                {
                    int[] xy = coord.ToInts(',').ToArray();

                    if (isFirst)
                    {
                        x = xy[0];
                        y = xy[1];

                        isFirst = false;
                    }
                    else
                    {
                        int dx = Math.Sign(xy[0] - x);
                        int dy = Math.Sign(xy[1] - y);

                        for (; (x != xy[0]) || (y != xy[1]); x += dx, y += dy)
                        {
                            grid[x, y] = '#';
                        }

                        grid[x, y] = '#';
                    }
                }
            }

            grid[500, 0] = '+';

            int minX;
            int maxX;
            int minY;

            grid.GetBounds(out minX, out minY, out maxX, out maxY);
        }

        bool IsBlocked((int X, int Y) pos)
        {
            char c = grid.GetValue(pos);

            if (doFloor && (pos.Y == maxY))
                return true;

            return (c == '#') || (c == 'o');
        }

        bool DropSand((int X, int Y) pos)
        {
            do
            {
                if (!IsBlocked((pos.X, pos.Y + 1)))
                {
                    pos.Y++;
                }
                else
                {
                    if (!IsBlocked((pos.X - 1, pos.Y + 1)))
                    {
                        pos.Y++;
                        pos.X--;
                    }
                    else
                    {
                        if (!IsBlocked((pos.X + 1, pos.Y + 1)))
                        {
                            pos.Y++;
                            pos.X++;
                        }
                        else
                        {
                            grid[pos] = 'o';

                            return (pos.Y > 0);
                        }
                    }
                }
            }
            while (pos.Y <= maxY);

            return false;
        }

        public override long Compute()
        {
            ReadInput();

            long numSand = 0;

            while(DropSand((500, 0)))
            {
                //grid.PrintToConsole();
                //Console.ReadLine();

                numSand++;
            }

            grid.PrintToConsole();

            return numSand;
        }

        public override long Compute2()
        {
            ReadInput();

            maxY += 2;
            doFloor = true;

            long numSand = 0;

            while (DropSand((500, 0)))
            {
                //grid.PrintToConsole();
                //Console.ReadLine();

                numSand++;
            }

            grid.PrintToConsole();

            return numSand + 1;
        }
    }
}
