namespace AdventOfCode._2019
{
    internal class Day2
    {
        public int RunProgram(int[] input)
        {
            int opCode = 0;
            int currentPos = 0;

            do
            {
                opCode = input[currentPos];

                switch (opCode)
                {
                    case 1:
                        input[input[currentPos + 3]] = input[input[currentPos + 1]] + input[input[currentPos + 2]];
                        break;
                    case 2:
                        input[input[currentPos + 3]] = input[input[currentPos + 1]] * input[input[currentPos + 2]];
                        break;
                }

                currentPos += 4;
            }
            while (opCode != 99);

            return input[0];
        }

        public long Compute()
        {
            int[] input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day2.txt").ToInts(',').ToArray();

            input[1] = 12;
            input[2] = 2;

            return RunProgram(input);
        }

        public long Compute2()
        {
            int[] initialInput = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day2.txt").ToInts(',').ToArray();

            for (int pos1 = 0; pos1 <= 99; pos1++)
            {
                for (int pos2 = 0; pos2 <= 99; pos2++)
                {
                    int[] input = initialInput.Clone() as int[];

                    input[1] = pos1;
                    input[2] = pos2;

                    if (RunProgram(input) == 19690720)
                    {
                        return (pos1 * 100) + pos2;
                    }
                }
            }

            throw new InvalidOperationException();
        }
    }
}
