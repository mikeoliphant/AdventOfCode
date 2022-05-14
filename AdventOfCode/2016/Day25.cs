namespace AdventOfCode._2016
{
    internal class Day25
    {
        class Day25Instruction : ComputerInstruction
        {
            public static long LastClock { get; private set; }

            public string Cmd { get; private set; }
            public string[] Args { get; private set; }

            public Day25Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(' ');

                Cmd = split[0];

                Args = split.Skip(1).ToArray();
            }

            public override void Execute()
            {
                switch (Cmd)
                {
                    case "out":
                        LastClock = Computer.GetRegisterOrVal(Args[0]);
                        break;

                    case "tgl":
                        int offset = Computer.InstructionPointer + (int)Computer.GetRegisterOrVal(Args[0]);

                        if ((offset >= 0) && (offset < (Computer as Computer<Day25Instruction>).Instructions.Count))
                        {
                            Day25Instruction instruction = (Computer as Computer<Day25Instruction>).Instructions[offset];

                            switch (instruction.Cmd)
                            {
                                case "tgl":
                                    instruction.Cmd = "inc";
                                    break;

                                case "cpy":
                                    instruction.Cmd = "jnz";
                                    break;

                                case "inc":
                                    instruction.Cmd = "dec";
                                    break;

                                case "dec":
                                    instruction.Cmd = "inc";
                                    break;

                                case "jnz":
                                    instruction.Cmd = "cpy";
                                    break;
                            }
                        }

                        break;

                    case "cpy":
                        Computer.SetRegister(Args[1], Computer.GetRegisterOrVal(Args[0]));
                        break;

                    case "inc":
                        Computer.SetRegister(Args[0], Computer.GetRegister(Args[0]) + 1);
                        break;

                    case "dec":
                        Computer.SetRegister(Args[0], Computer.GetRegister(Args[0]) - 1);
                        break;

                    case "jnz":
                        if (Computer.GetRegisterOrVal(Args[0]) != 0)
                        {
                            Computer.InstructionPointer += (int)Computer.GetRegisterOrVal(Args[1]);

                            return;
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                base.Execute();
            }
        }

        public long Compute()
        {
            Computer<Day25Instruction> computer = new Computer<Day25Instruction>();

            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day25.txt"));

            long a = 0;

            do
            {
                computer.Reset();
                computer.SetRegister("a", a);

                int desiredVal = 0;

                bool isGood = true;

                for (int i = 0; i < 10; i++)
                {
                    do
                    {
                        computer.RunInstruction();
                    }
                    while (computer.LastInstruction.Cmd != "out");

                    if (Day25Instruction.LastClock != desiredVal)
                    {
                        isGood = false;

                        break;
                    }

                    desiredVal = 1 - desiredVal;
                }

                if (isGood)
                    break;

                a++;
            }
            while (true);


            return a;
        }
    }
}
