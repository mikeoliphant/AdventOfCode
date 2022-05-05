namespace AdventOfCode._2017
{
    internal class Day19
    {
        Grid<char> grid = null;

        int numSteps = 0;

        public long Compute()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day19.txt"));

            int x = 0;
            int y = 0;

            for (; x < grid.Width; x++)
            {
                if (grid[x, y] == '|')
                {
                    break;
                }
            }

            int dy = 1;
            int dx = 0;

            string path = "";

            bool finished = false;

            do
            {
                switch (grid[x, y])
                {
                    case '|':
                        break;
                    case '-':
                        break;

                    case '+':
                        if (dy != 0)
                        {
                            dy = 0;

                            dx = (grid[x - 1, y] == ' ') ? 1 : -1;
                        }
                        else
                        {
                            dx = 0;

                            dy = (grid[x, y - 1] == ' ') ? 1 : -1;
                        }
                        break;

                        x += dx;
                        break;

                    case ' ':
                        finished = true;
                        break;

                    default:
                        path += grid[x, y];
                        break;
                }

                x += dx;
                y += dy;

                numSteps++;
            }
            while (!finished);

            return numSteps - 1;
        }
    }
}
