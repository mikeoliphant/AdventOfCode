using Microsoft.VisualBasic.Logging;

namespace AdventOfCode._2023
{
    internal class Day15 : Day
    {
        int Hash(string str)
        {
            int hash = 0;

            foreach (char c in str)
            {
                hash += c;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }

        public override long Compute()
        {
            long sum = 0;

            foreach (string str in File.ReadAllText(DataFile).Trim().Split(','))
            {
                sum += Hash(str);
            }

            return sum;
        }

        public override long Compute2()
        {
            List<string>[] lenses = new List<string>[256];

            for (int i = 0; i < 256; i++)
                lenses[i] = new List<string>();

            foreach (string str in File.ReadAllText(DataFile).Trim().Split(','))
            {
                string[] split = str.Split('=', '-');

                var box = lenses[Hash(split[0])];

                if (split[1].Length == 0)
                {
                    box.RemoveAll(b => b.StartsWith(split[0]));
                }
                else
                {
                    int index = box.FindIndex(b => b.StartsWith(split[0]));

                    if (index != -1)
                    {
                        box.RemoveAt(index);
                        box.Insert(index, split[0] + " " + split[1]);
                    }
                    else
                    {
                        box.Add(split[0] + " " + split[1]);
                    }
                }
            }

            long power = 0;

            for (int box = 0; box < lenses.Length; box++)
            {
                for (int slot = 0; slot < lenses[box].Count; slot++)
                {
                    power += int.Parse(lenses[box][slot].Split(' ')[1]) * (box + 1) * (slot + 1);
                }
            }

            return power;
        }
    }
}
