using System.Web;

namespace AdventOfCode._2024
{
    internal class Day17 : Day
    {
        Day17Computer computer = new();

        class Day17Instruction : ComputerInstruction
        {
            static string[] cmds = { "adv", "bxl", "bst", "jnz", "bxc", "out", "bdv", "cdv" };
            static string[] regs = { "A", "B", "C" };

            public static List<int> Output { get; private set; } =  new List<int>();

            string cmd;
            int arg = 0;

            public Day17Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(',', 2);

                cmd = cmds[split[0][0] - '0'];
                arg = int.Parse(split[1]);
            }

            public override string ToString()
            {
                return InstructionString + " (" + cmd + " " + arg + ")";
            }

            public override void Execute()
            {
                long combo = 0;

                if (arg < 4)
                    combo = arg;
                else
                    combo = Computer.GetRegister(regs[arg - 4]);

                switch (cmd)
                {
                    case "adv":
                        Computer.SetRegister("A", Computer.GetRegister("A") / (1L << (int)combo));
                        break;

                    case "bxl":
                        Computer.SetRegister("B", Computer.GetRegister("B") ^ arg);
                        break;

                    case "bst":
                        Computer.SetRegister("B", combo % 8);
                        break;

                    case "jnz":
                        if (Computer.GetRegister("A") != 0)
                        {
                            Computer.InstructionPointer = arg / 2;
                            return;
                        }
                        break;

                    case "bxc":
                        Computer.SetRegister("B", Computer.GetRegister("B") ^ Computer.GetRegister("C"));
                        break;

                    case "out":
                        Output.Add((int)(combo % 8));

                        Console.WriteLine("**out: " + (combo % 8));
                        break;

                    case "bdv":
                        Computer.SetRegister("B", Computer.GetRegister("A") / (1L << (int)combo));
                        break;

                    case "cdv":
                        Computer.SetRegister("C", Computer.GetRegister("A") / (1L << (int)combo));
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                base.Execute();
            }
        }

        class Day17Computer : Computer<Day17Instruction>
        {
            public override string GetRegisterDisplayValue(long reg)
            {
                return Convert.ToString(reg, 2);
            }

            public override void Reset()
            {
                base.Reset();

                Day17Instruction.Output.Clear();
            }
        }

        string opStr;

        void ReadData()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            foreach (string reg in split[0].SplitLines())
            {
                var match = Regex.Match(reg, @"Register (.*): (.*)");

                computer.SetRegister(match.Groups[1].Value, long.Parse(match.Groups[2].Value));
            }

            opStr = new String(split[1].Skip(9).ToArray());

            var ops = opStr.Split(",").InGroupsOf(2).Select(g => g[0] + "," + g[1]);

            computer.SetProgram(ops);
        }

        public override long Compute()
        {
            ReadData();

            while (computer.RunInstruction());

            string output = string.Join(',', Day17Instruction.Output);

            return 0;
        }

        string? Solve(List<int> desired, string soFar)
        {
            for (int digit = 0; digit < 8; digit++)
            {
                computer.Reset();

                string inputOctal = soFar + digit;

                computer.SetRegister("A", Convert.ToInt64(inputOctal, 8));

                while (computer.RunInstruction()) ;

                var output = Day17Instruction.Output;

                int takeLength = inputOctal.Length - 1;

                if (output.Count == desired.Count)
                    takeLength = inputOctal.Length;

                if (output.Take(takeLength).SequenceEqual(desired.TakeLast(takeLength)))
                {
                    if (output.SequenceEqual(desired))
                        return inputOctal;

                    var result = Solve(desired, inputOctal);

                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public override long Compute2()
        {
            ReadData();

            List<int> desired = opStr.Split(',').ToInts().ToList();

            string? result = Solve(desired, "");

            long value = Convert.ToInt64(result, 8);

            return value;
        }
    }
}
