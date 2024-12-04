namespace AdventOfCode._2024
{
    internal class Day4 : Day
    {
        Grid<char> grid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        bool Match(ReadOnlySpan<char> text, (int X, int Y) pos, (int X, int Y) dir)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!grid.IsValid(pos))
                    return false;

                if (grid[pos] != text[i])
                    return false;

                pos = (pos.X + dir.X, pos.Y + dir.Y);
            }

            return true;
        }

        long NumMatches(ReadOnlySpan<char> text, (int X, int Y) pos)
        {
            long matches = 0;

            if (grid[pos] == text[0])
            {
                foreach (var neighbor in grid.ValidNeighbors(pos.X, pos.Y, includeDiagonal: true))
                {
                    var dir = ((neighbor.X - pos.X), (neighbor.Y - pos.Y));

                    if (Match(text.Slice(1), neighbor, dir))
                        matches++;
                }
            }

            return matches;
        }

        public override long Compute()
        {
            ReadData();

            //grid.PrintToConsole();

            long count = 0;

            foreach (var pos in grid.GetAll())
            {
                count += NumMatches("XMAS", pos);
            }

            return count;
        }

        bool MatchMAS((int X, int Y) pos)
        {
            if (grid[pos] != 'A')
                return false;

            int matches = 0;

            foreach (var neighbor in grid.ValidDiagonalNeighbors(pos.X, pos.Y))
            {
                var dir = ((pos.X - neighbor.X), (pos.Y - neighbor.Y));

                if (Match("MAS", neighbor, dir))
                    matches++;

                if (matches == 2)
                    return true;
            }

            return false;
        }

        public override long Compute2()
        {
            ReadData();

            long count = 0;

            foreach (var pos in grid.GetAll())
            {
                if (MatchMAS(pos))
                    count++;
            }

            return count;
        }
    }
}
