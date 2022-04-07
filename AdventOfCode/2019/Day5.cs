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
            int[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToInts(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.AddInput(1);

            return computer.RunProgram(input);
        }

        public long Compute2()
        {
            int[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToInts(',').ToArray();

            IntcodeComputer computer = new IntcodeComputer();

            computer.AddInput(5);

            return computer.RunProgram(input);
        }
    }
}
