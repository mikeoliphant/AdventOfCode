namespace AdventOfCode._2015
{
    internal class Day23 : Day
    {
        class Day23Instruction : ComputerInstruction
        {
            string cmd;
            string[] args;

            public Day23Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(' ', 2);

                cmd = split[0];

                args = split[1].Split(',');
            }

            public override void Execute()
            {
                switch (cmd)
                {
                    case "hlf":
                        Computer.SetRegister(args[0], Computer.GetRegister(args[0]) / 2);
                        break;

                    case "tpl":
                        Computer.SetRegister(args[0], Computer.GetRegister(args[0]) * 3);
                        break;

                    case "inc":
                        Computer.SetRegister(args[0], Computer.GetRegister(args[0]) + 1);
                        break;

                    case "jmp":
                        Computer.InstructionPointer += int.Parse(args[0]);
                        return;

                    case "jie":
                        if ((Computer.GetRegister(args[0]) % 2) == 0)
                        {
                            Computer.InstructionPointer += int.Parse(args[1]);

                            return;
                        }
                        break;

                    case "jio":
                        if (Computer.GetRegister(args[0]) == 1)
                        {
                            Computer.InstructionPointer += int.Parse(args[1]);

                            return;
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                base.Execute();
            }
        }

        public override long Compute()
        {
            Computer<Day23Instruction> computer = new Computer<Day23Instruction>();

            computer.SetProgram(File.ReadLines(DataFile));

            computer.SetRegister("a", 1);

            //computer.RunDebug();
            while (computer.RunInstruction()) ;

            return computer.GetRegister("b");
        }
    }
}
