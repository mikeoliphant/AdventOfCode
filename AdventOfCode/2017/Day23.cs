namespace AdventOfCode._2017
{
    internal class Day23
    {
        class Day23Instruction : ComputerInstruction
        {
            public static long NumMul { get; private set; }

            string cmd;
            string[] args;

            public Day23Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(' ');

                cmd = split[0];

                args = split.Skip(1).ToArray();
            }

            long GetVal(string val)
            {
                if (char.IsLetter(val[0]))
                {
                    return Computer.GetRegister(val);
                }

                return int.Parse(val);
            }

            public override void Execute()
            {
                switch (cmd)
                {
                    case "set":
                        Computer.SetRegister(args[0], GetVal(args[1]));
                        break;

                    case "sub":
                        Computer.SetRegister(args[0], GetVal(args[0]) - GetVal(args[1]));
                        break;

                    case "mul":
                        Computer.SetRegister(args[0], GetVal(args[0]) * GetVal(args[1]));

                        NumMul++;

                        break;

                    case "jnz":
                        if (GetVal(args[0]) != 0)
                        {
                            Computer.InstructionPointer += (int)GetVal(args[1]);

                            return;
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                base.Execute();
            }
        }

        bool IsPrime(int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

            for (int i = 2; i <= limit; ++i)
                if (number % i == 0)
                    return false;
            return true;
        }

        public long Compute()
        {
            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day23.txt").ToArray();

            Computer<Day23Instruction> computer = new Computer<Day23Instruction>();
            computer.SetProgram(commands);

            while (computer.RunInstruction()) ;

            return Day23Instruction.NumMul;
        }


        public long Compute2()
        {
            // The program ends up doing an inefficent check for whether numbers in a range are prime
            //
            // So we instead do a quicker check...

            int numNotPrime = 0;

            for (int i = 109900; i <= 126900; i += 17)
            {
                if (!IsPrime(i))
                    numNotPrime++;
            }

            //string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day23Fixed.txt").ToArray();

            //Computer<Day23Instruction> computer = new Computer<Day23Instruction>();

            //computer.SetProgram(commands);
            //computer.SetRegister("a", 1);

            //computer.RunDebug();

            return numNotPrime;
        }
    }
}
