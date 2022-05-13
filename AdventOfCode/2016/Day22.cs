namespace AdventOfCode._2016
{
    internal class Day22
    {
        SparseGrid<(int Size, int Used)> nodes = new SparseGrid<(int Size, int Used)> ();

        void ReadInput()
        {
            foreach (string nodeStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day22.txt").Skip(2))
            {
                string[] split = nodeStr.SplitWhitespace();

                var match = Regex.Match(split[0], @"/dev/grid/node-x(.*)-y(.*)");

                nodes[int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)] = (int.Parse(split[1].Substring(0, split[1].Length - 1)), int.Parse(split[2].Substring(0, split[2].Length - 1)));
            }

        }

        public long Compute()
        {
            ReadInput();

            int numViablePairs = nodes.GetAll().Where(n => nodes[n].Used > 0).Sum(n => nodes.GetAll().Where(n2 => (n2 != n) && ((nodes[n].Used + nodes[n2].Used) <= nodes[n2].Size)).Count());

            return numViablePairs;
        }


        Grid<char> interchangeable = null;

        IEnumerable<KeyValuePair<((int X, int Y) Pos, (int X, int Y) GoalPos), float>> GetNeighbors(((int X, int Y) Pos, (int X, int Y) GoalPos) state)
        {
            foreach (var neighbor in interchangeable.ValidNeighbors(state.Pos.X, state.Pos.Y).Where(n => interchangeable[n] == '.'))
            {
                yield return new KeyValuePair<((int X, int Y) Pos, (int X, int Y) GoalPos), float>((neighbor, (neighbor == state.GoalPos) ? state.Pos : state.GoalPos), 1);
            }
        }


        public long Compute2()
        {
            ReadInput();

            var nonSparse = nodes.ToGrid();

            interchangeable = new Grid<char>(nonSparse.Width, nonSparse.Height);

            foreach (var pos in nodes.GetAll())
            {
                int size = nodes[pos].Size;

                if (size < 100)
                {
                    interchangeable[pos] = '.';
                }
                else if (size > 500)
                {
                    interchangeable[pos] = '#';
                }
            }

            var startNode = nodes.GetAll().Where(
                    n => nodes.ValidNeighbors(n.X, n.Y).Where(n2 => (n2 != n) && (nodes[n2].Used > 0) && ((nodes[n].Used + nodes[n2].Used) <= nodes[n].Size)).Any()
                    ).First();


            var goalPos = (nonSparse.Width - 1, 0);

            DijkstraSearch<((int X, int Y) Pos, (int X, int Y) GoalPos)> search = new DijkstraSearch<((int X, int Y), (int X, int Y))>(GetNeighbors);

            List<((int X, int Y) Pos, (int X, int Y) GoalPos)> path;
            float cost;

            if (search.GetShortestPath((startNode, goalPos), delegate (((int X, int Y) Pos, (int X, int Y) GoalPos) state)
                {
                    return state.GoalPos == (0, 0);
                }, out path, out cost))
            {
                return (int)cost;
            }

            throw new InvalidOperationException();
        }
    }
}
