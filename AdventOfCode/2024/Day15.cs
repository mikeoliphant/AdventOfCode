namespace AdventOfCode._2024
{
    internal class Day15 : Day
    {
        Grid<char> grid = null;
        string moves = null;

        void ReadData()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            grid = new Grid<char>().CreateDataFromRows(split[0].SplitLines());

            moves = Regex.Replace(split[1], "[\r\n]", "");
        }

        bool CanMove(IntVec2 pos, IntVec2 dir, bool checkBox = true)
        {
            var newPos = pos + dir;

            if (checkBox && (dir.Y != 0))
            {
                char c = grid[pos.X, pos.Y];

                if (c == '[')
                {
                    if (!CanMove(new IntVec2(pos.X + 1, pos.Y), dir, checkBox: false))
                        return false;
                }
                else if (c== ']')
                {
                    if (!CanMove(new IntVec2(pos.X - 1, pos.Y), dir, checkBox: false))
                        return false;
                }
            }

            if (grid[newPos.X, newPos.Y] == '#')
                return false;

            return (grid[newPos.X, newPos.Y] == '.') || CanMove(newPos, dir);
        }

        void DoMove(IntVec2 pos, IntVec2 dir, bool checkBox = true)
        {
            var newPos = pos + dir;

            char c = grid[pos.X, pos.Y];

            if (checkBox && (dir.Y != 0))
            {
                if (c == '[')
                {
                    DoMove(new IntVec2(pos.X + 1, pos.Y), dir, checkBox: false);
                }
                else if (c == ']')
                {
                    DoMove(new IntVec2(pos.X - 1, pos.Y), dir, checkBox: false);
                }
            }

            char newC = grid[newPos.X, newPos.Y];

            if ((newC == 'O') || (newC == '[') || (newC == ']'))
                DoMove(newPos, dir);

            grid[newPos.X, newPos.Y] = grid[pos.X, pos.Y];
            grid[pos.X, pos.Y] = '.';
        }

        void DoMoves()
        {
            var pos = new IntVec2(grid.FindValue('@').First());

            foreach (char move in moves)
            {
                //Console.WriteLine("Move: " + move);

                IntVec2 dir = IntVec2.GetFacingVector(move);

                if (CanMove(pos, dir))
                {
                    DoMove(pos, dir);

                    pos += dir;
                }

                //grid.PrintToConsole();
            }
        }

        Grid<char> GetBigGrid()
        {
            var bigGrid = new Grid<char>(grid.Width * 2, grid.Height);

            foreach (var pos in grid.GetAll())
            {
                char c1;
                char c2;

                switch (grid[pos])
                {
                    case '@':
                        c1 = '@';
                        c2 = '.';
                        break;

                    case 'O':
                        c1 = '[';
                        c2 = ']';
                        break;

                    default:
                        c1 = c2 = grid[pos];
                        break;
                }

                bigGrid[pos.X * 2, pos.Y] = c1;
                bigGrid[(pos.X * 2) + 1, pos.Y] = c2;
            }

            return bigGrid;
        }

        public override long Compute()
        {
            ReadData();

            grid.PrintToConsole();

            DoMoves();

            long sum = 0;

            foreach (var box in grid.FindValue('O'))
            {
                sum += (box.Y * 100) + box.X;
            }

            return sum;
        }

        public override long Compute2()
        {
            ReadData();

            grid = GetBigGrid();

            grid.PrintToConsole();

            DoMoves();

            long sum = 0;

            foreach (var box in grid.FindValue('['))
            {
                sum += (box.Y * 100) + box.X;
            }

            return sum;
        }
    }
}
