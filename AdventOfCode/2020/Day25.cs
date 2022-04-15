namespace AdventOfCode._2020
{
    internal class Day25
    {
        long cardKey = 13233401; //5764801;
        long doorKey = 6552760; //17807724;

        long FindLoopSize(long subjectNumber, long result)
        {
            long loop = 0;
            long val = 1;

            do
            {
                val = (val * subjectNumber) % 20201227;

                loop++;
            }
            while (val != result);

            return loop;
        }

        long TransForm(long subjectNumber, long loopSize)
        {
            long val = 1;

            for (long loop = 0; loop < loopSize; loop++)
            {
                val = (val * subjectNumber) % 20201227;
            }

            return val;

        }

        public long Compute()
        {
            long cardLoop = FindLoopSize(7, cardKey);
            long doorLoop = FindLoopSize(7, doorKey);

            long cardEncrypt = TransForm(doorKey, cardLoop);
            long doorEncrypt = TransForm(cardKey, doorLoop);

            if (cardEncrypt != doorEncrypt)
                throw new Exception();

            return cardEncrypt;
        }
    }
}
