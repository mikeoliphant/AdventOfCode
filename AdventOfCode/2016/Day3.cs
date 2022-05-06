namespace AdventOfCode._2016
{
    internal class Day3
    {

        public long Compute()
        {
            int valid = 0;

            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day3.txt"))
            {
                int[] sides = line.SplitWhitespace().ToInts().OrderBy(s => s).ToArray();

                if ((sides[0] + sides[1]) > sides[2])
                {
                    valid++;
                }
            }

            return valid;
        }

        IEnumerable<int[]> GetTriangles()
        {
            Grid<int> grid = new Grid<int>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day3.txt").Select(l => l.SplitWhitespace().ToInts()));

            for (int row = 0; row < grid.Height; row += 3)
            {
                for (int col = 0; col < grid.Width; col++)
                {
                    yield return new int[] { grid[col, row], grid[col, row + 1], grid[col, row + 2] };
                }
            }

        }

        public long Compute2()
        {
            int valid = 0;


            foreach (var triangle in GetTriangles())
            {
                int[] sides = triangle.OrderBy(s => s).ToArray();

                if ((sides[0] + sides[1]) > sides[2])
                {
                    valid++;
                }
            }

            return valid;
        }
    }
}
