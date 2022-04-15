namespace AdventOfCode._2019
{
    internal class Day18
    {
        Grid<char> grid;
        int allKeys;

        void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day18.txt"));

            for (int i = 0; i < 26; i++)
            {
                if (!grid.Find((char)('a' + i)).Any())
                    break;

                allKeys |= 1 << i;
            }
        }

        IEnumerable<KeyValuePair<(int, int, int), float>> GetNeighbors((int X, int Y, int Keys) state)
        {
            foreach ((int X, int Y) neighbor in grid.ValidNeighbors(state.X, state.Y, includeDiagonal: false))
            {
                char c = grid[neighbor.X, neighbor.Y];

                if ((c == '.') || (c == '@'))
                {
                    yield return new((neighbor.X, neighbor.Y, state.Keys), 1);
                }
                else if ((char.IsLower(c)))
                {
                    yield return new((neighbor.X, neighbor.Y, state.Keys | (1 << (c - 'a'))), 1);
                }
                else if ((char.IsUpper(c)) && ((state.Keys & (1 << (c - 'A'))) != 0))
                {
                    yield return new((neighbor.X, neighbor.Y, state.Keys), 1);
                }
            }
        }

        bool EndCheck((int X, int Y, int Keys) state)
        {
            return state.Keys == allKeys;
        }

        public long Compute()
        {
            ReadInput();

            grid.PrintToConsole();

            (int X, int Y) startPos = grid.Find('@').First();

            DijkstraSearch<(int, int, int)> search = new DijkstraSearch<(int, int, int)>(GetNeighbors);

            List<(int X, int Y, int Keys)> path;
            float cost;

            if (search.GetShortestPath((startPos.X, startPos.Y, 0), EndCheck, out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }

        (int X, int Y)[] robotPositions;

        bool RunSearch(int robot, int startX, int startY, int haveKeys, int desiredKeys, out int foundKeys, out List<(int X, int Y, int Keys)> path, out float cost)
        {
            int maxKeys = 0;

            DijkstraSearch<(int, int, int)> search = new DijkstraSearch<(int, int, int)>(GetNeighbors);

            bool success = search.GetShortestPath((robotPositions[robot].X, robotPositions[robot].Y, haveKeys),
                delegate ((int X, int Y, int Keys) state)
                {
                    maxKeys |= state.Keys;

                    return state.Keys == desiredKeys;
                }
                , out path, out cost);

            foundKeys = maxKeys;

            return success;
        }

        string KeyString(int keys)
        {
            string keyStr = "";

            for (int i = 0; i < 26; i++)
            {
                if ((keys & (1 << i)) != 0)
                {
                    keyStr += (char)('a' + i);
                }
            }

            return keyStr;
        }

        public long Compute2()
        {
            ReadInput();

            (int X, int Y) startPos = grid.Find('@').First();

            grid[startPos.X, startPos.Y] = '#';

            foreach ((int X, int Y) neighbor in grid.ValidNeighbors(startPos.X, startPos.Y, includeDiagonal: false))
            {
                grid[neighbor.X, neighbor.Y] = '#';
            }

            grid[startPos.X - 1, startPos.Y - 1] = '@';
            grid[startPos.X - 1, startPos.Y + 1] = '@';
            grid[startPos.X + 1, startPos.Y - 1] = '@';
            grid[startPos.X + 1, startPos.Y + 1] = '@';

            robotPositions = grid.Find('@').ToArray();

            grid.PrintToConsole();

            float totCost = 0;
            int haveKeys = 0;

            do
            {
                for (int robot = 0; robot < robotPositions.Length; robot++)
                {
                    float cost;
                    int foundKeys;

                    List<(int X, int Y, int Keys)> path;

                    // Find as many keys as we can with one robot
                    RunSearch(robot, robotPositions[0].X, robotPositions[0].Y, haveKeys, allKeys, out foundKeys, out path, out cost);

                    if ((haveKeys | foundKeys) == haveKeys) // We didn't find any more keys
                        continue;

                    // Go get the new keys
                    if (RunSearch(robot, robotPositions[0].X, robotPositions[0].Y, haveKeys, foundKeys, out foundKeys, out path, out cost))
                    {
                        haveKeys |= foundKeys;

                        var last = path.Last();

                        robotPositions[robot] = (last.X, last.Y);

                        totCost += cost;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            while (haveKeys != allKeys);

            return (long)totCost;
        }
    }
}
