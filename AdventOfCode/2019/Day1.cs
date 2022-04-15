namespace AdventOfCode._2019
{
    internal class Day1
    {
        public long Compute()
        {
            long fuel = 0;

            foreach (int mass in File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day1.txt").ToInts())
            {
                fuel += (mass / 3) - 2;
            }

            return fuel;
        }

        public long Compute2()
        {
            long fuel = 0;

            foreach (int mass in File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day1.txt").ToInts())
            {
                int moduleMass = mass;

                do
                {
                    moduleMass = (moduleMass / 3) - 2;

                    if (moduleMass <= 0)
                        break;

                    fuel += moduleMass;
                }
                while (true);
            }

            return fuel;
        }
    }
}
