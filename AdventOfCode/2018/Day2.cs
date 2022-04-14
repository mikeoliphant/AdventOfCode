using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day2
    {
        public long Compute()
        {
            string[] boxIDs = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day2.txt").ToArray();

            Dictionary<char, int> letterCount = new Dictionary<char, int>();

            int num2 = 0;
            int num3 = 0;

            foreach (string boxID in boxIDs)
            {
                letterCount.Clear();

                foreach (char c in boxID)
                {
                    if (!letterCount.ContainsKey(c))
                    {
                        letterCount[c] = 1;
                    }
                    else
                    {
                        letterCount[c]++;
                    }
                }

                if (letterCount.Values.Count(c => c == 2) > 0)
                    num2++;

                if (letterCount.Values.Count(c => c == 3) > 0)
                    num3++;
            }

            return num2 * num3;
        }

        public long Compute2()
        {
            string[] boxIDs = File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day2.txt").ToArray();

            
            for (int pos1 = 0; pos1 < boxIDs.Length; pos1++)
            {
                for (int pos2 = 0; pos2 < boxIDs.Length; pos2++)
                {
                    if (pos1 != pos2)
                    {
                        int numMiss = 0;

                        int missPos = 0;

                        for (int c = 0; c < boxIDs[pos1].Length; c++)
                        {
                            if (boxIDs[pos1][c] != boxIDs[pos2][c])
                            {
                                missPos = c;

                                numMiss++;

                                if (numMiss > 1)
                                    break;
                            }
                        }

                        if (numMiss == 1)
                        {
                            string common = boxIDs[pos1].Substring(0, missPos) + boxIDs[pos1].Substring(missPos + 1);
                        }
                    }
                }
            }


            return 0;
        }
    }
}
