using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2020
{
    public class Day1
    {
        List<int> numbers;

        void ReadInput()
        {
            numbers = new List<int>(from numStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day1.txt") select int.Parse(numStr));
        }

        public int Compute()
        {
            ReadInput();

            for (int pos1 = 0; pos1 < numbers.Count; pos1++)
            {
                for (int pos2 = 0; pos2 < numbers.Count; pos2++)
                {
                    if (pos1 != pos2)
                    {
                        if ((numbers[pos1] + numbers[pos2]) == 2020)
                            return (numbers[pos1] * numbers[pos2]);
                    }
                }
            }

            throw new Exception();
        }
    }
}
