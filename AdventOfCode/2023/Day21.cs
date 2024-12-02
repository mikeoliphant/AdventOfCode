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

            grid[startPos] = '.';

            int size = 11;

            Grid<char> newGrid = new(grid.Width * size, grid.Height * size);

            foreach (var cell in grid.GetAll())
            {
                char value = grid[cell];

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        newGrid[(x * grid.Width) + cell.X, (y * grid.Height) + cell.Y] = value;
                    }
                }
            }

            startPos = (((size / 2) * grid.Width) + startPos.X, ((size / 2) * grid.Height) + startPos.Y);

            newGrid[startPos] = 'S';

            grid = newGrid;

            minStepGrid = new Grid<int>(grid.Width, grid.Height);

            outGrid = new Grid<char>(grid);

            DoSteps(262, startPos);

            //outGrid.PrintToConsole();

            long num = outGrid.FindValue('O').Count();

            return num;
        }
    }
}
