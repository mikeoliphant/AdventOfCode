using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day17
    {
        Grid<char> grid = null;
        IntcodeComputer computer;

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day17.txt").ToLongs(',').ToArray();

            SparseGrid<char> sparseGrid = new SparseGrid<char>();

            computer = new IntcodeComputer();
            computer.SetProgram(program);

            int x = 0;
            int y = 0;

            while (computer.RunUntilOutput())
            {
                char c = (char)computer.GetLastOutput();

                if (c == 10)
                {
                    y++;
                    x = 0;
                }
                else
                {
                    sparseGrid[x, y] = c;

                    x++;
                }
            }

            grid = sparseGrid.ToGrid();
        }

        public long Compute()
        {
            ReadInput();

            grid.PrintToConsole();

            int alignment = 0;

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    int numScaffoldNeighbors = 0;

                    foreach (char c in grid.ValidNeighbors(x, y, includeDiagonal: false))
                    {
                        if (c != '.')
                            numScaffoldNeighbors++;
                    }

                    if (numScaffoldNeighbors == 4)
                    {
                        alignment += x * y;
                    }
                }
            }

            return alignment;
        }

        int CanMove(int x, int y, int dx, int dy)
        {
            int numMoves = 0;

            do
            {
                x += dx;
                y += dy;

                if (grid.GetValue(x, y) != '#')
                    break;

                numMoves++;
            }
            while (true);

            return numMoves;
        }

        List<ValueTuple<char, int>> GetPath(int x, int y, int dx, int dy)
        {
            List<ValueTuple<char, int>> path = new List<ValueTuple<char, int>>();

            do
            {
                // Rotate left
                int ndx = dy;
                int ndy = -dx;

                int numMoves = CanMove(x, y, ndx, ndy);

                if (numMoves > 0)
                {
                    path.Add(('L', numMoves));
                }
                else
                {
                    // Rotate right
                    ndx = -dy;
                    ndy = dx;

                    numMoves = CanMove(x, y, ndx, ndy);

                    if (numMoves == 0)
                        return path;

                    path.Add(('R', numMoves));
                }

                x += ndx * numMoves;
                y += ndy * numMoves;

                dx = ndx;
                dy = ndy;
            }
            while (true);
        }

        string inputStr;

        void AddInput(int input)
        {
            inputStr += (char)input;

            computer.AddInput(input);
        }

        public long Compute2()
        {
            ReadInput();

            grid.PrintToConsole();

            var startPos = grid.Find('^').First();

            var path = GetPath(startPos.Item1, startPos.Item2, 0, -1);

            string sequence = string.Join("", path.Select(p => p.Item1.ToString() + p.Item2.ToString()));

            string[] movementCmds = new string[] { "L12R4R4L6", "L12R4R4R12", "L10L6R4" };  // Just picked out by hand

            List<int> moveSequence = new List<int>();

            int pos = 0;

            while (pos < sequence.Length)
            {
                for (int i = 0; i < movementCmds.Length; i++)
                {
                    if (sequence.Skip(pos).Take(movementCmds[i].Length).SequenceEqual(movementCmds[i]))
                    {
                        moveSequence.Add(i);

                        pos += movementCmds[i].Length;

                        break;
                    }
                }
            }

            computer.Reset();

            computer.SetMemory(0, 2);

            // Main movement routine
            for (int i = 0; i < moveSequence.Count; i++)
            {
                if (i > 0)
                {
                    AddInput(',');
                }

                AddInput('A' + moveSequence[i]);
            }
            AddInput(10);

            // Movement functions
            for (int i = 0; i < movementCmds.Length; i++)
            {
                for (int cmdPos = 0; cmdPos < movementCmds[i].Length;)
                {
                    if (cmdPos > 0)
                    {
                        AddInput(',');
                    }

                    AddInput(movementCmds[i][cmdPos++]);

                    AddInput(',');

                    while ((cmdPos < movementCmds[i].Length) && char.IsDigit(movementCmds[i][cmdPos]))
                    {
                        AddInput(movementCmds[i][cmdPos++]);
                    }
                }

                AddInput(10);
            }

            AddInput('n'); // do we want video feed
            AddInput(10);

            //while (computer.RunUntilOutput())
            //    Console.Write((char)computer.GetLastOutput());

            computer.RunUntilHalt();

            long dust = computer.GetLastOutput();

            return dust;
        }
    }
}
