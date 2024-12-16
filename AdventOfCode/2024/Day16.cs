namespace AdventOfCode._2024
{
    using State = ((int X, int Y) Pos, int Facing);

    internal class Day16 : Day
    {
        Grid<char> grid = null;
        Random random = new Random();

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            bool blah = (1, 0) == new IntVec2(1, 0);
        }

        IEnumerable<KeyValuePair<State, float>> GetNeighbors(State state)
        {
            if (random.NextDouble() < 0.5)
            {
                // Turn right
                yield return new KeyValuePair<State, float>((state.Pos, IntVec2.TurnFacing(state.Facing, 1)), 1000);

                // Turn left
                yield return new KeyValuePair<State, float>((state.Pos, IntVec2.TurnFacing(state.Facing, -1)), 1000);
            }
            else
            {
                // Turn left
                yield return new KeyValuePair<State, float>((state.Pos, IntVec2.TurnFacing(state.Facing, -1)), 1000);

                // Turn right
                yield return new KeyValuePair<State, float>((state.Pos, IntVec2.TurnFacing(state.Facing, 1)), 1000);
            }

            var next = new IntVec2(state.Pos);
            next.AddFacing(state.Facing, 1);

            if (grid.IsValid(next.X, next.Y) && (grid[next.X, next.Y] != '#'))
                yield return new KeyValuePair<State, float>(((next.X, next.Y), state.Facing), 1);
        }

        public override long Compute()
        {
            ReadData();

            grid.PrintToConsole();

            var start = grid.FindValue('S').First();
            var end = grid.FindValue('E').First();

            var search = new DijkstraSearch<State>(GetNeighbors);

            var path = search.GetShortestPath((start, 1),
                delegate (State state)
                {
                    return state.Pos == end;
                });

            return (long)path.Cost;
        }

        public override long Compute2()
        {
            ReadData();

            grid.PrintToConsole();

            var start = grid.FindValue('S').First();
            var end = grid.FindValue('E').First();

            var search = new DijkstraSearch<State>(GetNeighbors);

            int count = 0;

            while (count < 100)
            {
                var path = search.GetShortestPath((start, 1),
                    delegate (State state)
                    {
                        return state.Pos == end;
                    });

                foreach (var state in path.Path)
                {
                    if (grid[state.Pos] == '.')
                    {
                        grid[state.Pos] = 'O';

                        count = 0;
                    }
                }

                count++;
            }

            int numTiles = grid.FindValue('O').Count() + 2;

            return numTiles;
        }
    }
}
