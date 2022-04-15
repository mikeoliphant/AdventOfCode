namespace AdventOfCode._2019
{
    internal class Day15
    {
        IntcodeComputer computer = new IntcodeComputer();
        SparseGrid<char> grid = new SparseGrid<char>();
        int goalX;
        int goalY;

        void Search(int x, int y)
        {
            //Console.WriteLine("Search " + x + "," + y);

            for (int move = 1; move < 5; move++)
            {
                int newX = 0;
                int newY = 0;
                int returnCmd = 0;

                switch (move)
                {
                    case 1: // north
                        newX = x;
                        newY = y - 1;
                        returnCmd = 2;
                        break;
                    case 2: // south
                        newX = x;
                        newY = y + 1;
                        returnCmd = 1;
                        break;
                    case 3: // west
                        newX = x - 1;
                        newY = y;
                        returnCmd = 4;
                        break;
                    case 4: // east
                        newX = x + 1;
                        newY = y;
                        returnCmd = 3;
                        break;
                }

                char c;

                if (grid.TryGetValue(newX, newY, out c))    // We've already been to this square
                    continue;

                computer.AddInput(move);
                computer.RunUntilOutput();

                long output = computer.GetLastOutput();

                bool moved = false;

                switch (output)
                {
                    case 0:
                        grid[newX, newY] = '#';
                        break;
                    case 1:
                        grid[newX, newY] = '.';
                        moved = true;
                        break;
                    case 2:
                        grid[newX, newY] = 'O';
                        moved = true;

                        goalX = newX;
                        goalY = newY;

                        break;
                    default:
                        throw new InvalidOperationException();
                }

                if (moved)
                {
                    Search(newX, newY);

                    computer.AddInput(returnCmd);
                    computer.RunUntilOutput();
                }
            }
        }

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day15.txt").ToLongs(',').ToArray();

            computer.SetProgram(program);

            grid[0, 0] = '.';

            Search(0, 0);
        }


        public long Compute()
        {
            ReadInput();

            grid.PrintToConsole();

            List<ValueTuple<int, int>> path;
            float cost;

            if (grid.DijkstraSearch(0, 0, goalX, goalY, '#', out path, out cost))
            {
                return (int)cost;
            }

            throw new InvalidOperationException();
        }

        bool PushOxygen(int x, int y)
        {
            char value;

            grid.TryGetValue(x, y, out value);

            if (value == '.')
            {
                grid[x, y] = 'O';

                return true;
            }

            return false;
        }

        public long Compute2()
        {
            ReadInput();

            List<Point> oxygenSquares = new List<Point>();

            oxygenSquares.Add(new Point(goalX, goalY));

            int step = 0;

            while (oxygenSquares.Count > 0)
            {
                List<Point> newSquares = new List<Point>();

                foreach (Point p in oxygenSquares)
                {
                    if (PushOxygen(p.X, p.Y - 1))
                    {
                        newSquares.Add(new Point(p.X, p.Y - 1));
                    }

                    if (PushOxygen(p.X, p.Y + 1))
                    {
                        newSquares.Add(new Point(p.X, p.Y + 1));
                    }

                    if (PushOxygen(p.X - 1, p.Y))
                    {
                        newSquares.Add(new Point(p.X - 1, p.Y));
                    }

                    if (PushOxygen(p.X + 1, p.Y))
                    {
                        newSquares.Add(new Point(p.X + 1, p.Y));
                    }
                }

                oxygenSquares = newSquares;

                step++;
            }

            grid.PrintToConsole();

            return step - 1;
        }
    }
}
