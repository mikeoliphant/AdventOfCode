namespace AdventOfCode._2024
{
    internal class Day6 : Day
    {
        Grid<char> grid;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        IEnumerable<(int X, int Y)> GetVisited()
        {
            LongVec2 pos = grid.FindValue('^').Select(p => new LongVec2(p.X, p.Y)).First();
            int facing = 0;

            do
            {
                grid[(int)pos.X, (int)pos.Y] = 'X';

                LongVec2 next = pos;
                next.AddFacing(facing, 1);

                if (!grid.IsValid((int)next.X, (int)next.Y))
                    break;

                if (grid[(int)next.X, (int)next.Y] == '#')
                {
                    facing = LongVec2.TurnFacing(facing, 1);
                }
                else
                {
                    pos = next;
                }
            }
            while (true);

            return grid.FindValue('X');
        }

        bool IsLoop(LongVec2 pos, int facing)
        {
            HashSet<(LongVec2, int)> visited = new();

            do
            {
                grid[(int)pos.X, (int)pos.Y] = 'X';

                LongVec2 next = pos;
                next.AddFacing(facing, 1);

                if (visited.Contains((next, facing)))
                    return true;

                visited.Add((next, facing));

                if (!grid.IsValid((int)next.X, (int)next.Y))
                    break;

                if (grid[(int)next.X, (int)next.Y] == '#')
                {
                    facing = LongVec2.TurnFacing(facing, 1);
                }
                else
                {
                    pos = next;
                }
            }
            while (true);

            return false;
        }

        public override long Compute()
        {
            ReadData();


            //grid.PrintToConsole();

            long numVisited = GetVisited().Count();

            return numVisited;
        }

        public override long Compute2()
        {
            ReadData();

            //grid.PrintToConsole();

            var visited = GetVisited().ToList();

            ReadData();

            var start = grid.FindValue('^').First();

            visited.Remove(start);

            long loops = 0;

            foreach (var pos in visited)
            {
                grid[pos] = '#';

                if (IsLoop(new LongVec2(start), 0))
                {
                    loops++;
                }

                grid[pos] = '.';
            }

            return loops;
        }
    }
}
