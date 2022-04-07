using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2019
{
    internal class IntcodeComputer
    {
        int lastOutput;
        List<int> inputValues = new List<int>();
        int[] program = null;
        int currentProgramPos = 0;

        public void AddInput(int input)
        {
            inputValues.Add(input);
        }

        public void SetInput(List<int> inputs)
        {
            inputValues = new List<int>(inputs);
        }

        int GetInput()
        {
            int value = inputValues[0];

            inputValues.RemoveAt(0);

            return value;
        }

        void WriteOutput(int output)
        {
            lastOutput = output;
        }

        public int GetLastOutput()
        {
            return lastOutput;
        }

        int GetParam(string opcodeStr, int[] input, int inputPos, int paramNum)
        {
            char mode = opcodeStr[2 - paramNum];

            if (mode == '0')
                return input[input[inputPos + paramNum + 1]];
            else
                return input[inputPos + paramNum + 1];
        }

        public void SetProgram(int[] program)
        {
            this.program = program;
            currentProgramPos = 0;
            inputValues.Clear();
        }

        public void RunUntilHalt()
        {
            while (RunInstruction() != 99) ;
        }

        public int RunInstruction()
        {
            string opCodeStr = program[currentProgramPos].ToString().PadLeft(5, '0');

            int opCode = int.Parse(opCodeStr.Substring(3));

            switch (opCode)
            {
                case 1:
                    program[program[currentProgramPos + 3]] = GetParam(opCodeStr, program, currentProgramPos, 0) + GetParam(opCodeStr, program, currentProgramPos, 1);
                    currentProgramPos += 4;
                    break;
                case 2:
                    program[program[currentProgramPos + 3]] = GetParam(opCodeStr, program, currentProgramPos, 0) * GetParam(opCodeStr, program, currentProgramPos, 1);
                    currentProgramPos += 4;
                    break;
                case 3:
                    program[program[currentProgramPos + 1]] = GetInput();
                    currentProgramPos += 2;
                    break;
                case 4:
                    WriteOutput(GetParam(opCodeStr, program, currentProgramPos, 0));
                    currentProgramPos += 2;
                    break;
                case 5:
                    if (GetParam(opCodeStr, program, currentProgramPos, 0) != 0)
                    {
                        currentProgramPos = GetParam(opCodeStr, program, currentProgramPos, 1);
                    }
                    else
                    {
                        currentProgramPos += 3;
                    }
                    break;
                case 6:
                    if (GetParam(opCodeStr, program, currentProgramPos, 0) == 0)
                    {
                        currentProgramPos = GetParam(opCodeStr, program, currentProgramPos, 1);
                    }
                    else
                    {
                        currentProgramPos += 3;
                    }
                    break;
                case 7:
                    if (GetParam(opCodeStr, program, currentProgramPos, 0) < GetParam(opCodeStr, program, currentProgramPos, 1))
                    {
                        program[program[currentProgramPos + 3]] = 1;
                    }
                    else
                    {
                        program[program[currentProgramPos + 3]] = 0;
                    }

                    currentProgramPos += 4;
                    break;
                case 8:
                    if (GetParam(opCodeStr, program, currentProgramPos, 0) == GetParam(opCodeStr, program, currentProgramPos, 1))
                    {
                        program[program[currentProgramPos + 3]] = 1;
                    }
                    else
                    {
                        program[program[currentProgramPos + 3]] = 0;
                    }

                    currentProgramPos += 4;
                    break;
                case 99:
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return opCode;
        }

        public int RunProgram(int[] program)
        {
            SetProgram(program);

            RunUntilHalt();

            return lastOutput;
        }

    }
}
