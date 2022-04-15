namespace AdventOfCode._2018
{
    internal class Day6
    {
        List<Point> points = new List<Point>();

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day6.txt"))
            {
                int[] coords = line.ToInts(',').ToArray();

                points.Add(new Point(coords[0], coords[1]));
            }
        }

        public long Compute()
        {
            ReadInput();

            int minX = points.Min(p => p.X);
            int maxX = points.Max(p => p.X);
            int minY = points.Min(p => p.Y);
            int maxY = points.Max(p => p.Y);

            Dictionary<Point, int> nonInfiniteDist = new Dictionary<Point, int>();

            foreach (Point p in points)
            {
                nonInfiniteDist[p] = 0;
            }

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if ((x == 0) || (x == maxX) || (y == 0) || (y == maxY))
                    {
                        Point p = new Point(x, y);

                        points.Sort((p1, p2) => p1.ManhattanDistance(p).CompareTo(p2.ManhattanDistance(p)));

                        int minDist = points[0].ManhattanDistance(p);

                        int pos = 0;

                        do
                        {

                            nonInfiniteDist.Remove(points[pos]);

                            pos++;
                        }
                        while (points[pos].ManhattanDistance(p) == minDist);
                    }
                }
            }

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point p = new Point(x, y);

                    if (!points.Contains(p))
                    {
                        points.Sort((p1, p2) => p1.ManhattanDistance(p).CompareTo(p2.ManhattanDistance(p)));

                        if (nonInfiniteDist.ContainsKey(points[0]))
                        {
                            if (points[0].ManhattanDistance(p) < points[1].ManhattanDistance(p))
                            {
                                nonInfiniteDist[points[0]]++;
                            }
                        }
                    }
                }
            }

            return nonInfiniteDist.Values.Max() + 1;
        }

        public long Compute2()
        {
            ReadInput();

            int minX = points.Min(p => p.X);
            int maxX = points.Max(p => p.X);
            int minY = points.Min(p => p.Y);
            int maxY = points.Max(p => p.Y);

            int regionSize = 0;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point p = new Point(x, y);

                    int totDist = points.Sum(p2 => p2.ManhattanDistance(p));

                    if (totDist < 10000)
                    {
                        regionSize++;
                    }
                }
            }


            return 0;
        }
    }
}
