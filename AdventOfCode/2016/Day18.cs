namespace AdventOfCode._2016
{
    internal class Day18
    {
        SparseGrid<char> grid = new SparseGrid<char>();

        int length = 0;

        public long Compute()
        {
            //string firstRow = ".^^.^.^^^^";

            string firstRow = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2016\Day18.txt").Trim();

            length = firstRow.Length;

            for (int i = 0; i < length; i++)
                grid[i, 0] = firstRow[i];

            for (int y = 1; y < 400000; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    bool l = grid.GetValue(x - 1, y - 1) == '^';
                    bool c = grid.GetValue(x, y - 1) == '^';
                    bool r = grid.GetValue(x + 1, y - 1) == '^';

                    grid[x, y] = ((l && c && !r) || (c && r && !l) || (l && !c && !r) || (r && !c && !l)) ? '^' : '.';
                }
            }

            //grid.PrintToConsole();

            return grid.CountValue('.');
        }
    }
}
