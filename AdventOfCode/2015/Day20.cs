namespace AdventOfCode._2015
{
    internal class Day20 : Day
    {
        public override long Compute()
        {
            long presents = 29000000;

            int house = 1;

            do
            {
                long housePresents = 0;

                foreach (int factor in FactorHelper.AllFactors(house))
                {
                    housePresents += (factor * 10);
                }

                if (housePresents >= presents)
                    return house;

                house++;
            }
            while (true);
        }

        Dictionary<int, int> numHousesForElf = new Dictionary<int, int>();

        bool IsDelivering(int elf)
        {
            if (!numHousesForElf.ContainsKey(elf))
            {
                numHousesForElf[elf] = 1;

                return true;
            }

            if (numHousesForElf[elf] == 50)
                return false;

            numHousesForElf[elf]++;

            return true;
        }

        public override long Compute2()
        {
            long presents = 29000000;

            int house = 1;

            do
            {
                long housePresents = 0;

                foreach (int factor in FactorHelper.AllFactors(house))
                {
                    if (IsDelivering(factor))
                    {
                        housePresents += (factor * 11);
                    }
                }

                if (housePresents >= presents)
                    return house;

                house++;
            }
            while (true);
        }
    }
}
