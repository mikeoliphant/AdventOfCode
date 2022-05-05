namespace AdventOfCode._2017
{
    internal class Day25
    {
        struct TuringInstruction
        {
            public int ToWrite { get; set; }
            public int MoveDir { get; set; }
            public char NewState { get; set; }
        }

        Dictionary<char, TuringInstruction[]> instructions = new Dictionary<char, TuringInstruction[]>();
        char currentState;
        int currentPos;
        int checksumAfter;
        SparseGrid<int> tape = new SparseGrid<int>();

        void ReadInput()
        {
            var input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day25.txt").SplitParagraphs();

            var initialState = input[0].SplitLines();

            currentState = initialState[0][initialState[0].Length - 2];

            checksumAfter = int.Parse(initialState[1].Split(' ')[5]);

            foreach (string instructionSet in input.Skip(1))
            {
                char state = instructionSet[9];

                string[] lines = instructionSet.SplitLines().Skip(1).ToArray();

                int pos = 0;

                instructions[state] = new TuringInstruction[2];

                for (int value = 0; value < 2; value++)
                {
                    TuringInstruction instruction = new TuringInstruction();

                    pos++;
                    instruction.ToWrite = lines[pos][lines[pos].Length - 2] - '0';
                    pos++;

                    instruction.MoveDir = lines[pos++].Trim().Split(' ')[6].StartsWith("left") ? -1 : 1;
                    instruction.NewState = lines[pos][lines[pos].Length - 2];
                    pos++;

                    instructions[state][value] = instruction;
                }
            }
        }

        void RunMachine()
        {
            int currentValue = 0;

            tape.TryGetValue(currentPos, 0, out currentValue);

            TuringInstruction instruction = instructions[currentState][currentValue];

            tape[currentPos, 0] = instruction.ToWrite;
            currentPos += instruction.MoveDir;
            currentState = instruction.NewState;
        }

        public long Compute()
        {
            ReadInput();

            for (int step = 0; step < checksumAfter; step++)
            {
                RunMachine();
            }

            return tape.CountValue(1);
        }
    }
}
