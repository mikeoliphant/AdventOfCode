using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    internal static class KnotHash
    {
        public static string ComputeHash(string input)
        {
            input += new string(new char[] { (char)17, (char)31, (char)73, (char)47, (char)23 });

            int size = 256;

            int[] list = new int[size];

            for (int i = 0; i < size; i++)
            {
                list[i] = i;
            }

            int skip = 0;
            int pos = 0;

            for (int round = 0; round < 64; round++)
            {
                for (int length = 0; length < input.Length; length++)
                {
                    int reverseSize = input[length] / 2;

                    for (int reverse = 0; reverse < reverseSize; reverse++)
                    {
                        int index1 = (pos + reverse) % size;
                        int index2 = (pos + input[length] - reverse - 1) % size;

                        int tmp = list[index1];
                        list[index1] = list[index2];
                        list[index2] = tmp;
                    }

                    pos = (pos + input[length] + skip) % size;
                    skip++;
                }
            }

            int[] denseHash = new int[16];

            for (int hash = 0; hash < 16; hash++)
            {
                denseHash[hash] = 0;
                int offset = hash * 16;

                for (int hashPos = 0; hashPos < 16; hashPos++)
                {
                    denseHash[hash] ^= list[offset + hashPos];
                }
            }

            return string.Join("", denseHash.Select(h => h.ToString("X2"))).ToLower();
        }
    }
}
