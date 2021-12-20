using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    internal class Day19
    {
        Vector3[][] scannedBeacons;
        List<Matrix4x4> possibleOrientations = new List<Matrix4x4>();

        void ReadInput()
        {
            string[] scannerData = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day19.txt").SplitParagraphs();

            scannedBeacons = (from scanner in scannerData select (from scan in scanner.SplitLines().Skip(1) select new Vector3(scan.ToFloats(",").ToArray().AsSpan<float>())).ToArray()).ToArray();

            List<Vector3> unitVectors = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

            foreach (Vector3 forward in unitVectors)
            {
                foreach (Vector3 up in unitVectors)
                {
                    if ((up * forward) == Vector3.Zero)
                    {
                        possibleOrientations.Add(Matrix4x4.CreateLookAt(Vector3.Zero, forward, up));
                    }
                }
            }
        }

        Vector3 Round(Vector3 vec)
        {
            return new Vector3((float)Math.Round(vec.X), (float)Math.Round(vec.Y), (float)Math.Round(vec.Z));
        }

        bool DetectOverlap(Vector3[] scan1, Matrix4x4 scanner2, Vector3[] scan2, out Vector3 location2)
        {
            location2 = Vector3.Zero;

            Matrix4x4 inv2;
            Matrix4x4.Invert(scanner2, out inv2);

            Dictionary<Vector3, int> locations = new Dictionary<Vector3, int>();

            foreach (Vector3 beacon1 in scan1)
            {
                foreach (Vector3 beacon2 in scan2)
                {
                    Vector3 loc2 = beacon1 - Round(Vector3.Transform(beacon2, inv2));

                    if (!locations.ContainsKey(loc2))
                    {
                        locations[loc2] = 1;
                    }
                    else
                    {
                        locations[loc2]++;
                    }
                }
            }

            foreach (var loc2 in locations)
            {
                if (loc2.Value >= 12)
                {
                    location2 = loc2.Key;

                    return true;
                }
            }

            return false;
        }

        public long Compute()
        {
            ReadInput();

            bool[] haveScanner = new bool[scannedBeacons.Length];
            Vector3[] scannerLocations = new Vector3[scannedBeacons.Length];
            scannerLocations[0] = Vector3.Zero;

            int scannerCount = 1;

            do
            {
                for (int scanner = 1; scanner < scannedBeacons.Length; scanner++)
                {
                    if (haveScanner[scanner])
                        continue;

                    for (int orientation = 0; orientation < possibleOrientations.Count; orientation++)
                    {
                        Vector3 relativeLocation;

                        if (DetectOverlap(scannedBeacons[0], possibleOrientations[orientation], scannedBeacons[scanner], out relativeLocation))
                        {
                            Matrix4x4 invScanner;

                            Matrix4x4.Invert(possibleOrientations[orientation], out invScanner);

                            List<Vector3> newBeacons = new List<Vector3>();

                            foreach (Vector3 beacon in scannedBeacons[scanner])
                            {
                                Vector3 absBeacon = relativeLocation + Round(Vector3.Transform(beacon, invScanner));

                                if (!scannedBeacons[0].Contains(absBeacon))
                                {
                                    newBeacons.Add(absBeacon);
                                }
                            }

                            if ((scannedBeacons[scanner].Length - newBeacons.Count) < 12)
                            {
                                //throw new Exception();
                            }

                            newBeacons.AddRange(scannedBeacons[0]);

                            scannedBeacons[0] = newBeacons.ToArray();

                            scannerLocations[scanner] = relativeLocation;
                            haveScanner[scanner] = true;
                            scannerCount++;

                            break;
                        }
                    }
                }
            }
            while (scannerCount < scannedBeacons.Length);

            int maxDist = 0;

            foreach (Vector3 s1 in scannerLocations)
            {
                foreach (Vector3 s2 in scannerLocations)
                {
                    maxDist = (int)Math.Max(maxDist, Math.Abs(s1.X - s2.X) + Math.Abs(s1.Y - s2.Y) + Math.Abs(s1.Z - s2.Z));
                }
            }

            return scannedBeacons[0].Length;
        }
    }
}
