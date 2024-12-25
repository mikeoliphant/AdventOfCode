namespace AdventOfCode._2024
{
    internal class Day25 : Day
    {
        List<List<int>> locks = new();
        List<List<int>> keys = new();

        void ReadData()
        {
            foreach (string pattern in File.ReadAllText(DataFile).SplitParagraphs())
            {
                var grid = new Grid<char>().CreateDataFromRows(pattern.SplitLines());

                List<int> pins = new();

                for (int x = 0; x < grid.Width; x++)
                {
                    int count = 0;

                    for (int y = 0; y < grid.Height; y++)
                    {
                        if (grid[x, y] == '#')
                            count++;
                    }

                    pins.Add(count - 1);
                }

                if (grid[0, 0] == '#')
                    locks.Add(pins);
                else
                    keys.Add(pins);
            }
        }

        bool Fits(List<int> l, List<int> k)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if ((l[i] + k[i]) > 5)
                    return false;
            }

            return true;
        }

        public override long Compute()
        {
            ReadData();

            long count = 0;

            foreach (var l in locks)
            {
                foreach (var k in keys)
                {
                    if (Fits(l, k))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
