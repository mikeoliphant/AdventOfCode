
namespace AdventOfCode._2025
{
    internal class Day1 : Day
    {
        public override long Compute()
        {
            int dialPos = 50;
            int numZeros = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                int dist = int.Parse(line.Substring(1));

                if (line[0] == 'L')
                {
                    dist = -dist;
                }

                dialPos = ModHelper.PosMod(dialPos + dist, 100);

                if (dialPos == 0)
                    numZeros++;
            }

            return numZeros;
        }

        public override long Compute2()
        {
            int dialPos = 50;
            int numZeros = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                int dist = int.Parse(line.Substring(1));

                if (line[0] == 'L')
                {
                    dist = -dist;
                }

                for (int i = 0; i < Math.Abs(dist); i++)
                {
                    dialPos = ModHelper.PosMod(dialPos + Math.Sign(dist), 100);

                    if (dialPos == 0)
                        numZeros++;
                }
            }

            return numZeros;
        }
    }
}
