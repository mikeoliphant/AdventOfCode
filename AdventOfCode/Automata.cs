namespace AdventOfCode
{
    public class Automata<T>
    {
        public GridBase<T> Grid { get; protected set; }
        public GridBase<T> StartGrid { get; protected set; }
        public Func<(int X, int Y), T, T> CellUpdateFunction { get; set; }

        public Automata(GridBase<T> grid)
        {
            this.Grid = grid;
            this.StartGrid = grid;
        }

        public void Reset()
        {
            Grid = StartGrid;
        }

        public void Cycle(int numCycles)
        {
            for (int cycle = 0; cycle < numCycles; cycle++)
                Cycle();
        }

        public virtual void Cycle()
        {
            GridBase<T> newGrid = Grid.CloneEmpty();

            foreach (var pos in Grid.GetAll())
            {
                newGrid[pos] = CellUpdateFunction(pos, Grid[pos]);
            }

            Grid = newGrid;
        }

        public bool FindLoop(int maxCycle, out int cyclePos, out int loopSize, Func<int> hashFunction, int numHitsInARow)
        {
            Dictionary<int, int> history = new Dictionary<int, int>();

            int histInARow = 0;

            for (int cycle = 0; cycle < maxCycle; cycle++)
            {
                int hash = hashFunction();

                if (history.ContainsKey(hash))
                {
                    histInARow++;

                    if (histInARow == numHitsInARow)
                    {
                        loopSize = cycle - history[hash];

                        cyclePos = cycle;

                        return true;
                    }
                }
                else
                {
                    histInARow = 0;
                }

                history[hash] = cycle;

                Cycle();
            }

            cyclePos = 0;
            loopSize = 0;

            return false;
        }

        public void PrintToConsole()
        {
            Grid.PrintToConsole();
        }
    }
}
