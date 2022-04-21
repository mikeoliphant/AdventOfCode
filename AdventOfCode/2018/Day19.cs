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

            do
            {
                R[instructionRegister] = instructionPointer;

                Instruction inst = instructions[instructionPointer];

                operators[inst.Opcode](inst.Args[0], inst.Args[1], inst.Args[2]);

                instructionPointer = (int)R[instructionRegister];

                instructionPointer++;

                if (instructionPointer >= instructions.Count)
                    break;

                //if (R[5] == (10551287 - 1))
                //{

                //}

                if (R[5] == 3)
                {
                    R[5] = 10551287 - 3;
                }
            }
            while (true);

            return R[0];
        }
    }
}
