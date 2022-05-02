namespace AdventOfCode._2017
{
    internal class Day5
    {

        public long Compute()
        {
            int[] jumps = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day5.txt").ToInts().ToArray();

            //int[] jumps = new int[] { 0, 3, 0, 1, -3 };

            int instructionPos = 0;

            int step = 0;

            do
            {
                int lastPos = instructionPos;

                instructionPos += jumps[instructionPos];

                jumps[lastPos]++;

                step++;
            }
            while (instructionPos < jumps.Length);

            return step;
        }

        public long Compute2()
        {
            int[] jumps = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day5.txt").ToInts().ToArray();

            int instructionPos = 0;

            int step = 0;

            do
            {
                int lastPos = instructionPos;

                instructionPos += jumps[instructionPos];

                if (jumps[lastPos] >= 3)
                {
                    jumps[lastPos]--;
                }
                else
                {
                    jumps[lastPos]++;
                }

                step++;
            }
            while (instructionPos < jumps.Length);

            return step;
        }
    }
}
