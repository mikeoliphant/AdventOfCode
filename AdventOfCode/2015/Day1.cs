namespace AdventOfCode._2015
{
    internal class Day1
    {
        public long Compute()
        {
            string input = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2015\Day1.txt").Trim();

            int floor = 0;

            for (int pos = 0; pos < input.Length; pos++)
            {
                char c = input[pos];

                if (c == '(')
                    floor++;
                else if (c == ')')
                    floor--;

                if (floor == -1)
                    return pos + 1;
            }

            return floor;
        }
    }
}
