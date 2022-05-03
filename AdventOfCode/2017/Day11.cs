namespace AdventOfCode._2017
{
    internal class Day11
    {
        (int X, int Y) Move((int X, int Y) pos, string dir)
        {
            switch (dir)
            {
                case "n":
                    return (pos.X, pos.Y + 1);

                case "s":
                    return (pos.X, pos.Y - 1);

                case "ne":
                    return (pos.X + 1, pos.Y);

                case "nw":
                    return (pos.X - 1, pos.Y + 1);

                case "se":
                    return (pos.X + 1, pos.Y - 1);

                case "sw":
                    return (pos.X - 1, pos.Y);

                default:
                    throw new InvalidOperationException();
            }
        }

        int Distance((int X, int Y) pos1, (int X, int Y) pos2)
        {
            int dx = pos2.X - pos1.X;
            int dy = pos2.Y - pos1.Y;

            return ((Math.Sign(dx) == Math.Sign(dy)) ? Math.Abs(dx + dy) : Math.Max(Math.Abs(dx), Math.Abs(dy)));
        }

        public long Compute()
        {
            //string[] path = "se,sw,se,sw,sw".Split(',');
            string[] path = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day11.txt").Trim().Split(',');

            int maxDist = int.MinValue;

            (int X, int Y) pos = (0, 0);

            foreach (string dir in path)
            {
                pos = Move(pos, dir);

                maxDist = Math.Max(maxDist, Distance(pos, (0, 0)));
            }

            return maxDist;// Distance(pos, (0, 0));
        }
    }
}
