namespace AdventOfCode._2023
{
    internal class Day17 : Day
    {
        char[] facingChars = { '^', '>', 'v', '<' };

        Grid<char> grid = new Grid<char>();

        IEnumerable<KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>> GetNeighbors((LongVec2 Pos, int Facing, int Steps) state)
        {
            if (state.Steps < 3)
            {
                LongVec2 straight = state.Pos;
                straight.AddFacing(state.Facing, 1);

                if (grid.IsValid((int) straight.X, (int)straight.Y))
                    yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((straight, state.Facing, state.Steps + 1), grid[(int)straight.X, (int)straight.Y] - '0');
            }

            LongVec2 left = state.Pos;
            int facing = LongVec2.TurnFacing(state.Facing, -1);
            left.AddFacing(facing, 1);

            if (grid.IsValid((int)left.X, (int)left.Y))
                yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((left, facing, 1), grid[(int)left.X, (int)left.Y] - '0');

            LongVec2 right = state.Pos;
            facing = LongVec2.TurnFacing(state.Facing, 1);
            right.AddFacing(facing, 1);

            if (grid.IsValid((int)right.X, (int)right.Y))
                yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((right, facing, 1), grid[(int)right.X, (int)right.Y] - '0');
        }

        public override long Compute()
        {
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            DijkstraSearch<(LongVec2 Pos, int Facing, int Steps)> search = new DijkstraSearch<(LongVec2 Pos, int Facing, int Steps)>(GetNeighbors);

            var result = search.GetShortestPath((new LongVec2(0, 0), 2, 0), delegate ((LongVec2 Pos, int Facing, int Steps) state) { return ((state.Pos.X == (grid.Width - 1)) && (state.Pos.Y == (grid.Height - 1))); });

            foreach (var cell in result.Path)
            {
                grid[(int)cell.Pos.X, (int)cell.Pos.Y] = facingChars[cell.Facing];
            }

            //grid.PrintToConsole();

            return (long)result.Cost;
        }

        IEnumerable<KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>> GetNeighbors2((LongVec2 Pos, int Facing, int Steps) state)
        {
            if (state.Steps < 10)
            {
                LongVec2 straight = state.Pos;
                straight.AddFacing(state.Facing, 1);

                if (grid.IsValid((int)straight.X, (int)straight.Y))
                    yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((straight, state.Facing, state.Steps + 1), grid[(int)straight.X, (int)straight.Y] - '0');
            }

            if (state.Steps > 3)
            {
                LongVec2 left = state.Pos;
                int facing = LongVec2.TurnFacing(state.Facing, -1);
                left.AddFacing(facing, 1);

                if (grid.IsValid((int)left.X, (int)left.Y))
                    yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((left, facing, 1), grid[(int)left.X, (int)left.Y] - '0');

                LongVec2 right = state.Pos;
                facing = LongVec2.TurnFacing(state.Facing, 1);
                right.AddFacing(facing, 1);

                if (grid.IsValid((int)right.X, (int)right.Y))
                    yield return new KeyValuePair<(LongVec2 Pos, int Facing, int Steps), float>((right, facing, 1), grid[(int)right.X, (int)right.Y] - '0');
            }
        }

        public override long Compute2()
        {
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            DijkstraSearch<(LongVec2 Pos, int Facing, int Steps)> search = new DijkstraSearch<(LongVec2 Pos, int Facing, int Steps)>(GetNeighbors2);

            var result = search.GetShortestPath((new LongVec2(0, 0), 2, 0), delegate ((LongVec2 Pos, int Facing, int Steps) state) { return ((state.Pos.X == (grid.Width - 1)) && (state.Pos.Y == (grid.Height - 1)) && (state.Steps > 3)); });

            var result2 = search.GetShortestPath((new LongVec2(0, 0), 1, 0), delegate ((LongVec2 Pos, int Facing, int Steps) state) { return ((state.Pos.X == (grid.Width - 1)) && (state.Pos.Y == (grid.Height - 1)) && (state.Steps > 3)); });

            return (long)Math.Min(result.Cost, result2.Cost);
        }
    }
}
