using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day4
    {
        char[] globalMin = "356261".ToArray();
        char[] globalMax = "846303".ToArray();

        void Increment(char[] digits, int pos)
        {
            int length = digits.Length;

            while (pos >= 0)
            {
                digits[pos]++;

                if (digits[pos] > '9')
                {
                    for (int pos2 = pos; pos2 < length; pos2++)
                        digits[pos2] = '0';

                    pos--;
                }
                else
                {
                    break;
                }
            }

        }

        public long Compute()
        {
            long numMatches = 0;

            char[] current = globalMin;

            int length = globalMin.Length;

            do
            {
                if (current[0] == '0')
                    break;

                bool matchMax = true;
                int dupeInARow = 0;
                bool haveDupe = false;

                for (int pos = 0; pos < length;)
                {
                    if (matchMax)
                    {
                        if (current[pos] > globalMax[pos])
                        {
                            current[0] = '0';

                            break;
                        }

                        if (current[pos] < globalMax[pos])
                            matchMax = false;
                    }

                    if ((pos > 0) && (current[pos] == current[pos - 1]))
                    {
                        dupeInARow++;
                    }
                    else
                    {
                        if (dupeInARow == 1)
                        {
                            haveDupe = true;
                        }

                        dupeInARow = 0;
                    }

                    if ((pos > 0) && (current[pos] < current[pos - 1]))
                    {
                        Increment(current, pos);

                        pos = 0;

                        matchMax = true;
                        haveDupe = false;
                        dupeInARow = 0;

                        continue;
                    }
                    else
                    {
                        pos++;
                    }
                }

                if (current[0] == '0')
                    break;

                if (haveDupe || (dupeInARow == 1))
                {
                    Console.WriteLine(current);

                    numMatches++;
                }

                Increment(current, length - 1);
            }
            while (true);

            return numMatches;
        }
    }
}
