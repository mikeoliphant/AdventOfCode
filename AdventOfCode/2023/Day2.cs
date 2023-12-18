using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day2 : Day
    {
        Dictionary<string, int> colDict = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

        bool IsValid(string game)
        {
            string[] pulls = game.Split(':')[1].Split(';');

            foreach (string pull in pulls)
            {
                string[] colors = pull.Split(",");

                foreach (string color in colors)
                {
                    string[] numCol = color.Trim().Split(' ');

                    int num = int.Parse(numCol[0]);
                    string col = numCol[1];

                    if (num > colDict[col])
                        return false;
                }
            }

            return true;
        }

        public override long Compute()
        {
            long sum = 0;

            long id = 1;

            foreach (string game in File.ReadLines(DataFile))
            {
                if (IsValid(game))
                {
                    sum += id;
                }

                id++;
            }

            return sum;
        }

        public override long Compute2()
        {
            long sum = 0;

            foreach (string game in File.ReadLines(DataFile))
            {
                Dictionary<string, int> maxDict = new Dictionary<string, int>();

                string[] pulls = game.Split(':')[1].Split(';');

                foreach (string pull in pulls)
                {
                    string[] colors = pull.Split(",");

                    foreach (string color in colors)
                    {
                        string[] numCol = color.Trim().Split(' ');

                        int num = int.Parse(numCol[0]);
                        string col = numCol[1];

                        int current = 0;

                        maxDict.TryGetValue(col, out current);

                        maxDict[col] = Math.Max(num, current);
                    }
                }

                long pow = 1;

                foreach (int num in maxDict.Values)
                {
                    pow *= num;
                }

                sum += pow;
            }

            return sum;
        }
    }
}
