namespace AdventOfCode._2023
{
    internal class Day23 : Day
    {
        Grid<char> grid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        List<(int X, int Y)> GetLongestPath(HashSet<(int X, int Y)> visited, (int X, int Y) startPos)
        {
            if ((startPos.Y == (grid.Height - 1)) && (startPos.X == (grid.Width - 2)))
            {
                return new List<(int X, int Y)> { startPos };
            }

            List<(int X, int Y)> maxPath = null;
            int maxPathLength = 0;

            foreach (var neighbor in grid.ValidNeighbors(startPos.X, startPos.Y))
            {
                if (visited.Contains(neighbor)) continue;

                switch (grid[neighbor])
                {
                    case '#':
                        continue;
                    case '.':
                        break;
                    case '^':
                        if (neighbor.Y >= startPos.Y)
                            continue;
                        break;
                    case 'v':
                        if (neighbor.Y <= startPos.Y)
                            continue;
                        break;
                    case '>':
                        if (neighbor.X <= startPos.X)
                            continue;
                        break;
                    case '<':
                        if (neighbor.X >= startPos.X)
                            continue;
                        break;
                }

                var newVisited = new HashSet<(int X, int Y)>(visited);
                newVisited.Add(startPos);

                var path = GetLongestPath(newVisited, neighbor);

                if ((path != null) && (path.Count > maxPathLength))
                {
                    maxPathLength = path.Count;
                    maxPath = path;
                }
            }

            if (maxPath == null)
                return null;

            maxPath.Add(startPos);

            return maxPath;
        }

        public override long Compute()
        {
            ReadData();

            //grid.PrintToConsole();

            var path = GetLongestPath(new HashSet<(int X, int Y)>(), (1, 0));

            return path.Count - 1;
        }

        Dictionary<(int X, int Y), List<(int X, int Y)>> singlePaths = new();
        Queue<(List<(int X, int Y)> Visited, (int X, int Y) StartPos)> searchQueue = new();

        List<(int X, int Y)> GetLongestPath2((int X, int Y) startPos)
        {
            searchQueue.Enqueue((new List<(int X, int Y)>(), startPos));

            List<(int X, int Y)> maxPath = null;
            int maxPathLength = 0;

            while (searchQueue.Count > 0)
            {
                var search = searchQueue.Dequeue();

                if ((search.StartPos.Y == (grid.Height - 1)) && (search.StartPos.X == (grid.Width - 2)))
                {
                    if (search.Visited.Count > maxPathLength)
                    {
                        maxPathLength = search.Visited.Count;
                        maxPath = search.Visited;
                    }

                    continue;
                }

                foreach (var neighbor in grid.ValidNeighbors(search.StartPos.X, search.StartPos.Y))
                {
                    if (search.Visited.Contains(neighbor)) continue;

                    switch (grid[neighbor])
                    {
                        case '#':
                            continue;
                    }

                    var newVisited = new List<(int X, int Y)>(search.Visited);
                    newVisited.Add(search.StartPos);

                    searchQueue.Enqueue((newVisited, neighbor));
                }
            }

            return maxPath;
        }

        public override long Compute2()
        {
            ReadData();

            //grid.PrintToConsole();

            var path = GetLongestPath2((1, 0));

            return path.Count - 1;
        }
    }
}
