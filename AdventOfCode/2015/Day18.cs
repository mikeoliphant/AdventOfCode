namespace AdventOfCode._2015
{
    internal class Day18 : Day
    {
        Automata<char> grid;

        public override long Compute()
        {
            grid = new Automata<char>(new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile)));

            grid.CellUpdateFunction = delegate ((int X, int Y) pos, char c)
            {
                int onNeighbors = grid.Grid.ValidNeighborValues(pos.X, pos.Y, includeDiagonal: true).Where(g => g == '#').Count();

                if (c == '#')
                {
                    return ((onNeighbors == 2) || (onNeighbors == 3)) ? '#' : '.';
                }
                else
                {
                    return (onNeighbors == 3) ? '#' : '.';
                }
            };

            for (int cycle = 0; cycle < 100; cycle++)
                grid.Cycle();

            return grid.Grid.CountValue('#');
        }

        public override long Compute2()
        {
            grid = new Automata<char>(new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile)));

            grid.Grid[0, 0] = '*';
            grid.Grid[0, 99] = '*';
            grid.Grid[99, 0] = '*';
            grid.Grid[99, 99] = '*';

            //grid.PrintToConsole();

            grid.CellUpdateFunction = delegate ((int X, int Y) pos, char c)
            {                
                int onNeighbors = grid.Grid.ValidNeighborValues(pos.X, pos.Y, includeDiagonal: true).Where(g => g != '.').Count();

                if (c == '#')
                {
                    return ((onNeighbors == 2) || (onNeighbors == 3)) ? '#' : '.';
                }
                else if (c == '.')
                {
                    return (onNeighbors == 3) ? '#' : '.';
                }
                else
                {
                    return '*';
                }
            };

            for (int cycle = 0; cycle < 100; cycle++)
                grid.Cycle();

            return grid.Grid.CountValue('#') + grid.Grid.CountValue('*');
        }
    }
}
