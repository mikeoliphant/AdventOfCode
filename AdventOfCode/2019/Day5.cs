using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2019
{
    internal class Day5
    {
        public long Compute()
        {
            long[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            return computer.RunProgram(input, 1);
        }

        public long Compute2()
        {
            long[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToLongs(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            return computer.RunProgram(input, 5);
        }
    }
}
