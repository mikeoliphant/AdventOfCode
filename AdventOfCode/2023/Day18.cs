using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2023
{
    internal class Day18 : Day
    {
        void FloodFill(GridBase<char> grid, (int X, int Y) pos)
        {
            Stack<(int X, int Y)> cellStack = new();

            cellStack.Push(pos);

            while (cellStack.Count > 0)
            {
                var cell = cellStack.Pop();

                if (grid.GetValue(cell) == '.')
                {
                    grid[cell] = '#';

                    foreach (var neighbor in Grid.AllNeighbors(cell.X, cell.Y, includeDiagonal: false))
                    {
                        cellStack.Push(neighbor);
                    }
                }
            }
        }

        IEnumerable<(int DX, int DY, int Dist)> GetInstructions(string dataFile)
        {
            foreach (string instruction in File.ReadLines(dataFile))
            {
                string[] split = instruction.Split(' ');

                int dx = 0;
                int dy = 0;

                switch (split[0][0])
                {
                    case 'U':
                        dy = -1;
                        break;
                    case 'D':
                        dy = 1;
                        break;
                    case 'L':
                        dx = -1;
                        break;
                    case 'R':
                        dx = 1;
                        break;
                }

                yield return (dx, dy, int.Parse(split[1]));
            }
        }

        public override long Compute()
        {
            SparseGrid<char> grid = new();
            grid.DefaultValue = '.';

            (int X, int Y) cell = (0, 0);

            grid[cell] = '#';

            var instructions = GetInstructions(DataFile);

            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Dist; i++)
                {
                    cell = (cell.X + instruction.DX, cell.Y + instruction.DY);

                    grid[cell] = '#';
                }
            }

            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Dist; i++)
                {
                    cell = (cell.X + instruction.DX, cell.Y + instruction.DY);

                    var flood = (cell.X - instruction.DY, cell.Y + instruction.DX);

                    FloodFill(grid, flood);
                }
            }

            Grid<char> dugGrid = grid.ToGrid();

            GridDisplay<char> display = new GridDisplay<char>(dugGrid, dugGrid.Width, dugGrid.Height);

            long count = dugGrid.FindValue('#').LongCount();

            return count;
        }


        IEnumerable<(int DX, int DY, int Dist)> GetInstructions2(string dataFile)
        {
            foreach (string instruction in File.ReadLines(dataFile))
            {
                string[] split = instruction.Split(' ');

                int dx = 0;
                int dy = 0;

                switch (split[2][7])
                {
                    case '3':
                        dy = -1;
                        break;
                    case '1':
                        dy = 1;
                        break;
                    case '2':
                        dx = -1;
                        break;
                    case '0':
                        dx = 1;
                        break;
                }

                yield return (dx, dy, int.Parse(split[2].Substring(2, 5), System.Globalization.NumberStyles.HexNumber));
            }
        }

        public override long Compute2()
        {
            SparseGrid<char> grid = new();
            grid.DefaultValue = '.';

            var instructions = GetInstructions(DataFileTest);

            Dictionary<(int X, int Y), int> hDict = new();

            List<(int X, int Y, int Height)> vertList = new();

            (int X, int Y) cell = (0, 0);

            foreach (var instruction in instructions)
            {
                if (instruction.DY != 0)
                {
                    int ny = instruction.DY * instruction.Dist;

                    if (ny > 0)
                        vertList.Add((cell.X, cell.Y, ny));
                    else
                        vertList.Add((cell.X, cell.Y + ny, -ny));

                    cell = (cell.X, cell.Y + ny);
                }
                else
                {
                    int nx = instruction.DX * instruction.Dist;

                    if (nx > 0)
                        hDict[(cell.X, cell.Y)] = nx;
                    else
                        hDict[(cell.X + nx, cell.Y)] = -nx;

                    cell = (cell.X + nx, cell.Y);
                }

                grid[cell] = '#';
            }

            var rowsOfInterest = hDict.Keys.Select(h => h.Y).Distinct().Order();

            int? lastRow = null;

            long totInterior = 0;

            int numInterior = 0;

            foreach (int row in rowsOfInterest)
            {
                if (lastRow.HasValue)
                {
                    totInterior += (row - lastRow.Value) * numInterior;

                    numInterior = 0;
                }

                lastRow = row;

                var cols = vertList.Where(v => (row >= v.Y) && (row < (v.Y + v.Height))).Select(v => v.X).Order();

                int? lastCol = null;

                foreach (int col in cols)
                {
                    if (lastCol.HasValue)
                    {
                        int diff = col - lastCol.Value;

                        numInterior += (diff + 1);

                        lastCol = null;
                    }
                    else
                    {
                        lastCol = col;
                    }
                }
            }

            grid.PrintToConsole();

            return 0;
        }
    }
}
