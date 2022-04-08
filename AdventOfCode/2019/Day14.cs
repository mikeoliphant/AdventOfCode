using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AdventOfCode._2019
{
    internal class Day14
    {
        Dictionary<string, Tuple<int, List<Tuple<int, string>>>> reactions = new Dictionary<string, Tuple<int, List<Tuple<int, string>>>>();

        void ReadInput()
        {
            foreach (string reactionStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day14.txt"))
            {
                string[] split = reactionStr.Split("=>");

                string[] componentStr = split[0].Split(',');

                List<Tuple<int, string>> components = new List<Tuple<int, string>>();

                foreach (string component in componentStr)
                {
                    string[] amountChem = component.Trim().Split(' ');

                    components.Add(new Tuple<int, string>(int.Parse(amountChem[0]), amountChem[1].Trim()));
                }

                string[] produceAmountChem = split[1].Trim().Split(' ');

                reactions[produceAmountChem[1].Trim()] = new Tuple<int, List<Tuple<int, string>>>(int.Parse(produceAmountChem[0]), components);
            }
        }

        public long ProduceFuel(long fuelAmount)
        {
            Dictionary<string, long> need = new Dictionary<string, long>();
            Dictionary<string, long> have = new Dictionary<string, long>();

            need["FUEL"] = fuelAmount;

            long neededOre = 0;

            while (need.Count > 0)
            {
                string needChem = need.Keys.First();
                long needAmount = need[needChem];

                if (needChem == "ORE")
                {
                    neededOre += needAmount;
                }
                else
                {
                    if (!have.ContainsKey(needChem))
                        have[needChem] = 0;

                    long haveAmount = have[needChem];

                    if (haveAmount < needAmount)
                    {
                        long multiple = (long)Math.Ceiling((float)(needAmount - haveAmount) / (float)reactions[needChem].Item1);

                        foreach (var component in reactions[needChem].Item2)
                        {
                            if (!need.ContainsKey(component.Item2))
                            {
                                need[component.Item2] = component.Item1 * multiple;
                            }
                            else
                            {
                                need[component.Item2] += component.Item1 * multiple;
                            }
                        }

                        have[needChem] += reactions[needChem].Item1 * multiple;
                    }

                    have[needChem] -= needAmount;
                }

                need.Remove(needChem);
            }

            return neededOre;
        }

        public long Compute()
        {
            ReadInput();

            return ProduceFuel(1);
        }

        public long Compute2()
        {
            ReadInput();

            long haveOre = 1000000000000;

            long lowerBound = 1;
            long upperBound = haveOre;

            do
            {
                long targetFuel = (lowerBound + upperBound) / 2;

                long requiredOre = ProduceFuel(targetFuel);

                if (requiredOre < haveOre)
                {
                    lowerBound = targetFuel;
                }
                else if (requiredOre > haveOre)
                {
                    upperBound = targetFuel;
                }
            }
            while (lowerBound != (upperBound - 1));

            return lowerBound;
        }
    }
}
