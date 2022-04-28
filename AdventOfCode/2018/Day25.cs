namespace AdventOfCode._2018
{
    internal class Day25
    {
        List<Vector4> points = new List<Vector4>();
        Dictionary<int, HashSet<int>> manhattanPairs = new Dictionary<int, HashSet<int>>();

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day25.txt"))
            {
                points.Add(new Vector4(line.ToFloats(',').ToArray()));
            }
        }

        bool CanReach(int pos1, int pos2)
        {
            foreach (int pair in manhattanPairs[pos1])
            {
                if ((pair == pos2) || CanReach(pair, pos2))
                    return true;
            }

            return false;
        }

        void SanityCheck(List<HashSet<int>> constellations)
        {
            foreach (var constellation1 in constellations)
            {
                foreach (var constellation2 in constellations)
                {
                    if (constellation1 != constellation2)
                    {
                        foreach (int pos1 in constellation1)
                        {
                            foreach (int pos2 in constellation2)
                            {
                                if (points[pos1].ManhattanDistance(points[pos2]) <= 3)
                                {
                                    throw new InvalidDataException();
                                }
                            }
                        }
                    }
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            for (int pos1 = 0; pos1 < points.Count; pos1++)
            {
                manhattanPairs[pos1] = new HashSet<int>();

                for (int pos2 = pos1 + 1; pos2 < points.Count; pos2++)
                {
                    if (points[pos1].ManhattanDistance(points[pos2]) <= 3)
                    {
                        manhattanPairs[pos1].Add(pos2);
                    }
                }
            }

            foreach (var pair in manhattanPairs)
            {
                foreach (int pos in pair.Value)
                {
                    manhattanPairs[pos].Add(pair.Key);
                }
            }

            List<int> toConstellate = Enumerable.Range(0, points.Count).ToList();

            List<HashSet<int>> constellations = new List<HashSet<int>>();

            do
            {
                HashSet<int> constellation = new HashSet<int>();
                constellation.Add(toConstellate[0]);
                toConstellate.RemoveAt(0);

                do
                {
                    HashSet<int> toAdd = new HashSet<int>();

                    foreach (int pos in constellation)
                    {
                        foreach (int pos2 in manhattanPairs[pos])
                        {
                            if (!constellation.Contains(pos2))
                            {
                                toAdd.Add(pos2);

                                toConstellate.Remove(pos2);
                            }
                        }
                    }

                    if (toAdd.Count == 0)
                        break;

                    foreach (int pos in toAdd)
                    {
                        constellation.Add(pos);
                    }
                }
                while (true);

                constellations.Add(constellation);
            }
            while (toConstellate.Count > 0);

            SanityCheck(constellations);

            return constellations.Count;
        }
    }
}
