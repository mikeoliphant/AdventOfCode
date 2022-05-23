namespace AdventOfCode._2015
{
    internal class Day3
    {
        public long Compute()
        {
            SparseGrid<int> grid = new SparseGrid<int>();

            LongVec2 position = LongVec2.Zero;

            string cmds = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2015\Day3.txt").Trim();

            foreach (char cmd in cmds)
            {
                int val = 0;

                grid.TryGetValue((int)position.X, (int)position.Y, out val);

                grid[(int)position.X, (int)position.Y] = val + 1;

                int dx = 0;
                int dy = 0;

                switch (cmd)
                {
                    case '^':
                        dy = -1;
                        break;

                    case 'v':
                        dy = 1;
                        break;

                    case '>':
                        dx = 1;
                        break;

                    case '<':
                        dx = -1;
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                position = position + new LongVec2(dx, dy);
            }

            return grid.Count;
        }

        public long Compute2()
        {
            SparseGrid<int> grid = new SparseGrid<int>();

            LongVec2[] positions = new LongVec2[2];

            string cmds = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2015\Day3.txt").Trim();

            int santaIndex = 0;

            foreach (char cmd in cmds)
            {
                LongVec2 position = positions[santaIndex];

                int val = 0;

                grid.TryGetValue((int)position.X, (int)position.Y, out val);

                grid[(int)position.X, (int)position.Y] = val + 1;

                int dx = 0;
                int dy = 0;

                switch (cmd)
                {
                    case '^':
                        dy = -1;
                        break;

                    case 'v':
                        dy = 1;
                        break;

                    case '>':
                        dx = 1;
                        break;

                    case '<':
                        dx = -1;
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                positions[santaIndex] = position + new LongVec2(dx, dy);

                santaIndex = 1 - santaIndex;
            }

            return grid.Count;
        }
    }
}
