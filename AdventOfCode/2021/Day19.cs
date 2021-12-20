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
            string[] scannerData = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day19Test.txt").SplitParagraphs();

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

            Vector3[] scan1 = new Vector3[] { new Vector3(-618, -824, -621) };
            Vector3[] scan2 = new Vector3[] { new Vector3(686, 422, 578) };

            Dictionary<int, Vector3> relativeLocations = new Dictionary<int, Vector3>();
            Dictionary<int, Vector3> absoluteLocations = new Dictionary<int, Vector3>();
            Dictionary<int, int> relativeTo = new Dictionary<int, int>();
            Dictionary<int, Matrix4x4> relativeOrientations = new Dictionary<int, Matrix4x4>();
            Dictionary<int, Matrix4x4> absoluteOrientations = new Dictionary<int, Matrix4x4>();

            relativeLocations[0] = Vector3.Zero;
            absoluteLocations[0] = Vector3.Zero;
            relativeOrientations[0] = Matrix4x4.Identity;
            absoluteOrientations[0] = Matrix4x4.Identity;

            for (int o = 0; o < possibleOrientations.Count; o++)
            {
                Vector3 trans = Vector3.Transform(new Vector3(1, 2, 3), possibleOrientations[o]);
            }

            Vector3 blah = Vector3.Transform(new Vector3(1, 2, 3), possibleOrientations[22]);

            do
            {
                for (int newScan = 0; newScan < scannedBeacons.Length; newScan++)
                {
                    if (!relativeLocations.ContainsKey(newScan))
                    {
                        for (int existingScan = 0; existingScan < scannedBeacons.Length; existingScan++)
                        {
                            if (newScan == existingScan)
                                continue;

                            if (relativeLocations.ContainsKey(existingScan))
                            {
                                Vector3 relativeLocation = Vector3.Zero;

                                foreach (Matrix4x4 orientation in possibleOrientations)
                                {
                                    if (DetectOverlap(scannedBeacons[existingScan], orientation, scannedBeacons[newScan], out relativeLocation))
                                    {
                                        relativeTo[newScan] = existingScan;
                                        relativeLocations[newScan] = relativeLocation;
                                        relativeOrientations[newScan] = orientation;

                                        absoluteLocations[newScan] = absoluteLocations[existingScan] + Vector3.Transform(relativeLocations[newScan], relativeOrientations[existingScan]);
                                        absoluteOrientations[newScan] = relativeOrientations[newScan] * absoluteOrientations[existingScan];

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            while (absoluteLocations.Count < scannedBeacons.Length);

            Dictionary<Vector3, bool> beaconLocations = new Dictionary<Vector3, bool>();

            for (int scanner = 0; scanner < scannedBeacons.Length; scanner++)
            {
                foreach (Vector3 beacon in scannedBeacons[scanner])
                {
                    Vector3 location = absoluteLocations[scanner] + Vector3.Transform(beacon, absoluteOrientations[scanner]);

                    beaconLocations[Round(location)] = true;
                }
            }

            return 0;
        }
    }
}
