using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2019
{
    internal class Day5
    {
        int lastOutput;
        int inputValue;

        int GetInput()
        {
            return inputValue;
        }

        void WriteOutput(int output)
        {
            lastOutput = output;
        }

        int GetParam(string opcodeStr, int[] input, int inputPos, int paramNum)
        {
            char mode = opcodeStr[2 - paramNum];

            if (mode == '0')
                return input[input[inputPos + paramNum + 1]];
            else
                return input[inputPos + paramNum + 1];
        }

        public int RunProgram(int[] input)
        {
            int opCode = 0;
            int currentPos = 0;

            do
            {
                string opCodeStr = input[currentPos].ToString().PadLeft(5, '0');

                opCode = int.Parse(opCodeStr.Substring(3));

                switch (opCode)
                {
                    case 1:
                        input[input[currentPos + 3]] = GetParam(opCodeStr, input, currentPos, 0) + GetParam(opCodeStr, input, currentPos, 1);
                        currentPos += 4;
                        break;
                    case 2:
                        input[input[currentPos + 3]] = GetParam(opCodeStr, input, currentPos, 0) * GetParam(opCodeStr, input, currentPos, 1);
                        currentPos += 4;
                        break;
                    case 3:
                        input[input[currentPos + 1]] = GetInput();
                        currentPos += 2;
                        break;
                    case 4:
                        WriteOutput(GetParam(opCodeStr, input, currentPos, 0));
                        currentPos += 2;
                        break;
                    case 5:
                        if (GetParam(opCodeStr, input, currentPos, 0) != 0)
                        {
                            currentPos = GetParam(opCodeStr, input, currentPos, 1);
                        }
                        else
                        {
                            currentPos += 3;
                        }
                        break;
                    case 6:
                        if (GetParam(opCodeStr, input, currentPos, 0) == 0)
                        {
                            currentPos = GetParam(opCodeStr, input, currentPos, 1);
                        }
                        else
                        {
                            currentPos += 3;
                        }
                        break;
                    case 7:
                        if (GetParam(opCodeStr, input, currentPos, 0) < GetParam(opCodeStr, input, currentPos, 1))
                        {
                            input[input[currentPos + 3]] = 1;
                        }
                        else
                        {
                            input[input[currentPos + 3]] = 0;
                        }

                        currentPos += 4;
                        break;
                    case 8:
                        if (GetParam(opCodeStr, input, currentPos, 0) == GetParam(opCodeStr, input, currentPos, 1))
                        {
                            input[input[currentPos + 3]] = 1;
                        }
                        else
                        {
                            input[input[currentPos + 3]] = 0;
                        }

                        currentPos += 4;
                        break;
                    case 99:
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            while (opCode != 99);

            return lastOutput;
        }

        public long Compute()
        {
            int[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToInts(',').ToArray();

            inputValue = 1;

            return RunProgram(input);
        }

        public long Compute2()
        {
            int[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day5.txt").ToInts(',').ToArray();

            inputValue = 5;

            return RunProgram(input);
        }
    }
}
