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

        Dictionary<(int X, int Y), List<((int X, int Y) Pos, int Dist)>> junctions = new();

        ((int X, int Y) Pos, int Dist) FindEndpoint((int X, int Y) pos, (int X, int Y) lastPos)
        {
            if (junctions.ContainsKey(pos))
                return (pos, 1);

            var neighbors = grid.ValidNeighbors(pos.X, pos.Y).Where(g => (g != lastPos) && grid[g] != '#');

            if (neighbors.Count() != 1)
                throw new InvalidOperationException("Should only be one path");

            var endpoint = FindEndpoint(neighbors.First(), pos);

            return (endpoint.Pos, endpoint.Dist + 1);
        }

        void FindAllJunctions()
        {
            // Start
            junctions[(1, 0)] = new();

            // End
            junctions[(grid.Width - 2, grid.Height - 1)] = new();

            foreach (var pos in grid.GetAll())
            {
                if ((grid[pos] == '.') && (grid.ValidNeighborValues(pos.X, pos.Y).Count('.') == 0))
                {
                    junctions[pos] = new();
                }
            }

            foreach (var junction in junctions.Keys)
            {
                foreach (var neighbor in grid.ValidNeighbors(junction.X, junction.Y))
                {
                    if (grid[neighbor] != '#')
                    {
                        junctions[junction].Add(FindEndpoint(neighbor, junction));
                    }
                }
            }
        }

        Dictionary<(int X, int Y), List<(int X, int Y)>> singlePaths = new();
        Queue<(List<(int X, int Y)> Visited, (int X, int Y) StartPos)> searchQueue = new();

        long GetPathLength(List<(int X, int Y)> path)
        {
            long dist = 0;

            (int X, int Y)? lastPos = null;

            foreach (var pos in path)
            {
                if (lastPos != null)
                {
                    dist += junctions[lastPos.Value].Where(j => j.Pos == pos).First().Dist;
                }

                lastPos = pos;
            }

            return dist;
        }

        List<(int X, int Y)> GetLongestPath2((int X, int Y) startPos)
        {
            searchQueue.Enqueue((new List<(int X, int Y)>(), startPos));

            List<(int X, int Y)> maxPath = null;
            long maxPathLength = 0;

            while (searchQueue.Count > 0)
            {
                var search = searchQueue.Dequeue();

                if (!junctions.ContainsKey(search.StartPos))
                {
                    throw new InvalidOperationException("Not a junction!");
                }

                if ((search.StartPos.Y == (grid.Height - 1)) && (search.StartPos.X == (grid.Width - 2)))
                {
                    search.Visited.Add(search.StartPos);

                    long length = GetPathLength(search.Visited);

                    if (length > maxPathLength)
                    {
                        maxPathLength = length;
                        maxPath = search.Visited;
                    }

                    continue;
                }

                foreach (var otherJunction in junctions[search.StartPos])
                {
                    if (search.Visited.Contains(otherJunction.Pos)) continue;

                    var newVisited = new List<(int X, int Y)>(search.Visited);
                    newVisited.Add(search.StartPos);

                    searchQueue.Enqueue((newVisited, otherJunction.Pos));
                }
            }

            return maxPath;
        }

        public override long Compute2()
        {
            ReadData();

            FindAllJunctions();

            //foreach (var pos in junctions.Keys)
            //{
            //    grid[pos] = '+';
            //}

            //grid.PrintToConsole();

            var path = GetLongestPath2((1, 0));

            long dist = GetPathLength(path);

            return dist;
        }
    }
}
