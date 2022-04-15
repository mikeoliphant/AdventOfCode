namespace AdventOfCode._2020
{
    public class Day8
    {
        string[] instructions;

        int instructionPos = 0;
        int acc = 0;

        void ReadData()
        {
            instructions = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day8.txt").ToArray();
        }

        bool CheckLoop()
        {
            Dictionary<int, bool> visitedInstructions = new Dictionary<int, bool>();

            instructionPos = 0;
            acc = 0;

            do
            {
                if (visitedInstructions.ContainsKey(instructionPos))
                    return true;

                string instruction = instructions[instructionPos];

                visitedInstructions[instructionPos] = true;

                string[] instVal = instruction.Split(' ');

                int val = int.Parse(instVal[1]);

                switch (instVal[0])
                {
                    case "jmp":
                        instructionPos += val;
                        continue;

                    case "acc":
                        acc += val;
                        break;
                }

                instructionPos++;
            }
            while (instructionPos < (instructions.Length - 1));

            return false;
        }

        public long Compute()
        {
            ReadData();

            CheckLoop();

            return acc;
        }

        public long Compute2()
        {
            ReadData();

            for (int pos = 0; pos < instructions.Length; pos++)
            {
                if (instructions[pos].StartsWith("jmp"))
                {
                    instructions[pos] = instructions[pos].Replace("jmp", "nop");
                }
                else if (instructions[pos].StartsWith("nop"))
                {
                    instructions[pos] = instructions[pos].Replace("nop", "jmp");
                }
                else
                {
                    continue;
                }

                if (!CheckLoop())
                    break;

                if (instructions[pos].StartsWith("jmp"))
                {
                    instructions[pos] = instructions[pos].Replace("jmp", "nop");
                }
                else if (instructions[pos].StartsWith("nop"))
                {
                    instructions[pos] = instructions[pos].Replace("nop", "jmp");
                }
            }

            return acc;
        }
    }
}
