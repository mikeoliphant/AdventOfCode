using Microsoft.VisualBasic.FileIO;

namespace AdventOfCode._2022
{
    internal class Day23 : Day
    {
        string moves = "nswe";
        int currentMove = 0;
        int numElves;
        bool haveMove = false;

        SparseGrid<char> grid;
        SparseGrid<char> newGrid;
        SparseGrid<byte> grid2;

        void ReadInput(string file)
        {
            grid = new SparseGrid<char>().CreateDataFromRows(File.ReadLines(file));
            grid.DefaultValue = '.';

            foreach (var pos in grid.FindValue('.')) grid.RemoveValue(pos);

            newGrid = new SparseGrid<char>();
            grid2 = new SparseGrid<byte>();
            grid2.DefaultValue = 0;
        }

        void Update1((int X, int Y) pos)
        {
            if (!grid.ValidNeighborValues(pos.X, pos.Y, includeDiagonal: true).Where(g => g != '.').Any())
                return;

            int move = currentMove;

            var newPos = pos;

            do
            {
                switch (moves[move])
                {
                    case 'n':
                        if (!grid.GetValidRectangleValues(new Rectangle(pos.X - 1, pos.Y - 1, 3, 1)).Where(g => g != '.').Any())
                        {
                            newPos = (pos.X, pos.Y - 1);
                        }
                        break;
                    case 's':
                        if (!grid.GetValidRectangleValues(new Rectangle(pos.X - 1, pos.Y + 1, 3, 1)).Where(g => g != '.').Any())
                        {
                            newPos = (pos.X, pos.Y + 1);
                        }
                        break;
                    case 'w':
                        if (!grid.GetValidRectangleValues(new Rectangle(pos.X - 1, pos.Y - 1, 1, 3)).Where(g => g != '.').Any())
                        {
                            newPos = (pos.X - 1, pos.Y);
                        }
                        break;
                    case 'e':
                        if (!grid.GetValidRectangleValues(new Rectangle(pos.X + 1, pos.Y - 1, 1, 3)).Where(g => g != '.').Any())
                        {
                            newPos = (pos.X + 1, pos.Y);
                        }
                        break;
                }

                if (newPos != pos)
                {
                    grid[pos] = moves[move];
                    grid2.SetValue(newPos, (byte)(grid2.GetValue(newPos) + 1));

                    break;
                }

                move = (move + 1) % moves.Length;
            }
            while (move != currentMove);
        }

        void Update2((int X, int Y) pos)
        {
            var newPos = pos;

            switch (grid[pos])
            {
                case 'n':
                    if (grid2.GetValue(pos.X, pos.Y - 1) == 1)
                    {
                        newPos = (pos.X, pos.Y - 1);
                    }
                    break;
                case 's':
                    if (grid2.GetValue(pos.X, pos.Y + 1) == 1)
                    {
                        newPos = (pos.X, pos.Y + 1);
                    }
                    break;
                case 'w':
                    if (grid2.GetValue(pos.X - 1, pos.Y) == 1)
                    {
                        newPos = (pos.X - 1, pos.Y);
                    }
                    break;
                case 'e':
                    if (grid2.GetValue(pos.X + 1, pos.Y) == 1)
                    {
                        newPos = (pos.X + 1, pos.Y);
                    }
                    break;
            }

            if (newPos != pos)
            {
                haveMove = true;

                newGrid.RemoveValue(pos);
            }
            
            newGrid[newPos] = '#';
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            grid.PrintToConsole();

            numElves = grid.FindValue('#').Count();

            int round = 0;

            do
            {
                //Console.WriteLine("First move is: " + moves[currentMove]);

                grid2.Clear();

                foreach (var pos in grid.GetAll())
                {
                    Update1(pos);
                }

                //grid2.PrintToConsole();

                newGrid = new SparseGrid<char>(grid);

                //newGrid.PrintToConsole();

                haveMove = false;

                foreach (var pos in grid.GetAll())
                {
                    Update2(pos);
                }

                grid = newGrid;

                Console.WriteLine("End of round " + (round + 1));

                //grid.PrintToConsole();

                //Console.ReadLine();

                //if (grid.FindValue('#').Count() != numElves)
                //    throw new Exception();

                currentMove = (currentMove + 1) % moves.Length;

                round++;
            }
            //while (round < 10);
            while (haveMove);

            return round;

            //var bounds = grid.GetBounds();

            //int area = ((bounds.MaxX - bounds.MinX) + 1) * ((bounds.MaxY - bounds.MinY) + 1);

            //return area - grid.FindValue('#').Count();
        }
    }
}
