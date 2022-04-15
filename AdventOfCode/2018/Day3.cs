namespace AdventOfCode._2018
{
    internal class Day3
    {
        SparseGrid<int> grid = new SparseGrid<int>();
        List<Rectangle> claims = new List<Rectangle>();

        void ReadInput()
        {
            foreach (string claimStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day3.txt"))
            {
                string[] words = claimStr.Split(' ');

                int[] xy = words[2].Replace(":", "").ToInts(',').ToArray();
                int[] size = words[3].ToInts('x').ToArray();

                Rectangle claim = new Rectangle(xy[0], xy[1], size[0], size[1]);

                claims.Add(claim);

                foreach (var pos in grid.GetRectangle(claim))
                {
                    int currentVal = 0;

                    grid.TryGetValue(pos.X, pos.Y, out currentVal);

                    grid[pos.X, pos.Y] = currentVal + 1;
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            return grid.GetAllValues().Count(g => g > 1);
        }

        public long Compute2()
        {
            ReadInput();

            for (int pos = 0; pos < claims.Count; pos++)
            {
                if (grid.GetRectangleValues(claims[pos]).Count(g => g == 1) == (claims[pos].Width * claims[pos].Height))
                {
                    return pos + 1;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
