namespace AdventOfCode._2018
{
    internal class Day18
    {
        Automata<char> automata = null;

        void ReadInput()
        {
            automata = new Automata<char>(new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day18.txt")));
            automata.CellUpdateFuntion = UpdateCell;
        }

        char UpdateCell((int X, int Y) pos, char c)
        {
            var neighbors = automata.Grid.ValidNeighborValues(pos.X, pos.Y, includeDiagonal: true);

            switch (c)
            {
                case '.':
                    if (neighbors.Count('|') > 2)
                        return '|';

                    return '.';

                case '|':
                    if (neighbors.Count('#') > 2)
                        return '#';

                    return '|';

                case '#':
                    if ((neighbors.Count('|') > 0) && (neighbors.Count(n => n == '#') > 0))
                        return '#';

                    return '.';
            }

            throw new InvalidOperationException();
        }

        public long Compute()
        {
            ReadInput();

            //grid.PrintToConsole();

            for (int cycle = 0; cycle < 10; cycle++)
            {
                automata.Cycle();

                //grid.PrintToConsole();
            }

            return automata.Grid.CountValue('|') * automata.Grid.CountValue('#');
        }

        public long Compute2()
        {
            ReadInput();

            int cyclePos;
            int loopSize;

            long maxCycle = 1000000000;

            automata.FindLoop(100000, out cyclePos, out loopSize, delegate { return (int)automata.Grid.CountValue('|') * (int)automata.Grid.CountValue('#');  }, 100);

            int offset = (int)(maxCycle - cyclePos) % loopSize;

            int dupeCycle = cyclePos + offset;

            automata.Reset();
            automata.Cycle(dupeCycle);

            return automata.Grid.CountValue('|') * automata.Grid.CountValue('#');
        }
    }
}
