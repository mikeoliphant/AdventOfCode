namespace AdventOfCode._2018
{
    class OpcodeSample
    {
        public int[] Instruction { get; set; }
        public int[] BeforeRegisters { get; set; }
        public int[] AfterRegisters { get; set; }
    }

    internal class Day16
    {
        Dictionary<string, Action<int, int, int>> operators = new Dictionary<string, Action<int, int, int>>();
        int[] R = new int[4];

        List<OpcodeSample> data = new List<OpcodeSample>();
        List<int[]> program = new List<int[]>();

        public void ReadInput()
        {
            operators["addr"] = delegate(int A, int B, int C) { R[C] =  R[A] + R[B]; };
            operators["addi"] = delegate (int A, int B, int C) { R[C] = R[A] + B; };

            operators["mulr"] = delegate (int A, int B, int C) { R[C] = R[A] * R[B]; };
            operators["muli"] = delegate (int A, int B, int C) { R[C] = R[A] * B; };

            operators["banr"] = delegate (int A, int B, int C) { R[C] = R[A] & R[B]; };
            operators["bani"] = delegate (int A, int B, int C) { R[C] = R[A] & B; };

            operators["borr"] = delegate (int A, int B, int C) { R[C] = R[A] | R[B]; };
            operators["bori"] = delegate (int A, int B, int C) { R[C] = R[A] | B; };

            operators["setr"] = delegate (int A, int B, int C) { R[C] = R[A]; };
            operators["seti"] = delegate (int A, int B, int C) { R[C] = A; };

            operators["gtir"] = delegate (int A, int B, int C) { R[C] = (A > R[B]) ? 1 : 0; };
            operators["gtri"] = delegate (int A, int B, int C) { R[C] = (R[A] > B) ? 1 : 0; };
            operators["gtrr"] = delegate (int A, int B, int C) { R[C] = (R[A] > R[B]) ? 1 : 0; };

            operators["eqir"] = delegate (int A, int B, int C) { R[C] = (A == R[B]) ? 1 : 0; };
            operators["eqri"] = delegate (int A, int B, int C) { R[C] = (R[A] == B) ? 1 : 0; };
            operators["eqrr"] = delegate (int A, int B, int C) { R[C] = (R[A] == R[B]) ? 1 : 0; };

            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day16.txt").ToArray();

            int pos = 0;

            do
            {
                if (string.IsNullOrEmpty(lines[pos].Trim()))
                    break;

                OpcodeSample opcode = new OpcodeSample();

                Match match = Regex.Match(lines[pos++], "\\[(.*)\\]");
                opcode.BeforeRegisters = match.Groups[1].Value.ToInts(',').ToArray(); ;

                opcode.Instruction = lines[pos++].ToInts(' ').ToArray();

                match = Regex.Match(lines[pos++], "\\[(.*)\\]");
                opcode.AfterRegisters = match.Groups[1].Value.ToInts(',').ToArray();

                data.Add(opcode);

                pos++;
            }
            while (true);

            while (string.IsNullOrEmpty(lines[pos].Trim()))
            {
                pos++;
            }

            do
            {
                if (pos == lines.Length)
                    break;

                if (string.IsNullOrEmpty(lines[pos].Trim()))
                    break;

                program.Add(lines[pos++].ToInts(' ').ToArray());
            }
            while (true);
        }

        public long Compute()
        {
            ReadInput();

            int numThreePlus = 0;

            foreach (OpcodeSample sample in data)
            {
                int numMatch = 0;

                foreach (var opFunc in operators.Values)
                {
                    for (int r = 0; r < sample.BeforeRegisters.Length; r++)
                    {
                        R[r] = sample.BeforeRegisters[r];
                    }

                    opFunc(sample.Instruction[1], sample.Instruction[2], sample.Instruction[3]);

                    bool matchAll = true;

                    for (int r = 0; r < sample.BeforeRegisters.Length; r++)
                    {
                        if (R[r] != sample.AfterRegisters[r])
                        {
                            matchAll = false;

                            break;
                        }
                    }

                    if (matchAll)
                    {
                        numMatch++;
                    }
                }

                if (numMatch >= 3)
                {
                    numThreePlus++;
                }
            }

            return numThreePlus;
        }

        public long Compute2()
        {
            ReadInput();

            Dictionary<int, HashSet<string>> possibleOpcodes = new Dictionary<int, HashSet<string>>();
            Dictionary<int, string> opcodeMap = new Dictionary<int, string>();

            foreach (OpcodeSample sample in data)
            {
                int numMatch = 0;

                foreach (var opFunc in operators)
                {
                    for (int r = 0; r < sample.BeforeRegisters.Length; r++)
                    {
                        R[r] = sample.BeforeRegisters[r];
                    }

                    opFunc.Value(sample.Instruction[1], sample.Instruction[2], sample.Instruction[3]);

                    bool matchAll = true;

                    for (int r = 0; r < sample.BeforeRegisters.Length; r++)
                    {
                        if (R[r] != sample.AfterRegisters[r])
                        {
                            matchAll = false;

                            break;
                        }
                    }

                    if (matchAll)
                    {
                        if (!possibleOpcodes.ContainsKey(sample.Instruction[0]))
                        {
                            possibleOpcodes[sample.Instruction[0]] = new HashSet<string>();
                        }

                        possibleOpcodes[sample.Instruction[0]].Add(opFunc.Key);
                    }
                }
            }

            do
            {
                var op = possibleOpcodes.Where(o => o.Value.Count == 1).First();

                string name = op.Value.First();

                opcodeMap[op.Key] = name;

                possibleOpcodes.Remove(op.Key);

                foreach (var otherOp in possibleOpcodes)
                {
                    otherOp.Value.Remove(name);
                }
            }
            while (possibleOpcodes.Count > 0);

            Array.Clear(R);

            foreach (int[] instr in program)
            {
                operators[opcodeMap[instr[0]]](instr[1], instr[2], instr[3]);
            }

            return R[0];
        }
    }
}
