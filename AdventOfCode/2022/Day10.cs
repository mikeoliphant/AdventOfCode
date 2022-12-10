namespace AdventOfCode._2022
{
    internal class Day10 : Day
    {
        class Day10Instruction : ComputerInstruction
        {
            string cmd;
            string arg = null;

            public Day10Instruction(string instructionString, IComputer computer)
                : base(instructionString, computer)
            {
                string[] split = instructionString.Split(' ', 2);

                cmd = split[0];

                if (cmd == "addx")
                    NumCycles = 2;

                if (split.Length > 1)
                {
                    arg = split[1];
                }
            }

            public override void Execute()
            {
                switch (cmd)
                {
                    case "addx":
                        Computer.SetRegister("X", Computer.GetRegister("X") + int.Parse(arg));
                        break;

                    case "noop":
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                base.Execute();
            }
        }

        public override long Compute()
        {
            Computer<Day10Instruction> computer = new Computer<Day10Instruction>();

            computer.SetProgram(File.ReadLines(DataFile));

            computer.SetRegister("X", 1);

            long signalSum = 0;

            while (computer.AdvanceCycle())
            {
                if (((computer.CurrentCycle - 20) % 40) == 0)
                {
                    long signal = computer.CurrentCycle * computer.GetRegister("X");
                    signalSum += signal;
                }
            }

            return signalSum;
        }

        public override long Compute2()
        {
            Computer<Day10Instruction> computer = new Computer<Day10Instruction>();

            computer.SetProgram(File.ReadLines(DataFile));

            computer.SetRegister("X", 1);

            Grid<char> grid = new Grid<char>(40, 6);
            grid.Fill(' ');

            long signalSum = 0;

            do
            {
                int gridPos = (int)((computer.CurrentCycle - 1) % (grid.Width * grid.Height));

                int gridY = gridPos / 40;
                int gridX = gridPos % 40;

                int spritePos = (int)(computer.GetRegister("X"));

                if (Math.Abs(spritePos - gridX) < 2)
                {
                    grid[gridX, gridY] = '#';
                }
            }
            while (computer.AdvanceCycle());

            grid.PrintToConsole();

            return 0;
        }
    }
}
