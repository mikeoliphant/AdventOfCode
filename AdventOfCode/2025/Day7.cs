
namespace AdventOfCode._2025
{
    internal class Day7 : Day
    {
        public override long Compute()
        {
            Grid<char> grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            var startPos = grid.FindValue('S').FirstOrDefault();

            HashSet<GridPos> beams = new();

            beams.Add(startPos);

            long numSplits = 0;

            do
            {
                HashSet<GridPos> newBeams = new();

                foreach (var beam in beams)
                {
                    GridPos newPos = new(beam.X, beam.Y + 1);

                    if (grid[newPos] == '^')
                    {
                        newBeams.Add(new GridPos(newPos.X - 1, newPos.Y));
                        newBeams.Add(new GridPos(newPos.X + 1, newPos.Y));

                        numSplits++;
                    }
                    else
                        newBeams.Add(newPos);
                }

                beams = newBeams;

                if (beams.First().Y == (grid.Height - 1))
                    break;
            }
            while (true);

            return numSplits;
        }

        void Increment(Dictionary<GridPos, long> dict, GridPos pos, long num)
        {
            if (!dict.ContainsKey(pos))
                dict[pos] = num;
            else
                dict[pos] += num;
        }

        public override long Compute2()
        {
            Grid<char> grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            var startPos = grid.FindValue('S').FirstOrDefault();

            Dictionary<GridPos, long> beams = new();

            beams[startPos] = 1;

            do
            {
                Dictionary<GridPos, long> newBeams = new();

                foreach ((var beam, var count) in beams)
                {
                    GridPos newPos = new(beam.X, beam.Y + 1);

                    if (grid[newPos] == '^')
                    {
                        Increment(newBeams, new GridPos(newPos.X - 1, newPos.Y), count);
                        Increment(newBeams, new GridPos(newPos.X + 1, newPos.Y), count);
                    }
                    else
                        Increment(newBeams, newPos, count);
                }

                beams = newBeams;

                if (beams.First().Key.Y == (grid.Height - 1))
                    break;
            }
            while (true);

            return beams.Select(b => b.Value).Sum();
        }
    }
}
