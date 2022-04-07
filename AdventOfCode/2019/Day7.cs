using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day7
    {
        IntcodeComputer[] amplifiers = new IntcodeComputer[5];

        int RunPhaseSequence(int[] phaseValues, int[] program)
        {
            int output = 0;

            for (int i = 0; i < amplifiers.Length; i++)
            {
                amplifiers[i].AddInput(phaseValues[i]);
                amplifiers[i].AddInput(output);

                output = amplifiers[i].RunProgram(program.Clone() as int[]);
            }

            return output;
        }

        // Calculate all permutation of a sequence using Heap's algorithm
        IEnumerable<int[]> GetAllPermutations(int[] sequence)
        {
            sequence = sequence.Clone() as int[];  // don't trash our input sequence

            int length = sequence.Length;

            int numPermutations = length * (length - 1);

            int[][] permutations = new int[numPermutations][];

            yield return sequence.Clone() as int[];

            int[] c = new int[length];

            for (int i = 0;  i < length;)
            {
                if (c[i] < i)
                {
                    if ((i % 2) == 0)
                    {
                        int tmp = sequence[0];
                        sequence[0] = sequence[i];
                        sequence[i] = tmp;
                    }
                    else
                    {
                        int tmp = sequence[c[i]];
                        sequence[c[i]] = sequence[i];
                        sequence[i] = tmp;
                    }

                    yield return sequence.Clone() as int[];

                    c[i]++;
                    i = 0;
                }
                else
                {
                    c[i] = 0;
                    i++;
                }
            }
        }

        public long Compute()
        {
            int[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day7.txt").ToInts(',').ToArray();

            for (int i = 0; i < amplifiers.Length; i++)
                amplifiers[i] = new IntcodeComputer();

            int maxOutput = 0;
            int[] maxPhaseSequence;

            foreach (int[] phaseSequence in GetAllPermutations(new int[] { 0, 1, 2, 3, 4 }))
            {
                int output = RunPhaseSequence(phaseSequence, program);

                if (output > maxOutput)
                {
                    maxOutput = output;
                    maxPhaseSequence = phaseSequence;
                }
            }

            return maxOutput;
        }

        int RunPhaseSequence2(int[] phaseValues, int[] program)
        {
            int output = 0;

            for (int i = 0; i < amplifiers.Length; i++)
            {
                amplifiers[i].SetProgram(program.Clone() as int[]);
                amplifiers[i].AddInput(phaseValues[i]);
            }

            bool halted = false;

            do
            {
                for (int i = 0; i < amplifiers.Length; i++)
                {
                    amplifiers[i].AddInput(output);

                    int opCode = 0;

                    do
                    {
                        opCode = amplifiers[i].RunInstruction();
                    }
                    while((opCode != 4) && (opCode != 99));

                    if (opCode == 99)
                    {
                        halted = true;
                        break;
                    }

                    output = amplifiers[i].GetLastOutput();
                }
            }
            while (!halted);

            return output;
        }

        public long Compute2()
        {
            int[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day7.txt").ToInts(',').ToArray();

            for (int i = 0; i < amplifiers.Length; i++)
                amplifiers[i] = new IntcodeComputer();

            int maxOutput = 0;
            int[] maxPhaseSequence;

            foreach (int[] phaseSequence in GetAllPermutations(new int[] { 5, 6, 7, 8, 9 }))
            {
                int output = RunPhaseSequence2(phaseSequence, program);

                if (output > maxOutput)
                {
                    maxOutput = output;
                    maxPhaseSequence = phaseSequence;
                }
            }

            return maxOutput;
        }
    }
}
