using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2019
{
    internal class IntcodeComputer
    {
        long lastOutput;
        List<long> inputValues = new List<long>();
        long currentProgramPos = 0;
        long relativeBase = 0;
        Dictionary<long, long> memory = new Dictionary<long, long>();

        public void AddInput(long input)
        {
            inputValues.Add(input);
        }

        public void SetInput(List<long> inputs)
        {
            inputValues = new List<long>(inputs);
        }

        long GetInput()
        {
            long value = inputValues[0];

            inputValues.RemoveAt(0);

            return value;
        }

        void WriteOutput(long output)
        {
            lastOutput = output;
        }

        public long GetLastOutput()
        {
            return lastOutput;
        }

        public void SetMemory(long pos, long value)
        {
            memory[pos] = value;
        }

        public long GetMemory(long pos)
        {
            if (!memory.ContainsKey(pos))
                return 0;

            return memory[pos];
        }

        public void SetProgram(long[] program)
        {
            memory.Clear();

            for (int i = 0; i < program.Length; i++)
            {
                SetMemory(i, program[i]);
            }

            currentProgramPos = 0;
            relativeBase = 0;
            inputValues.Clear();
        }

        public void RunUntilHalt()
        {
            while (RunInstruction() != 99) ;
        }

        long GetParam(string opcodeStr, long inputPos, int paramNum)
        {
            char mode = opcodeStr[2 - paramNum];

            switch (mode)
            {
                case '0':
                    return GetMemory(GetMemory(inputPos + paramNum + 1));
                case '1':
                    return GetMemory(inputPos + paramNum + 1);
                case '2':
                    return GetMemory(GetMemory(inputPos + paramNum + 1) + relativeBase);
            }

            throw new InvalidOperationException();
        }

        void WriteParam(string opcodeStr, long inputPos, int paramNum, long value)
        {
            char mode = opcodeStr[2 - paramNum];

            switch (mode)
            {
                case '0':
                    SetMemory(GetMemory(inputPos + paramNum + 1), value);
                    return;
                case '2':
                    SetMemory(GetMemory(inputPos + paramNum + 1) + relativeBase, value);
                    return;
            }

            throw new InvalidOperationException();
        }

        public int RunInstruction()
        {
            string opCodeStr = GetMemory(currentProgramPos).ToString().PadLeft(5, '0');

            int opCode = int.Parse(opCodeStr.Substring(3));

            switch (opCode)
            {
                case 1:
                    WriteParam(opCodeStr, currentProgramPos, 2,  GetParam(opCodeStr, currentProgramPos, 0) + GetParam(opCodeStr, currentProgramPos, 1));
                    currentProgramPos += 4;
                    break;
                case 2:
                    WriteParam(opCodeStr, currentProgramPos, 2, GetParam(opCodeStr, currentProgramPos, 0) * GetParam(opCodeStr, currentProgramPos, 1));
                    currentProgramPos += 4;
                    break;
                case 3:
                    WriteParam(opCodeStr, currentProgramPos, 0, GetInput());
                    currentProgramPos += 2;
                    break;
                case 4:
                    WriteOutput(GetParam(opCodeStr, currentProgramPos, 0));
                    currentProgramPos += 2;
                    break;
                case 5:
                    if (GetParam(opCodeStr, currentProgramPos, 0) != 0)
                    {
                        currentProgramPos = GetParam(opCodeStr, currentProgramPos, 1);
                    }
                    else
                    {
                        currentProgramPos += 3;
                    }
                    break;
                case 6:
                    if (GetParam(opCodeStr, currentProgramPos, 0) == 0)
                    {
                        currentProgramPos = GetParam(opCodeStr, currentProgramPos, 1);
                    }
                    else
                    {
                        currentProgramPos += 3;
                    }
                    break;
                case 7:
                    if (GetParam(opCodeStr, currentProgramPos, 0) < GetParam(opCodeStr, currentProgramPos, 1))
                    {
                        WriteParam(opCodeStr, currentProgramPos, 2, 1);
                    }
                    else
                    {
                        WriteParam(opCodeStr, currentProgramPos, 2, 0);
                    }

                    currentProgramPos += 4;
                    break;
                case 8:
                    if (GetParam(opCodeStr, currentProgramPos, 0) == GetParam(opCodeStr, currentProgramPos, 1))
                    {
                        WriteParam(opCodeStr, currentProgramPos, 2, 1);
                    }
                    else
                    {
                        WriteParam(opCodeStr, currentProgramPos, 2, 0);
                    }

                    currentProgramPos += 4;
                    break;
                case 9:
                    relativeBase += GetParam(opCodeStr, currentProgramPos, 0);

                    currentProgramPos += 2;
                    break;
                case 99:
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return opCode;
        }

        public long RunProgram(long[] program)
        {
            return RunProgram(program, new long[0]);
        }

        public long RunProgram(long[] program, long input)
        {
            return RunProgram(program, new long[] { input });
        }

        public long RunProgram(long[] program, IEnumerable<long> input)
        {
            SetProgram(program);

            foreach (long i in input)
                AddInput(i);

            RunUntilHalt();

            return lastOutput;
        }

    }
}
