using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2019
{
    internal class Day3
    {
        SparseGrid<int> wire1Points = new SparseGrid<int>();

        int x;
        int y;

        int wireDist;

        int minDist = int.MaxValue;
        int minSteps = int.MaxValue;

        bool isWire2 = false;

        void DrawLine(int dx, int dy, int length)
        {
            for (int i = 0; i < length; i++)
            {
                x += dx;
                y += dy;
                wireDist++;

                if (isWire2)
                {
                    int wire1Value;

                    if (wire1Points.TryGetValue(x, y, out wire1Value))
                    {
                        int dist = Math.Abs(x) + Math.Abs(y);

                        if (dist < minDist)
                            minDist = dist;

                        int steps = wire1Value + wireDist;

                        if (steps < minSteps)
                            minSteps = steps;
                    }
                }
                else
                {
                    wire1Points[x, y] = wireDist;
                }
            }
        }

        void DrawWire(string wire)
        {
            string[] path = wire.Split(',');

            wireDist = 0;

            x = 0;
            y = 0;

            foreach (string cmd in path)
            {
                char dir = cmd[0];
                int length = int.Parse(cmd.Substring(1));

                switch (dir)
                {
                    case 'U':
                        DrawLine(0, -1, length);
                        break;

                    case 'D':
                        DrawLine(0, 1, length);
                        break;

                    case 'L':
                        DrawLine(-1, 0, length);
                        break;

                    case 'R':
                        DrawLine(1, 0, length);
                        break;
                }
            }
        }

        public long Compute()
        {
            string[] wires = File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day3.txt").ToArray();

            isWire2 = false;
            DrawWire(wires[0]);

            isWire2 = true;
            DrawWire(wires[1]);

            return minDist;
        }

        public long Compute2()
        {
            string[] wires = File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day3.txt").ToArray();

            isWire2 = false;
            DrawWire(wires[0]);

            isWire2 = true;
            DrawWire(wires[1]);

            return minSteps;
        }
    }
}
