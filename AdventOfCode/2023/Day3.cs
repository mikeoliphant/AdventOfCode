using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day3 : Day
    {
        Grid<char> grid = new();

        int GetPartNum(int col, int row)
        {
            List<char> num = new List<char>();

            while ((col > 0) && char.IsDigit(grid[col - 1, row]))
                col--;

            for (; col < grid.Width; col++)
            {
                char c = grid[col, row];

                if (char.IsDigit(c))
                    num.Add(c);
                else
                    break;
            }

            return int.Parse(num.ToArray());
        }

        public override long Compute()
        {
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            //grid.PrintToConsole();

            List<(int Col, int Row)> parts = new();

            for (int row = 0; row < grid.Height; row++)
            {
                bool inPart = false;

                for (int col = 0; col < grid.Width; col++)
                {
                    if (char.IsDigit(grid[col, row]))
                    {
                        if (!inPart)
                        {
                            inPart = true;

                            parts.Add((col, row));
                        }
                    }
                    else
                    {
                        inPart = false;
                    }
                }
            }

            long sum = 0;

            foreach (var part in parts)
            {
                int partNum = 0;
                
                for (int col = part.Col; col < grid.Width; col++)
                {
                    if (!char.IsDigit(grid[col, part.Row]))
                        break;

                    foreach (char c in grid.ValidNeighborValues(col, part.Row, includeDiagonal: true))
                    {
                        if (!char.IsDigit(c) && (c != '.'))
                        {
                            partNum = GetPartNum(part.Col, part.Row);

                            break;
                        }
                    }

                    if (partNum != 0)
                        break;
                }

                if (partNum != 0)
                {
                    sum += partNum;
                }
            }

            return sum;
        }

        public override long Compute2()
        {
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            //grid.PrintToConsole();

            long sum = 0;

            foreach (var pos in grid.FindValue('*'))
            {
                Dictionary<int, bool> partDict = new Dictionary<int, bool>();

                foreach (var partPos in grid.ValidNeighbors(pos.X, pos.Y, includeDiagonal: true).Where(c => char.IsDigit(grid[c.X, c.Y])))
                {
                    int partNum = GetPartNum(partPos.X, partPos.Y);

                    partDict[partNum] = true;
                }

                if (partDict.Count == 2)
                {
                    long prod = 1;

                    foreach (int partNum in partDict.Keys)
                    {
                        prod *= partNum;
                    }

                    sum += prod;
                }
            }

            return sum;
        }
    }
}
