using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AdventOfCode._2021
{
    internal class Day22
    {
        List<KeyValuePair<bool, BoundingBox>> boxes = new List<KeyValuePair<bool, BoundingBox>>();

        void ReadInput()
        {
            foreach (string cmd in File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day22.txt"))
            {
                ExecuteCommand(cmd);
            }
        }

        void ExecuteCommand(string cmd)
        {
            string[] lr = cmd.Split(' ');

            string[] xyz = lr[1].Split(',');

            long[] minVals = new long[3];
            long[] maxVals = new long[3];

            for (int i = 0; i < xyz.Length; i++)
            {
                string[] minMax = xyz[i].Substring(2).Split("..");

                minVals[i] = long.Parse(minMax[0]);
                maxVals[i] = long.Parse(minMax[1]);
            }

            bool on = (lr[0] == "on");

            boxes.Add(new KeyValuePair<bool, BoundingBox>(on, new BoundingBox(minVals, maxVals)));
        }

        public long Compute()
        {
            ReadInput();

            Dictionary<string, bool> cubes = new Dictionary<string, bool>();

            foreach (var box in boxes)
            {
                bool inRegion = true;

                for (int i = 0; i < 3; i++)
                {
                    if ((box.Value.Min[i] < -50) || (box.Value.Max[i] > 50))
                    {
                        inRegion = false;

                        break;
                    }
                }

                if (!inRegion)
                    continue;

                for (long x = box.Value.Min[0]; x <= box.Value.Max[0]; x++)
                {
                    for (long y = box.Value.Min[1]; y <= box.Value.Max[1]; y++)
                    {
                        for (long z = box.Value.Min[2]; z <= box.Value.Max[2]; z++)
                        {
                            string hash = x + "-" + y + "-" + z;

                            if (box.Key)
                            {
                                cubes[hash] = true;
                            }
                            else
                            {
                                cubes.Remove(hash);
                            }
                        }
                    }
                }
            }

            return cubes.Count;
        }

        public long Compute2()
        {
            ReadInput();

            List<BoundingBox> onRegions = new List<BoundingBox>();

            foreach (var box in boxes)
            {
                List<BoundingBox> newRegions = new List<BoundingBox>();

                foreach (BoundingBox existingRegion in onRegions)
                {
                    BoundingBox intersection;

                    if (BoundingBox.GetIntersection(box.Value, existingRegion, out intersection))
                    {
                        List<BoundingBox> slices = existingRegion.Slice(intersection);

                        foreach (BoundingBox slice in slices)
                        {
                            if (!slice.Equals(intersection))
                            {
                                newRegions.Add(slice);
                            }
                        }
                    }
                    else
                    {
                        newRegions.Add(existingRegion);
                    }
                }

                if (box.Key)
                {
                    newRegions.Add(box.Value);
                }

                onRegions = newRegions;
            }

            long numOn = 0;

            foreach (BoundingBox region in onRegions)
            {
                numOn += region.GetVolume();
            }

            return numOn;
        }
    }
}
