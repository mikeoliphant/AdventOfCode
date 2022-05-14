namespace AdventOfCode._2016
{
    internal class Day23
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
                    case "tgl":
                        int offset = Computer.InstructionPointer + (int)Computer.GetRegisterOrVal(args[0]);

                        if ((offset >= 0) && (offset < (Computer as Computer<Day23Instruction>).Instructions.Count))
                        {
                            Day23Instruction instruction = (Computer as Computer<Day23Instruction>).Instructions[offset];

                            switch (instruction.cmd)
                            {
                                case "tgl":
                                    instruction.cmd = "inc";
                                    break;

                                case "cpy":
                                    instruction.cmd = "jnz";
                                    break;

                                case "inc":
                                    instruction.cmd = "dec";
                                    break;

                                case "dec":
                                    instruction.cmd = "inc";
                                    break;

                                case "jnz":
                                    instruction.cmd = "cpy";
                                    break;
                            }
                        }

                        break;

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

            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day23.txt"));

            computer.SetRegister("a", 7);

            while (computer.RunInstruction()) ;

            return computer.GetRegister("a");
        }


        public long Compute2()
        {
            Computer<Day23Instruction> computer = new Computer<Day23Instruction>();

            // The program slowly calculates a series in register 'a'
            //
            // It also toggles every other instruction at the end of the program
            //
            //132 * 10
            //1320 * 9
            //11880 * 8
            //95040 * 7
            //665280 * 6
            //3991680 * 5
            //19958400 * 4-- toggle + 8
            //79833600 * 3-- toggle + 6
            //239500800 * 2-- toggle + 4
            //479001600 * 1-- toggle + 2

            // This hacked program has the end instructions properly toggled
            computer.SetProgram(File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day23Hack.txt"));

            //computer.SetRegister("a", 12);

            // Set regizter "a" to the value it would have had
            computer.SetRegister("a", 479001600);

            //computer.RunDebug();

            while (computer.RunInstruction()) ;

            return computer.GetRegister("a");
        }
    }
}
