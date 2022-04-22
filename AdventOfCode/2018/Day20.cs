namespace AdventOfCode._2018
{
    internal class Day20
    {
        SparseGrid<char> grid = new SparseGrid<char>();
        TreeNode<string> tree = new TreeNode<string>();

        IEnumerable<string> SplitParens(string regex)
        {
            int nesting = 0;
            int lastPos = 0;

            for (int pos = 0; pos < regex.Length; pos++)
            {
                if (regex[pos] == '(')
                {
                    if (nesting == 0)
                    {
                        if (pos > lastPos)
                        {
                            yield return regex.Substring(lastPos, pos - lastPos);

                            lastPos = pos;
                        }
                    }

                    nesting++;
                }
                else if (regex[pos] == ')')
                {
                    nesting--;

                    if (nesting == 0)
                    {
                        yield return regex.Substring(lastPos, pos - lastPos + 1);

                        lastPos = pos + 1;
                    }
                }
            }

            if (lastPos < regex.Length)
            {
                yield return regex.Substring(lastPos);
            }
        }

        IEnumerable<string> SplitOr(string regex)
        {
            int nesting = 0;
            int lastPos = 0;

            for (int pos = 0; pos < regex.Length; pos++)
            {
                if (regex[pos] == '(')
                {
                    nesting++;
                }
                else if (regex[pos] == ')')
                {
                    nesting--;
                }
                else if ((nesting == 0) && (regex[pos] == '|'))
                {
                    yield return regex.Substring(lastPos, pos - lastPos);

                    lastPos = pos + 1;
                }
            }

            if (lastPos < regex.Length)
            {
                yield return regex.Substring(lastPos);
            }
        }

        void ParseTree(TreeNode<string> node, string regex)
        {
            foreach (string s in SplitParens(regex))
            {
                if (s[0] == '(')
                {
                    TreeNode<string> orNode = new TreeNode<string>();

                    foreach (string orStr in SplitOr(s.Substring(1, s.Length - 2)))
                    {
                        TreeNode<string> andNode = new TreeNode<string>();

                        ParseTree(andNode, orStr);

                        orNode.Children.Add(andNode);
                    }

                    node.Children.Add(orNode);
                }
                else
                {
                    node.Children.Add(new TreeNode<string>(s));
                }
            }
        }

        string CreateRegex(TreeNode<string> node, bool isAnd)
        {
            if (node.Children.Count == 0)
                return node.Value;

            if (isAnd)
            {
                return String.Join("", node.Children.Select(n => CreateRegex(n, isAnd: false)));   
            }
            else
            {
                return "(" + String.Join("|", node.Children.Select(n => CreateRegex(n, isAnd: true))) + ")";
            }
        }

        void ReadInput()
        {
            //string regex = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            string regex = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2018\Day20.txt").Trim();

            ParseTree(tree, regex.Substring(1, regex.Length - 2));
        }

        IEnumerable<(int X, int Y)> WalkTree(TreeNode<string> node, bool isAnd, (int X, int Y) gridPos)
        {
            if (node.Children.Count == 0)
            {
                foreach (char c in node.Value)
                {
                    foreach (var neighbor in grid.AllNeighbors(gridPos.X, gridPos.Y, includeDiagonal: true))
                    {
                        char n;

                        if (!grid.TryGetValue(neighbor.X, neighbor.Y, out n))
                        {
                            grid[neighbor.X, neighbor.Y] = '#';
                        }

                        grid[gridPos] = '.';
                    }

                    switch (c)
                    {
                        case 'N':
                            grid[gridPos.X, gridPos.Y - 1] = '-';
                            gridPos = (gridPos.X, gridPos.Y - 2);
                            break;

                        case 'S':
                            grid[gridPos.X, gridPos.Y + 1] = '-';
                            gridPos = (gridPos.X, gridPos.Y + 2);
                            break;

                        case 'W':
                            grid[gridPos.X - 1, gridPos.Y] = '|';
                            gridPos = (gridPos.X - 2, gridPos.Y);
                            break;

                        case 'E':
                            grid[gridPos.X + 1, gridPos.Y] = '|';
                            gridPos = (gridPos.X + 2, gridPos.Y);
                            break;
                    }

                    foreach (var neighbor in grid.AllNeighbors(gridPos.X, gridPos.Y, includeDiagonal: true))
                    {
                        char n;

                        if (!grid.TryGetValue(neighbor.X, neighbor.Y, out n))
                        {
                            grid[neighbor.X, neighbor.Y] = '#';
                        }

                        grid[gridPos] = '.';
                    }
                }

                foreach (var neighbor in grid.AllNeighbors(gridPos.X, gridPos.Y, includeDiagonal: true))
                {
                    char n;

                    if (!grid.TryGetValue(neighbor.X, neighbor.Y, out n))
                    {
                        grid[neighbor.X, neighbor.Y] = '#';
                    }

                    grid[gridPos] = '.';
                }

                yield return gridPos;
            }
            else
            {
                if (isAnd)
                {
                    foreach (var child in node.Children)
                    {
                        foreach (var pos in WalkTree(child, isAnd: false, gridPos))
                        {
                            gridPos = pos;
                        }
                    }
                }
                else
                {
                    foreach (var child in node.Children)
                    {
                        foreach (var pos in WalkTree(child, isAnd: true, gridPos))
                        {
                            yield return pos;
                        }
                    }
                }
            }
        }

        bool IsLoop(string regex)
        {
            int x = 0;
            int y = 0;

            foreach (char c in regex)
            {
                switch (c)
                {
                    case 'N':
                        y--;
                        break;

                    case 'S':
                        y++;
                        break;

                    case 'W':
                        x--;
                        break;

                    case 'E':
                        x++;
                        break;
                }
            }

            return (x == 0) && (y == 0);
        }

        int MaxDoors(TreeNode<string> node, bool isAnd)
        {
            if (node.Children.Count == 0)
            {
                if (IsLoop(node.Value))
                    return 0;

                return node.Value.Length;
            }

            if (isAnd)
            {
                return node.Children.Sum(c => MaxDoors(c, isAnd: false));
            }
            else
            {
                return node.Children.Max(c => MaxDoors(c, isAnd: true));
            }
        }

        IEnumerable<KeyValuePair<(int X, int Y), float>> GetUnblockedNeighborCost((int X, int Y) position)
        {
            foreach (var pos in grid.AllNeighbors(position.X, position.Y))
            {
                if (grid[pos.X, pos.Y] != '#')
                {
                    int dx = pos.X - position.X;
                    int dy = pos.Y - position.Y;

                    yield return new KeyValuePair<(int X, int Y), float>((pos.X + dx, pos.Y + dy), 1);
                }
            }
        }


        public bool GetShortestPath(int startX, int startY, int endX, int endY, out List<(int X, int Y)> path, out float cost)
        {
            DijkstraSearch<ValueTuple<int, int>> search = new DijkstraSearch<ValueTuple<int, int>>(GetUnblockedNeighborCost);

            return search.GetShortestPath((startX, startY), (endX, endY), out path, out cost);
        }

        public long Compute()
        {
            ReadInput();

            //tree.PrintToConsole();

            foreach (var pos in WalkTree(tree, isAnd: true, (0, 0))) ;

            grid[0, 0] = 'X';

            //grid.PrintToConsole();

            return MaxDoors(tree, isAnd: true);
        }

        public long Compute2()
        {
            ReadInput();

            foreach (var pos in WalkTree(tree, isAnd: true, (0, 0))) ;

            grid[0, 0] = 'X';

            List<(int X, int y)> path;
            float cost;

            int numAtLeast1000 = 0;

            foreach (var pos in grid.FindValue('.'))
            {               
                if (GetShortestPath(0, 0, pos.X, pos.Y, out path, out cost))
                {
                    if (cost >= 1000)
                        numAtLeast1000++;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return numAtLeast1000;
        }
    }
}
