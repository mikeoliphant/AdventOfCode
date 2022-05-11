namespace AdventOfCode._2016
{
    internal class Day12
    {
        class Day23Instruction : ComputerInstruction
        {
            string cmd;
            string[] args;

            public Day23Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(' ');

                cmd = split[0];

                args = split.Skip(1).ToArray();
            }

            public override void Execute()
            {
                switch (cmd)
                {
                    case "cpy":
                        Computer.SetRegister(args[1], Computer.GetRegisterOrVal(args[0]));
                        break;

                    case "inc":
                        Computer.SetRegister(args[0], Computer.GetRegister(args[0]) + 1);
                        break;

                    case "dec":
                        Computer.SetRegister(args[0], Computer.GetRegister(args[0]) - 1);
                        break;

                    case "jnz":
                        if (Computer.GetRegisterOrVal(args[0]) != 0)
                        {
                            Computer.InstructionPointer += (int)Computer.GetRegisterOrVal(args[1]);

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
            Computer<Day23Instruction> computer = new Computer<Day23Instruction>();

            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day12.txt"));

            computer.SetRegister("c", 1);

            while (computer.RunInstruction()) ;

            return computer.GetRegister("a");
        }
    }
}
