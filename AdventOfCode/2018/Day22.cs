namespace AdventOfCode._2018
{
    internal class Day22
    {
        SparseGrid<int> grid = new SparseGrid<int>();
        //int depth = 510;// 10914;
        int depth = 10914;
        //(int X, int Y) target = (10, 10);// (9, 739);
        (int X, int Y) target = (9, 739);

        int ErosionLevel(int x, int y)
        {
            return (grid[x, y] + depth) % 20183;
        }

        int GeoIndex(int x, int y)
        {
            if ((x == target.X) && (y == target.Y))
                return 0;

            if (y == 0)
                return x * 16807;

            if (x == 0)
                return y * 48271;

            return ErosionLevel(x - 1, y) * ErosionLevel(x, y - 1);
        }

        public void PrintToConsole()
        {
            int minX;
            int maxX;
            int minY;
            int maxY;

            grid.GetBounds(out minX, out minY, out maxX, out maxY);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    int type = ErosionLevel(x, y) % 3;

                    switch (type)
                    {
                        case 0:
                            Console.Write('.');
                            break;
                        case 1:
                            Console.Write('=');
                            break;
                        case 2:
                            Console.Write('|');
                            break;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public long Compute()
        {
            for (int y = 0; y <= target.Y; y++)
            {
                for (int x = 0; x <= target.X; x++)
                {
                    grid[x, y] = GeoIndex(x, y);
                }
            }

            grid[target] = 0;

            //PrintToConsole();

            int risk = grid.GetAllValues().Sum(g => ((g + depth) % 20183) % 3);

            return risk;
        }

        IEnumerable<KeyValuePair<(int X, int Y, int Tool), float>> GetNeighborCost((int X, int Y, int Tool) state)
        {
            foreach (var pos in grid.ValidNeighbors(state.X, state.Y))
            {
                int type = ErosionLevel(pos.X, pos.Y) % 3;

                switch (type)
                {
                    case 0:
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 1), 1 + ((state.Tool == 1) ? 0 : 7));     // Climbing
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 2), 1 + ((state.Tool == 2) ? 0 : 7));     // Torch
                        break;
                    case 1:
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 0), 1 + ((state.Tool == 0) ? 0 : 7));     // Neither
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 1), 1 + ((state.Tool == 1) ? 0 : 7));     // Climbing
                        break;
                    case 2:
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 0), 1 + ((state.Tool == 0) ? 0 : 7));     // Neither
                        yield return new KeyValuePair<(int X, int Y, int Tool), float>((pos.X, pos.Y, 2), 1 + ((state.Tool == 2) ? 0 : 7));     // Torch
                        break;
                }
            }
        }

        public long Compute2()
        {
            for (int y = 0; y <= target.Y + 100; y++)
            {
                for (int x = 0; x <= target.X + 100; x++)
                {
                    grid[x, y] = GeoIndex(x, y);
                }
            }

            //PrintToConsole();

            DijkstraSearch<ValueTuple<int, int, int>> search = new DijkstraSearch<ValueTuple<int, int, int>>(GetNeighborCost);

            List<(int X, int Y, int Tool)> path;
            float cost;

            if (search.GetShortestPath((0, 0, 2), (target.X, target.Y, 2), out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }

    }
}
