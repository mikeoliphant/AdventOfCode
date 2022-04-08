using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day13
    {
        SparseGrid<char> grid = new SparseGrid<char>();

        char[] blocks = new char[] { ' ', '#', '=', '_', 'o' };

        int ballX = 0;
        int paddleX = 0;
        long score = 0;

        public long Compute()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day13.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.SetProgram(program);
            computer.SetMemory(0, 2);

            computer.InputFunc = delegate
            {
                return Math.Sign(ballX - paddleX);
            };

            do
            {
                if (!computer.RunUntilOutput())
                    break;

                int x = (int)computer.GetLastOutput();

                computer.RunUntilOutput();

                int y = (int)computer.GetLastOutput();

                computer.RunUntilOutput();

                long block = computer.GetLastOutput();

                if ((x == -1) && (y == 0))
                {
                    score = block;
                }
                else
                {
                    grid[x, y] = blocks[block];

                    if (block == 3)
                    {
                        paddleX = x;
                    }
                    else if (block == 4)
                    {
                        ballX = x;

                        //Console.Clear();
                        //grid.PrintToConsole();
                    }
                }
            }
            while (true);

            return score; // grid.GetAllValues().Count(block => block == '=');
        }
    }
}
