namespace AdventOfCode._2017
{
    internal class Day3
    {
        long GetDist(long startPos)
        {
            long size = 1;

            while ((size * size) <= startPos)
            {
                size += 2;
            }

            long xPos = (size - 1) / 2;
            long yPos = xPos;

            long value = size * size;

            for (int i = 0; i < (size - 1); i++)
            {
                if (value == startPos)
                    break;

                value--;
                xPos--;
            }

            for (int i = 0; i < (size - 1); i++)
            {
                if (value == startPos)
                    break;

                value--;
                yPos--;
            }

            for (int i = 0; i < (size - 1); i++)
            {
                if (value == startPos)
                    break;

                value--;
                xPos++;
            }

            for (int i = 0; i < (size - 1); i++)
            {
                if (value == startPos)
                    break;

                value--;
                yPos++;
            }

            return Math.Abs(xPos) + Math.Abs(yPos);
        }

        public long Compute()
        {
            return GetDist(265149);
        }

        public long Compute2()
        {
            SparseGrid<int> grid = new SparseGrid<int>();

            grid[0, 0] = 1;

            int xPos = 1;
            int yPos = 1;
            int size = 3;

            int dx = 0;
            int dy = -1;

            do
            {
                do
                {
                    for (int i = 0; i < (size - 1); i++)
                    {
                        xPos += dx;
                        yPos += dy;

                        grid[xPos, yPos] = grid.ValidNeighborValues(xPos, yPos, includeDiagonal: true).Sum();

                        if (grid[xPos, yPos] > 265149)
                            return grid[xPos, yPos];
                    }

                    int tmp = dx;
                    dx = dy;
                    dy = -tmp;
                }
                while (dy != -1);

                xPos++;
                yPos++;
                size += 2;
            }
            while (true);

            throw new InvalidOperationException();
        }
    }
}
