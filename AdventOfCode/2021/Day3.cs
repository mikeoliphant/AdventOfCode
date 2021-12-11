using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode._2021
{
    public class Day3
    {
        const int NumBits = 12;

        List<string> ReadInput()
        {
            return new List<string>(File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\DayThreeInput.txt"));
        }

        public void Compute()
        {
            List<string> inputLines = ReadInput();

            int[,] bitHistogram = new int[NumBits, 2];

            foreach (string inputLine in inputLines)
            {
                for (int pos = 0; pos < NumBits; pos++)
                {
                    int val = (inputLine[pos] == '1') ? 1 : 0;

                    bitHistogram[pos, val]++;
                }
            }

            int gamma = 0;
            int epsilon = 0;

            for (int pos = 0; pos < NumBits; pos++)
            {
                if (bitHistogram[pos, 1] > bitHistogram[pos, 0])
                {
                    gamma += 1 << ((NumBits - 1) - pos);
                }
                else
                {
                    epsilon += 1 << ((NumBits - 1) - pos);
                }
            }

            string gammaString = Convert.ToString(gamma, 2);
            string epsilonString = Convert.ToString(epsilon, 2);

            int power = gamma * epsilon;
        }

        public void Compute2()
        {
            List<string> inputLines = ReadInput();

            string o2String = GetRatingString(inputLines, mostCommon: true);
            string co2String = GetRatingString(inputLines, mostCommon: false);

            int o2Rating = Convert.ToInt32(o2String, 2);
            int co2Rating = Convert.ToInt32(co2String, 2);

            int lifeSupport = o2Rating * co2Rating;
        }

        string GetRatingString(List<string> inputLines, bool mostCommon)
        {

            for (int bitPos = 0; bitPos < NumBits; bitPos++)
            {
                char rating = GetRating(inputLines, bitPos, mostCommon) ? '1' : '0';

                inputLines = new List<string>(from line in inputLines where line[bitPos] == rating select line);

                if (inputLines.Count == 1)
                    break;
            }

            return inputLines[0];
        }

        bool GetRating(List<string> inputLines, int pos, bool mostCommon)
        {
            int numZeros = 0;
            int numOnes = 0;

            foreach (string inputLine in inputLines)
            {
                if (inputLine[pos] == '0')
                    numZeros++;
                else
                    numOnes++;
            }

            return mostCommon ? (numOnes >= numZeros) : (numOnes < numZeros);
        }
    }
}
