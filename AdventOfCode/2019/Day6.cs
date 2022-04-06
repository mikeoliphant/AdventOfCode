using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode._2019
{
    internal class Day6
    {
        Dictionary<string, List<string>> orbits = new Dictionary<string, List<string>>();
        Dictionary<string, string> objOrbits = new Dictionary<string, string>();
        Dictionary<string, int> distToCom = new Dictionary<string, int>();

        void ReadInput()
        {
            foreach (string pair in File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day6.txt"))
            {
                string[] lr = pair.Split(')');

                objOrbits[lr[1]] = lr[0];

                AddOrbiter(lr[0], lr[1]);
            }

            ComputeDistToCom(0, "COM");
        }

        void AddOrbiter(string a, string b)
        {
            if (!orbits.ContainsKey(a))
            {
                orbits[a] = new List<string>();
            }

            orbits[a].Add(b);
        }

        void ComputeDistToCom(int distSoFar, string obj)
        {
            distToCom[obj] = distSoFar;

            if (!orbits.ContainsKey(obj))
                return;

            foreach (string orbiter in orbits[obj])
            {
                ComputeDistToCom(distSoFar + 1, orbiter);
            }
        }

        public long Compute()
        {
            ReadInput();

            int numOrbits = distToCom.Values.Sum();

            return numOrbits;
        }

        List<string> GetPath(string obj1, string obj2)
        {
            List<string> path = new List<string>();

            while (obj1 != obj2)
            {
                path.Add(obj1);

                obj1 = objOrbits[obj1];
            }

            return path;
        }

        public long Compute2()
        {
            ReadInput();

            List<string> youPath = GetPath("YOU", "COM");
            List<string> sanPath = GetPath("SAN", "COM");

            string sharedObj = null;

            for (int pos = 0; pos < youPath.Count; pos++)
            {
                if (youPath[youPath.Count - pos - 1] != sanPath[sanPath.Count - pos - 1])
                {
                    sharedObj = youPath[youPath.Count - pos];

                    break;
                }
            }

            int numMoves = (distToCom["YOU"] + distToCom["SAN"] - (distToCom[sharedObj] * 2)) - 2;

            return numMoves;
        }
    }
}
