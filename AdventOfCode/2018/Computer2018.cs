namespace AdventOfCode._2018
{
    internal class Computer2018
    {
        public long[] R { get; private set; }
        public int InstructionPointer { get; private set; }
        public int InstructionRegister { get; set; }
        public Instruction LastInstruction { get; private set; }

        Dictionary<string, Action<int, int, int>> operators = new Dictionary<string, Action<int, int, int>>();
        List<Instruction> instructions = new List<Instruction>();

        public Computer2018()
        {
            R = new long[6];

            InstructionRegister = -1;

            operators["addr"] = delegate (int A, int B, int C) { R[C] = R[A] + R[B]; };
            operators["addi"] = delegate (int A, int B, int C) { R[C] = R[A] + B; };

            operators["mulr"] = delegate (int A, int B, int C) { R[C] = R[A] * R[B]; };
            operators["muli"] = delegate (int A, int B, int C) { R[C] = R[A] * B; };

            operators["banr"] = delegate (int A, int B, int C) { R[C] = R[A] & R[B]; };
            operators["bani"] = delegate (int A, int B, int C) { R[C] = R[A] & B; };

            operators["borr"] = delegate (int A, int B, int C) { R[C] = R[A] | R[B]; };
            operators["bori"] = delegate (int A, int B, int C) { R[C] = R[A] | (long)B; };



            operators["setr"] = delegate (int A, int B, int C) { R[C] = R[A]; };
            operators["seti"] = delegate (int A, int B, int C) { R[C] = A; };

            operators["gtir"] = delegate (int A, int B, int C) { R[C] = (A > R[B]) ? 1 : 0; };
            operators["gtri"] = delegate (int A, int B, int C) { R[C] = (R[A] > B) ? 1 : 0; };
            operators["gtrr"] = delegate (int A, int B, int C) { R[C] = (R[A] > R[B]) ? 1 : 0; };

            operators["eqir"] = delegate (int A, int B, int C) { R[C] = (A == R[B]) ? 1 : 0; };
            operators["eqri"] = delegate (int A, int B, int C) { R[C] = (R[A] == B) ? 1 : 0; };
            operators["eqrr"] = delegate (int A, int B, int C) { R[C] = (R[A] == R[B]) ? 1 : 0; };
        }

        public void SetProgram(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                string[] split = line.Split(' ', 2);

                if (split[0] == "#ip")
                {
                    InstructionRegister = int.Parse(split[1]);
                }
                else
                {
                    int[] args = split[1].ToInts(' ').ToArray();

                    instructions.Add(new Instruction { Opcode = split[0], Args = args });
                }
            }
        }

        public bool RunInstruction()
        {
            R[InstructionRegister] = InstructionPointer;

            LastInstruction = instructions[InstructionPointer];

            operators[LastInstruction.Opcode](LastInstruction.Args[0], LastInstruction.Args[1], LastInstruction.Args[2]);

            InstructionPointer = (int)R[InstructionRegister];

            InstructionPointer++;

            return (InstructionPointer < instructions.Count);
        }

        public void RunDebug()
        {
            InstructionPointer = 0;

            int waitForRegister = -1;
            int waitForInstruction = -1;
            int run = 0;

            do
            {
                int lastInstructionPos = InstructionPointer;

                if (!RunInstruction())
                    break;

                if (run > 0)
                {
                    run--;
                    continue;
                }

                if (waitForRegister != -1)
                {
                    if (LastInstruction.Args[2] != waitForRegister)
                    {
                        continue;
                    }

                    waitForRegister = -1;
                }

                if (waitForInstruction != -1)
                {
                    if (InstructionPointer != waitForInstruction)
                    {
                        continue;
                    }

                    waitForInstruction = -1;
                }

                do
                {
                    Console.WriteLine();
                    Console.WriteLine(lastInstructionPos + ": " + LastInstruction.Opcode + " " + String.Join(' ', LastInstruction.Args));
                    Console.WriteLine();

                    for (int r = 0; r < R.Length; r++)
                    {
                        Console.Write("[" + r + "] " + R[r]);

                        if (LastInstruction.Args[2] == r)
                            Console.Write(" < ");

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.Write("> ");

                    string cmd = Console.ReadLine();

                    if (cmd.StartsWith("run"))
                    {
                        run = int.Parse(cmd.Substring(4));
                    }
                    if (cmd.StartsWith("waitr"))
                    {
                        waitForRegister = int.Parse(cmd.Substring(5));

                        if ((waitForRegister < 0) | (waitForRegister > (R.Length - 1)))
                        {
                            Console.WriteLine("Bad register");

                            waitForRegister = -1;
                        }
                        break;
                    }
                    if (cmd.StartsWith("waiti"))
                    {
                        waitForInstruction = int.Parse(cmd.Substring(5));

                        if ((waitForInstruction < 0) | (waitForInstruction > (instructions.Count - 1)))
                        {
                            Console.WriteLine("Bad instruction");

                            waitForInstruction = -1;
                        }
                        break;
                    }
                    else if (cmd.StartsWith("set"))
                    {
                        int[] args = cmd.Substring(4).ToInts(' ').ToArray();

                        R[args[0]] = args[1];
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cmd))
                        {
                            Console.WriteLine("Bad command");
                        }

                        break;
                    }
                }
                while (true);
            }
            while (true);
        }

    }
}
