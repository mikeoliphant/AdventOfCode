namespace AdventOfCode._2019
{
    internal class Day19
    {
        IntcodeComputer computer;

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day19.txt").ToLongs(',').ToArray();

            SparseGrid<char> sparseGrid = new SparseGrid<char>();

            computer = new IntcodeComputer();
            computer.SetProgram(program);
        }

        Grid<char> PlotGrid(int startX, int startY, int size)
        {
            return PlotGrid(startX, startY, size, size);
        }

        Grid<char> PlotGrid(int startX, int startY, int width, int height)
        {
            Grid<char> grid = new Grid<char>(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    computer.Reset();

                    computer.AddInput(x + startX);
                    computer.AddInput(y + startY);

                    computer.RunUntilHalt();

                    grid[x, y] = (computer.GetLastOutput() == 1) ? '#' : '.';
                }
            }

            return grid;
        }

        public long Compute()
        {
            ReadInput();

            Grid<char>grid = PlotGrid(0, 0, 50);

            grid.PrintToConsole();

            return grid.Count('#');
        }

        public long Compute2()
        {
            ReadInput();

            int y = 10000;

            int startX = 0;
            int endX = 0;

            do
            {
                computer.Reset();

                computer.AddInput(endX);
                computer.AddInput(y);

                computer.RunUntilHalt();

                if (computer.GetLastOutput() == 1)
                {
                    if (startX == 0)
                    {
                        startX = endX;
                    }
                }
                else
                {
                    if (startX != 0)
                    {
                        endX--;

                        break;
                    }
                }

                endX++;
            }
            while (true);

            float startSlope = (float)startX / (float)y;
            float endSlope = (float)endX / (float)y;

            int boxSize = 100;

            //
            // Some algebra:
            //

            //endX = startX + 100
            //endY = startY + 100
            //startY = endY - 100
            //endX = endSlope * startY
            //startX = startSlope * endY

            //endX = endSlope * (endY - 100)
            //(startX + 100) = endSlope * (endY - 100)

            //(startSlope * endY) + 100 = endSlope * (endY - 100)

            //(startSlope * endY) = (endSlope * endY) - (endSlope * 100) - 100

            //(startSlope * endY) - (endSlope * endY) = -(endSlope * 100) - 100

            //endY * (startSlope - endSlope) = -(endSlope * 100) - 100

            //endY = (-(endSlope * 100) - 100) / (startSlope - endSlope)

            float boxEndY = (-(endSlope * boxSize) - boxSize) / (startSlope - endSlope);

            float boxStartY = boxEndY - boxSize;
            float boxStartX = startSlope * boxEndY;

            int gx = (int)boxStartX - 10;   // Manually futzed for precision errors
            int gy = (int)boxStartY - 14;

            Grid<char> grid = PlotGrid(gx - 1, gy - 1, boxSize + 2);

            for (int y1 = 1; y1 < (boxSize + 1); y1++)
            {
                for (int x1 = 1; x1 < (boxSize + 1); x1++)
                {
                    if (grid[x1, y1] != '#')
                        throw new Exception();

                    grid[x1, y1] = 'O';
                }
            }

            grid.PrintToConsole();

            return (gx * 10000) + gy;
        }
    }
}