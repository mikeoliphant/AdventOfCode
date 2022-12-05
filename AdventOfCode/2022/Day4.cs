namespace AdventOfCode._2022
{
    internal class Day4 : Day
    {
        char[] splitChars = new char[] { ',', '-' };

        public override long Compute()
        {
            int numContained = 0;

            foreach (var pair in File.ReadLines(DataFile))
            {
                int[] sections = pair.Split(splitChars).ToInts().ToArray();

                if (sections[0] <= sections[2])
                {
                    if ((sections[3] <= sections[1]))
                    {
                        numContained++;

                        continue;
                    }
                }

                if (sections[2] <= sections[0])
                {
                    if ((sections[1] <= sections[3]))
                    {
                        numContained++;
                    }
                }
            }

            return numContained;
        }

        public override long Compute2()
        {
            int numOverlap = 0;

            foreach (var pair in File.ReadLines(DataFile))
            {
                int[] sections = pair.Split(splitChars).ToInts().ToArray();

                if (sections[0] <= sections[2])
                {
                    if ((sections[2] <= sections[1]))
                    {
                        numOverlap++;

                        continue;
                    }
                }

                if (sections[2] <= sections[0])
                {
                    if ((sections[0] <= sections[3]))
                    {
                        numOverlap++;
                    }
                }
            }

            return numOverlap;
        }
    }
}
