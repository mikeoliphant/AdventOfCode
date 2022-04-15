namespace AdventOfCode._2019
{
    internal class IntcodeComputer
    {
        public Func<long> InputFunc { get; set; }

        bool halted = false;
        long[] program = null;
        long lastOutput;
        Queue<long> inputValues = new Queue<long>();
        long currentProgramPos = 0;
        long relativeBase = 0;
        Dictionary<long, long> memory = new Dictionary<long, long>();
        long memoryStart = 0;

        public void AddInput(long input)
        {
            inputValues.Enqueue(input);
        }

        public void SetInput(List<long> inputs)
        {
            inputValues = new Queue<long>(inputs);
        }

        long GetInput()
        {
            if (inputValues.Count > 0)
            {
                long value = inputValues.Dequeue();

                return value;
            }

            if (InputFunc != null)
                return InputFunc();

            throw new InvalidOperationException();
        }

        void WriteOutput(long output)
        {
            lastOutput = output;
        }

        public long GetLastOutput()
        {
            return lastOutput;
        }

        public long GetNextOutput()
        {
            RunUntilOutput();

            return lastOutput;
        }

        public Dictionary<long, long> SnapshotMemory()
        {
            Dictionary<long, long> snapshot = new Dictionary<long, long>();

            foreach (var loc in memory)
            {
                if (loc.Key >= memoryStart)
                {
                    snapshot[loc.Key - memoryStart] = loc.Value;
                }
            }

            return snapshot;
        }

        public void SetMemory(long pos, long value)
        {
            memory[memoryStart + pos] = value;
        }

        public long GetMemory(long pos)
        {
            pos += memoryStart;

            if (!memory.ContainsKey(pos))
                return 0;

            return memory[pos];
        }

        public void Reset()
        {
            halted = false;

            memory.Clear();

            if (program != null)
            {
                memoryStart = program.LongLength;

                for (int i = 0; i < program.Length; i++)
                {
                    SetMemory(i, program[i]);
                }
            }

            currentProgramPos = 0;
            relativeBase = 0;

            inputValues.Clear();
        }

        public void SetProgram(long[] program)
        {
            this.program = program;

            Reset();
        }

        public void RunUntilHalt()
        {
            while (RunInstruction() != 99) ;
        }

        public bool RunUntilOutput()
        {
            int opCode = 0;

            do
            {
                opCode = RunInstruction();
            }
            while ((opCode != 4) && (opCode != 99));

            return (opCode == 4);
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
            if (halted)
                throw new InvalidOperationException();

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
                    halted = true;
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
