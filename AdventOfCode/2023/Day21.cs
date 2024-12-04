namespace AdventOfCode._2023
{
    internal class Day21 : Day
    {
        Grid<char> grid = null;
        Grid<char> outGrid = null;
        Grid<int> minStepGrid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
            minStepGrid = new Grid<int>(grid.Width, grid.Height);
            outGrid = new Grid<char>(grid);
        }

        void DoSteps(int steps, (int X, int Y) startPos)
        {
            if ((steps % 2) == 0)
            {
                outGrid[startPos] = 'O';
            }
            else
            {
                outGrid[startPos] = 'x';
            }

            if (minStepGrid[startPos] >= steps)
                return;

            minStepGrid[startPos] = steps;

            if (steps == 0)
                return;

            foreach (var next in grid.ValidNeighbors(startPos.X, startPos.Y))
            {
                if ((grid[next] == '.'))
                {
                    DoSteps(steps - 1, next);
                }
            }
        }

        public override long Compute()
        {
            ReadData();

            //grid.PrintToConsole();

            var startPos = grid.FindValue('S').First();

            DoSteps(64, startPos);

            outGrid.PrintToConsole();

            long num = outGrid.FindValue('O').Count();

            return num;
        }

        long ComputeGrid(int numBlocks, Grid<char> baseGrid)
        {
            var startPos = baseGrid.FindValue('S').First();

            baseGrid[startPos] = '.';

            int size = 1 + (numBlocks * 2);

            int numSteps = 65 + (numBlocks * 131);

            Grid<char> newGrid = new(baseGrid.Width * size, baseGrid.Height * size);

            foreach (var cell in baseGrid.GetAll())
            {
                char value = baseGrid[cell];

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        newGrid[(x * baseGrid.Width) + cell.X, (y * baseGrid.Height) + cell.Y] = value;
                    }
                }
            }

            startPos = (((size / 2) * baseGrid.Width) + startPos.X, ((size / 2) * baseGrid.Height) + startPos.Y);

            newGrid[startPos] = 'S';

            grid = newGrid;

            minStepGrid = new Grid<int>(grid.Width, grid.Height);

            outGrid = new Grid<char>(grid);

            DoSteps(numSteps, startPos);

            outGrid.DefaultValue = '.';

            GridDisplay display = new GridDisplay(outGrid, outGrid.Width, outGrid.Height);

            long num = outGrid.FindValue('O').Count();

            return ((numSteps + 1) * (numSteps + 1)) - num;
        }

        public override long Compute2()
        {
            ReadData();

            long oneBlock = ComputeGrid(0, grid);

            ReadData();

            long twoBlock = ComputeGrid(1, grid);

            ReadData();

            long threeBlock = ComputeGrid(2, grid);

            //long steps = 65 + 131;// 26501365;

            //long square1Blocked = GetBlockedSquares(5);   // Just the values from the internal diamond

            //long square2Blocked = GetBlockedSquares(65 + 131);   // The values from both the internal and outer diamonds

            //long numSquareSteps = (steps - 65) / 131;

            //long squaresPerSide = (numSquareSteps * 2);

            //long totBlocked = square1Blocked + (squaresPerSide * 2 * square2Blocked);

            //long tot = (steps + 1) * (steps + 1);

            return 0; // tot - totBlocked;
        }
    }
}
