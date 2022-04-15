namespace AdventOfCode._2019
{
    internal class Day21
    {
        IntcodeComputer computer;

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day21.txt").ToLongs(',').ToArray();

            computer = new IntcodeComputer();
            computer.SetProgram(program);
        }

        void AddInput(string input)
        {
            foreach (char c in input)
                computer.AddInput(c);

            computer.AddInput('\n');
        }

        public long Compute()
        {
            ReadInput();

            AddInput("NOT A J");
            AddInput("NOT C T");
            AddInput("OR T J");
            AddInput("AND D J");


            AddInput("WALK");

            while (computer.RunUntilOutput())
            {
                long output = computer.GetLastOutput();

                if (output > char.MaxValue)
                {
                    return output;
                }

                Console.Write((char)computer.GetLastOutput());
            }

            throw new InvalidOperationException();
        }

        public long Compute2()
        {
            ReadInput();

            AddInput("NOT E T");
            AddInput("NOT T T");
            AddInput("OR H T");

            AddInput("NOT B J");
            AddInput("NOT J J");
            AddInput("AND C J");
            AddInput("NOT J J");
            AddInput("AND D J");

            AddInput("AND T J");

            AddInput("NOT A T");
            AddInput("OR T J");

            AddInput("RUN");

            while (computer.RunUntilOutput())
            {
                long output = computer.GetLastOutput();

                if (output > char.MaxValue)
                {
                    return output;
                }

                Console.Write((char)computer.GetLastOutput());
            }

            throw new InvalidOperationException();
        }
    }
}
