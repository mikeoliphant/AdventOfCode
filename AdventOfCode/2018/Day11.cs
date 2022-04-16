namespace AdventOfCode._2018
{
    internal class Day11
    {
        int size = 300;
        int gridSerialNumber = 2866;

        SparseGrid<int> grid = new SparseGrid<int>();

        void CalculatePower()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int rackID = (x + 1) + 10;

                    int power = rackID * (y + 1);

                    power += gridSerialNumber;

                    power *= rackID;

                    power = (power / 100) % 10;

                    power -= 5;

                    grid[x, y] = power;
                }
            }
        }

        public long Compute()
        {
            CalculatePower();

            int kernelSize = 3;

            int kernelHalfSize = kernelSize / 2;

            int max = grid.GetAll().Max(g => grid.GetValidRectangleValues(new Rectangle(g.X - kernelHalfSize, g.Y - kernelHalfSize, kernelSize, kernelSize)).Sum());

            var pos = grid.GetAll().First(g => (grid.GetValidRectangleValues(new Rectangle(g.X - kernelHalfSize, g.Y - kernelHalfSize, kernelSize, kernelSize)).Sum() == max));

            return 0;
        }

        public long Compute2()
        {
            CalculatePower();

            Grid<int> oneByOnePow = grid.ToGrid();

            Dictionary<int, int> kernelMax = new Dictionary<int, int>();
            Dictionary<int, (int X, int Y)> kernelMaxPos = new Dictionary<int, (int X, int Y)>();

            Grid<int> lastGrid = oneByOnePow;

            for (int kernelSize = 2; kernelSize <= size; kernelSize++)
            {
                Grid<int> thisGrid = new Grid<int>(size, size);

                int max = 0;
                (int X, int Y) maxPos = (0, 0);

                for (int x = 0; x < (size - kernelSize - 1); x++)
                {
                    for (int y = 0; y < (size - kernelSize - 1); y++)
                    {
                        int pow = lastGrid[x, y];

                        int maxY = y + kernelSize - 1;
                        int maxX = x + kernelSize - 1;

                        for (int gy = y; gy < y + kernelSize; gy++) // right edge
                        {
                            pow += oneByOnePow[maxX, gy];
                        }

                        for (int gx = x; gx < x + kernelSize - 1; gx++) // bottom edge (minus lower-right corner)
                        {
                            pow += oneByOnePow[gx, maxY];
                        }

                        if (pow > max)
                        {
                            max = pow;
                            maxPos = (x, y);
                        }

                        thisGrid[x, y] = pow;
                    }
                }

                kernelMax[kernelSize] = max;
                kernelMaxPos[kernelSize] = maxPos;

                lastGrid = thisGrid;
            }

            int globalMax = kernelMax.Values.Max();
            int globalMaxSize = kernelMax.First(k => (k.Value == globalMax)).Key;

            var globalMaxPos = kernelMaxPos[globalMaxSize];

            string answer = (globalMaxPos.X + 1) + "," + (globalMaxPos.Y + 1) + "," + globalMaxSize;

            return 0;
        }
    }
}
