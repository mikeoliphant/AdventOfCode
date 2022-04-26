using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    internal class Day21
    {
        Computer2018 computer = new Computer2018();

        public long Compute()
        {
            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day21.txt"));

            computer.RunDebug();

            return 15823996;    // Figured out by using "waiti 28"
        }

        public long Compute2()
        {
            Dictionary<long, bool> hist = new Dictionary<long, bool>();

            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day21.txt"));

            //computer.RunDebug();

            long lastVal = 0;

            while (computer.RunInstruction())
            {
                if (computer.InstructionPointer == 25)
                {
                    if (computer.R[5] == 2)
                        computer.R[5] = computer.R[3] / 256;
                }

                if (computer.InstructionPointer == 28)
                {
                    long val = computer.R[4];

                    if (hist.ContainsKey(val))
                    {
                        return lastVal;
                    }
                    else
                    {
                        hist[val] = true;
                    }

                    lastVal = val;
                }
            }

            return 0;
        }
    }
}
