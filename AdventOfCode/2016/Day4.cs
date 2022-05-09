using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2016
{
    internal class Day4
    {
        public long Compute()
        {
            long sectorSum = 0;

            foreach (string room in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day4.txt"))
            {
                var match = Regex.Match(room, @"(.*)-(.*)\[(.*)\]$");

                string name = match.Groups[1].Value;

                string checksum = match.Groups[3].Value;

                string top5 = new string(name.Replace("-", "").GroupBy(c => c).OrderByDescending(g => g.Count()).ThenBy(g => g.Key).Take(5).Select(g => g.Key).ToArray());

                if (top5 == checksum)
                {
                    sectorSum += int.Parse(match.Groups[2].Value);
                }
            }

            return sectorSum;
        }

        public long Compute2()
        {
            foreach (string room in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day4.txt"))
            {
                var match = Regex.Match(room, @"(.*)-(.*)\[(.*)\]$");

                string name = match.Groups[1].Value;

                string checksum = match.Groups[3].Value;

                string top5 = new string(name.Replace("-", "").GroupBy(c => c).OrderByDescending(g => g.Count()).ThenBy(g => g.Key).Take(5).Select(g => g.Key).ToArray());

                if (top5 == checksum)
                {
                    int sectorID = int.Parse(match.Groups[2].Value);

                    string decrypted = new string(name.Select(c => (char)('a' + (((c - 'a') + sectorID) % 26))).ToArray());

                    if (decrypted.Contains("north"))
                    {
                        return sectorID;
                    }
                }

            }

            throw new InvalidOperationException();
        }
    }
}
