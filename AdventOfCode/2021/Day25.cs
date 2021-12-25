using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode._2021
{
    internal class Day25
    {
        Grid<char> grid;

        void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day25.txt"));
        }

        bool Move()
        {
            Grid<char> startGrid = grid;

            Grid<char> newGrid = new Grid<char>(grid.Width, grid.Height);
            newGrid.Fill('.');

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == '>')
                    {
                        int destX = (x + 1) % grid.Width;

                        if (grid[destX, y] == '.')
                        {
                            newGrid[destX, y] = '>';
                            newGrid[x, y] = '.';
                        }
                        else
                        {
                            newGrid[x, y] = '>';
                        }
                    }
                    else if (grid[x, y] != '.')
                    {
                        newGrid[x, y] = grid[x, y];
                    }
                }
            }

            grid = newGrid;
            newGrid = new Grid<char>(grid.Width, grid.Height);
            newGrid.Fill('.');

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == 'v')
                    {
                        int destY = (y + 1) % grid.Height;

                        if (grid[x, destY] == '.')
                        {
                            newGrid[x, destY] = 'v';
                            newGrid[x, y] = '.';
                        }
                        else
                        {
                            newGrid[x, y] = 'v';
                        }
                    }
                    else if (grid[x, y] != '.')
                    {
                        newGrid[x, y] = grid[x, y];
                    }
                }
            }

            grid = newGrid;

            return grid.Equals(startGrid);
        }

        public long Compute()
        {
            ReadInput();

            //grid.PrintToConsole();

            int step = 0;

            do
            {
                step++;

                if (Move())
                    break;

                //grid.PrintToConsole();
            }
            while (true);

            return step;
        }
    }
}
