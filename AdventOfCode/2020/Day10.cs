using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2020
{
    public class Day10
    {
        List<int> adapters;

        void ReadData()
        {
            adapters = new List<int>(from numStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day10.txt") select int.Parse(numStr));

            //adapters = new List<int>(from numStr in "1, 4, 5, 6, 7, 10, 11, 12, 15, 16, 19".Split(", ") select int.Parse(numStr));

            //adapters = new List<int>(from numStr in "1, 2, 3, 4, 7, 8, 9, 10, 11, 14, 17, 18, 19, 20, 23, 24, 25, 28, 31, 32, 33, 34, 35, 38, 39, 42, 45, 46, 47, 48, 49".Split(", ") select int.Parse(numStr));

            //adapters = new List<int>(from numStr in "1, 2, 3, 4, 5".Split(", ") select int.Parse(numStr));

            adapters.Sort();

            adapters.Add(adapters[adapters.Count - 1] + 3);
        }

        public long Compute()
        {
            ReadData();

            int numOneDiffs = 0;
            int numThreeDiffs = 0;

            long lastJoltage = 0;

            foreach (long adapter in adapters)
            {
                if ((adapter - lastJoltage) == 1)
                {
                    numOneDiffs++;
                }
                else if ((adapter - lastJoltage) == 3)
                {
                    numThreeDiffs++;
                }

                lastJoltage = adapter;
            }

            return numOneDiffs * numThreeDiffs;
        }

        public int ComputePermutations(List<int> adapters)
        {
            int permutations = 0;

            for (int bits = 0; bits < Math.Pow(2, adapters.Count - 2); bits++)
            {
                if (bits == (Math.Pow(2, adapters.Count - 2) - 1))
                {
                }

                List<int> testList = new List<int>();

                for (int pos = 0; pos < adapters.Count; pos++)
                {
                    if ((pos == 0) || (pos == (adapters.Count - 1) || (bits & (1 << (pos - 1))) != 0))
                    {
                        testList.Add(adapters[pos]);
                    }
                }

                bool isGood = true;

                int lastValue = testList[0];

                foreach (int adapter in testList)
                {
                    if ((adapter - lastValue) > 3)
                    {
                        isGood = false;

                        break;
                    }

                    lastValue = adapter;
                }

                if (isGood)
                    permutations++;
            }

            return permutations;
        }

        public long Compute2()
        {
            ReadData();

            adapters.Insert(0, 0);

            List<int> subList = new List<int>();

            long val = 1;

            for (int i = 0; i < adapters.Count; i++)
            {
                if ((i > 0) && ((adapters[i] - adapters[i - 1]) >= 3))
                {
                    subList.Add(adapters[i]);

                    if (subList.Count > 2)
                    {
                        int permutations = ComputePermutations(subList);

                        val *= permutations;
                    }

                    subList.Clear();
                    subList.Add(adapters[i]);
                }
                else
                {
                    subList.Add(adapters[i]);
                }
            }

            return val;
        }
    }
}
