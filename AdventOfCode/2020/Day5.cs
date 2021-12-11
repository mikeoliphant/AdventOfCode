using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day5
    {
        string[] boardingPasses;

        void ReadInput()
        {
            boardingPasses = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day5.txt").ToArray();
        }

        int BinaryPartition(int size, string cmds, string chars)
        {
            int start = 0;
            int end = size;

            foreach (char c in cmds)
            {
                if (c == chars[0])
                {
                    end -= (end - start) / 2;
                }
                else if (c == chars[1])
                {
                    start += (end - start) / 2;
                }
            }

            return start;
        }

        public long Compute()
        {
            ReadInput();

            int maxID = 0;

            foreach (string boardingPass in boardingPasses)
            {
                string fb = boardingPass.Substring(0, 7);
                string rl = boardingPass.Substring(7, 3);

                int row = BinaryPartition(128, fb, "FB");
                int col = BinaryPartition(8, rl, "LR");

                int id = (row * 8) + col;

                maxID = Math.Max(maxID, id);
            }

            return maxID;
        }

        public long Compute2()
        {
            ReadInput();

            List<int> ids = new List<int>();

            foreach (string boardingPass in boardingPasses)
            {
                string fb = boardingPass.Substring(0, 7);
                string rl = boardingPass.Substring(7, 3);

                int row = BinaryPartition(128, fb, "FB");
                int col = BinaryPartition(8, rl, "LR");

                int id = (row * 8) + col;

                ids.Add(id);
            }

            ids.Sort();

            int lastID = -1;

            foreach (int id in ids)
            {
                if ((lastID != -1) && (id != (lastID + 1)))
                    return (id - 1);

                lastID = id;
            }

            throw new Exception();
        }
    }
}
