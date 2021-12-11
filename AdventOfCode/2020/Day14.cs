using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day14
    {
        string[] cmds;

        void ReadInput()
        {
            cmds = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day14.txt").ToArray();
        }

        public long Compute()
        {
            Dictionary<long, long> memory = new Dictionary<long, long>();

            ReadInput();

            string mask = "";

            foreach (string cmd in cmds)
            {
                string[] split = cmd.Split(' ');

                if (split[0] == "mask")
                {
                    mask = split[2];
                }
                else
                {
                    long val = long.Parse(split[2]);

                    long address = long.Parse(split[0].Split(new char[] { '[', ']' })[1]);

                    for (int i = 0; i < mask.Length; i++)
                    {
                        char bitChar = mask[mask.Length - (i + 1)];

                        if (bitChar == '1')
                        {
                            val |= ((long)1 << i);
                        }
                        else if (bitChar == '0')
                        {
                            val &= ~((long)1 << i);
                        }

                        memory[address] = val;
                    }
                }
            }

            long sum = 0;

            foreach (long val in memory.Values)
            {
                sum += val;
            }

            return sum;
        }

        Dictionary<long, long> memory = new Dictionary<long, long>();

        void WriteMemory(char[] mask, long value)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    char[] newMask = mask.Clone() as char[];
                    newMask[i] = '0';

                    WriteMemory(newMask, value);

                    newMask = mask.Clone() as char[];
                    newMask[i] = '1';

                    WriteMemory(newMask, value);

                    return;
                }
            }

            memory[Convert.ToInt64(new string(mask), 2)] = value;
        }

        public long Compute2()
        {
            var writeList = new List<KeyValuePair<string, long>>();

            ReadInput();

            string mask = "";

            foreach (string cmd in cmds)
            {
                string[] split = cmd.Split(' ');

                if (split[0] == "mask")
                {
                    mask = split[2];
                }
                else
                {
                    long val = long.Parse(split[2]);

                    long address = long.Parse(split[0].Split(new char[] { '[', ']' })[1]);

                    char[] addressMask = Convert.ToString(address, 2).PadLeft(36, '0').ToCharArray();

                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == 'X')
                            addressMask[i] = 'X';
                        else if (mask[i] == '1')
                            addressMask[i] = '1';
                    }

                    writeList.Add(new KeyValuePair<string, long>(new string(addressMask), val));
                }
            }

            foreach (var write in writeList)
            {
                WriteMemory(write.Key.ToCharArray(), write.Value);
            }

            long sum = 0;

            foreach (long value in memory.Values)
            {
                sum += value;
            }

            return sum;
        }
    }
}
