using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day9
    {
        public long Compute()
        {
            long[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day9.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.RunProgram(input, 1);

            return computer.GetLastOutput();
        }

        public long Compute2()
        {
            long[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day9.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.RunProgram(input, 2);

            return computer.GetLastOutput();
        }
    }
}
