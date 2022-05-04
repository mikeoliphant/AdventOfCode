namespace AdventOfCode._2017
{
    internal class Day15
    {
        public long Compute()
        {
            long genA = 703;
            long genB = 516;

            long numMatches = 0;

            for (int i = 0; i < 40000000; i++)
            {
                genA = (genA * 16807) % 2147483647;
                genB = (genB * 48271) % 2147483647;

                if ((genA & 0xFFFF) == (genB & 0xFFFF))
                {
                    numMatches++;
                }
            }

            return numMatches;
        }

        public long Compute2()
        {
            //long genA = 65;
            //long genB = 8921;
            long genA = 703;
            long genB = 516;

            long numMatches = 0;

            bool aReady = false;
            bool bReady = false;

            int numPairs = 0;

            do
            {
                if (!aReady)
                {
                    genA = (genA * 16807) % 2147483647;
                    aReady = (genA % 4) == 0;
                }

                if (!bReady)
                {
                    genB = (genB * 48271) % 2147483647;
                    bReady = (genB % 8) == 0;
                }

                if (aReady && bReady)
                {
                    numPairs++;

                    if ((genA & 0xFFFF) == (genB & 0xFFFF))
                    {
                        numMatches++;
                    }

                    aReady = false;
                    bReady = false;
                }
            }
            while (numPairs < 5000000);

            return numMatches;
        }
    }
}
