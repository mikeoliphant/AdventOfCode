namespace AdventOfCode._2017
{
    internal class Day22
    {
        SparseGrid<char> grid;
        int xPos;
        int yPos;
        int dir = 0;

        void ReadInput()
        {
            grid = new SparseGrid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day22.txt"));

            int minX;
            int maxX;
            int minY;
            int maxY;

            grid.GetBounds(out minX, out minY, out maxX, out maxY);

            xPos = (minX + maxX) / 2;
            yPos = (minY + maxY) / 2;
        }

        public long Compute()
        {
            ReadInput();

            int numInfections = 0;

            for (int i = 0; i < 10000; i++)
            {
                char val = '\0';

                grid.TryGetValue(xPos, yPos, out val);

                if (val == '#')
                {
                    dir = (dir + 1) % 4;

                    grid[xPos, yPos] = '.';
                }
                else
                {
                    dir = dir - 1;

                    if (dir == -1)
                        dir = 3;

                    grid[xPos, yPos] = '#';

                    numInfections++;
                }

                switch (dir)
                {
                    case 0: // up
                        yPos--;
                        break;

                    case 1: // right
                        xPos++;
                        break;

                    case 2: // down
                        yPos++;
                        break;

                    case 3: // left
                        xPos--;
                        break;
                }

            }

            return numInfections;
        }

        public long Compute2()
        {
            ReadInput();

            int numInfections = 0;

            for (int i = 0; i < 10000000; i++)
            {
                char val = '\0';

                grid.TryGetValue(xPos, yPos, out val);

                if (val == '#')
                {
                    dir = (dir + 1) % 4;

                    grid[xPos, yPos] = 'F';
                }
                else if (val == 'W')
                {
                    grid[xPos, yPos] = '#';

                    numInfections++;
                }
                else if (val == 'F')
                {
                    dir = (dir + 2) % 4;

                    grid[xPos, yPos] = '.';
                }
                else
                {
                    dir = dir - 1;

                    if (dir == -1)
                        dir = 3;

                    grid[xPos, yPos] = 'W';
                }

                switch (dir)
                {
                    case 0: // up
                        yPos--;
                        break;

                    case 1: // right
                        xPos++;
                        break;

                    case 2: // down
                        yPos++;
                        break;

                    case 3: // left
                        xPos--;
                        break;
                }

            }

            return numInfections;
        }
    }
}
