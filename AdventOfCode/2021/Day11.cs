using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class Day11
    {
        int width;
        int height;
        int[,] grid;

        long flashCount = 0;

        void ReadInput()
        {
            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day11.txt").ToArray();

            width = lines[0].Length;
            height = lines.Length;

            grid = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = lines[y][x] - '0';
                }
            }
        }

        int GetGrid(int x, int y)
        {
            if ((x < 0) || (x >= width) || (y < 0) || (y >= height))
                return 10;

            return grid[x, y];
        }

        void Cycle()
        {
            bool steady = true;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] += 1;

                    if (grid[x, y] > 9)
                    {
                        grid[x, y] = 0;

                        flashCount++;
                    }
                }
            }

            do
            {
                steady = true;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int neighborFlashes = 0;

                        for (int dy = -1; dy <= 1; dy++)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                if (((dx != 0) || (dy != 0)) && (GetGrid(x + dx, y + dy) == 0))
                                {
                                    neighborFlashes++;
                                }
                            }
                        }

                        if ((grid[x, y] + neighborFlashes) > 9)
                        {
                            flashCount++;
                            steady = false;

                            grid[x, y] = 0;
                        }
                    }
                }
            }
            while (!steady);

            int[,] newGrid = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int neighborFlashes = 0;

                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (((dx != 0) || (dy != 0)) && (GetGrid(x + dx, y + dy) == 0))
                            {
                                neighborFlashes++;
                            }
                        }
                    }

                    if (grid[x, y] != 0)
                        newGrid[x, y] = grid[x, y] + neighborFlashes;
                }
            }

            grid = newGrid;
        }

        void PrintGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(grid[x, y].ToString());
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public long Compute()
        {
            ReadInput();

            //PrintGrid();

            for (int cycle = 0; cycle < 100; cycle++)
            {
                Cycle();

                //PrintGrid();
            }

            return flashCount;
        }

        bool AllZeros()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y] != 0)
                        return false;
                }
            }

            return true;
        }

        public long Compute2()
        {
            ReadInput();

            long numCycles = 0;

            do
            {
                Cycle();

                numCycles++;
            }
            while (!AllZeros());

            return numCycles;
        }
    }
}
