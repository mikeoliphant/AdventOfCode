using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day1 : Day
    {
        List<int> list1 = new();
        List<int> list2 = new();

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] vals = ParseHelper.SplitWhitespace(line);

                list1.Add(int.Parse(vals[0]));
                list2.Add(int.Parse(vals[1]));
            }
        }

        public override long Compute()
        {
            ReadData();

            list1.Sort();
            list2.Sort();

            long dist = 0;

            for (int pos = 0; pos < list1.Count; pos++)
            {
                dist += Math.Abs(list1[pos] - list2[pos]);
            }

            return dist;
        }

        public override long Compute2()
        {
            ReadData();

            long similarity = 0;

            foreach (int val1 in list1)
            {
                similarity += (list2.FindAll(x  => x == val1).Count() * val1);
            }

            return similarity;
        }
    }
}
