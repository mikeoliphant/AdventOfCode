namespace AdventOfCode._2016
{
    internal class Day13
    {
        //int designerVal = 10;
        int designerVal = 1358;

        bool IsWall(int x, int y)
        {
            int val = (x * x) + (3 * x) + (2 * x * y) + y + (y * y);
            val += designerVal;

            return (BitUtil.NumberOfSetBits(val) % 2) != 0;
        }

        IEnumerable<KeyValuePair<(int X, int Y), float>> GetNeighbors((int X, int Y) pos)
        {
            foreach (var neighbor in Grid<int>.AllNeighbors(pos.X, pos.Y))
            {
                if ((neighbor.X < 0) || (neighbor.Y < 0))
                    continue;
 
                if (!IsWall(neighbor.X, neighbor.Y))
                {
                    yield return new KeyValuePair<(int X, int Y), float>(neighbor, 1);
                }
            }
        }

        public long Compute()
        {
            DijkstraSearch<(int X, int Y)> search = new DijkstraSearch<(int X, int Y)>(GetNeighbors);

            List<(int X, int Y)> path;
            float cost;

            //if (search.GetShortestPath((1, 1), (7, 4), out path, out cost))
            if (search.GetShortestPath((1, 1), (31, 39), out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }

        public long Compute2()
        {
            DijkstraSearch<(int X, int Y)> search = new DijkstraSearch<(int X, int Y)>(GetNeighbors);

            List<(int X, int Y)> path;
            float cost;

            int numReacheable = 0;

            for (int x = 0; x <= 50; x++)
            {
                for (int y = 0; y <= 50; y++)
                {
                    if (search.GetShortestPath((1, 1), (x, y), out path, out cost))
                    {
                        if (cost <= 50)
                            numReacheable++;
                    }

                }
            }

            return numReacheable;
        }
    }
}
