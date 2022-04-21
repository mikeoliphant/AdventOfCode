namespace AdventOfCode._2018
{
    class Instruction
    {
        public string Opcode { get; set; }
        public int[] Args { get; set; }

        public override string ToString()
        {
            return Opcode + " " + String.Join(" ", Args);
        }
    }

    internal class Day19
    {
        Dictionary<string, Action<int, int, int>> operators = new Dictionary<string, Action<int, int, int>>();
        long[] R = new long[6];
        int instructionRegister = 0;
        List<Instruction> instructions = new List<Instruction>();

        void ReadInput()
        {
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

            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day19.txt"))
            {
                string[] split = line.Split(' ', 2);

                if (split[0] == "#ip")
                {
                    instructionRegister = int.Parse(split[1]);
                }
                else
                {
                    int[] args = split[1].ToInts(' ').ToArray();

                    instructions.Add(new Instruction { Opcode = split[0], Args = args });
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            int instructionPointer = 0;

            R[0] = 1;

            int waitFor = -1;
            int run = 0;

            //
            // Key is that the program is running two loops up to 10551287
            //
            // Register 0 gets written to if loop1 var * loop2 var == 10551287
            //
            // Only a few factors, so I solved it manually
            //
            // 83081 * 127
            // 42037 * 251
            // 31877 * 331

            do
            {
                int lastInstruction = instructionPointer;

                R[instructionRegister] = instructionPointer;

                Instruction inst = instructions[instructionPointer];

                operators[inst.Opcode](inst.Args[0], inst.Args[1], inst.Args[2]);

                instructionPointer = (int)R[instructionRegister];

                instructionPointer++;

                if (instructionPointer >= instructions.Count)
                    break;

                if (run > 0)
                {
                    run--;
                    continue;
                }

                if (waitFor != -1)
                {
                    if (inst.Args[2] != waitFor)
                        continue;

                    waitFor = -1;
                }

                do
                {
                    Console.WriteLine();
                    Console.WriteLine(lastInstruction + ": " + inst.Opcode + " " + String.Join(' ', inst.Args));
                    Console.WriteLine();

                    for (int r = 0; r < R.Length; r++)
                    {
                        Console.Write("[" + r + "] " + R[r]);

                        if (inst.Args[2] == r)
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
                    if (cmd.StartsWith("wait"))
                    {
                        waitFor = int.Parse(cmd.Substring(5));

                        if ((waitFor < 0) | (waitFor > (R.Length - 1)))
                        {
                            Console.WriteLine("Bad register");

                            waitFor = -1;
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
                        break;
                    }
                }
                while (true);
            }
            while (true);

            return R[0];
        }
    }
}
