namespace AdventOfCode._2019
{
    internal class Day11
    {
        public long Compute()
        {
            SparseGrid<byte> grid = new SparseGrid<byte>();

            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day11.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.SetProgram(program);

            int x = 0;
            int y = 0;
            int dir = 0;

            grid[x, y] = 1;

            do
            {
                byte value = 0;

                grid.TryGetValue(x, y, out value);

                computer.AddInput(value);
                
                if (!computer.RunUntilOutput())
                    break;

                long paint = computer.GetLastOutput();

                grid[x, y] = (byte)paint;

                if (!computer.RunUntilOutput())
                {
                    throw new InvalidOperationException();
                }

                long turn = computer.GetLastOutput();

                switch (turn)
                {
                    case 0:
                        dir--;

                        if (dir < 0)
                            dir = 3;
                        break;
                    case 1:
                        dir++;

                        if (dir > 3)
                            dir = 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                switch (dir)
                {
                    case 0: // up
                        y--;
                        break;
                    case 1: // right
                        x++;
                        break;
                    case 2: // down
                        y++;
                        break;
                    case 3: // left
                        x--;
                        break;
                }
            }
            while (true);

            grid.PrintToConsole();

            return grid.Count;
        }
    }
}
