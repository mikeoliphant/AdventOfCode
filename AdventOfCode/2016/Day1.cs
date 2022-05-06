namespace AdventOfCode._2016
{
    internal class Day1
    {
        public long Compute()
        {
            LongVec2 pos = new LongVec2(0, 0);
            int facing = 0;

            //var directions = "R2, R2, R2".Split(", ");

            var directions = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2016\Day1.txt").Trim().Split(", ");

            foreach (string direction in directions)
            {
                char dir = direction[0];
                int amount = int.Parse(direction.Substring(1));

                if (dir == 'R')
                {
                    facing = LongVec2.TurnFacing(facing, 1);
                }
                else
                {
                    facing = LongVec2.TurnFacing(facing, -1);
                }

                pos.AddFacing(facing, amount);
            }

            return pos.ManhattanDistance(LongVec2.Zero);
        }

        public long Compute2()
        {
            Dictionary<LongVec2, bool> visited = new Dictionary<LongVec2, bool>();

            LongVec2 pos = new LongVec2(0, 0);
            int facing = 0;

            //var directions = "R8, R4, R4, R8".Split(", ");
            var directions = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2016\Day1.txt").Trim().Split(", ");

            foreach (string direction in directions)
            {
                char dir = direction[0];
                int amount = int.Parse(direction.Substring(1));

                if (dir == 'R')
                {
                    facing = LongVec2.TurnFacing(facing, 1);
                }
                else
                {
                    facing = LongVec2.TurnFacing(facing, -1);
                }

                for (int i = 0; i < amount; i++)
                {
                    if (visited.ContainsKey(pos))
                    {
                        return pos.ManhattanDistance(LongVec2.Zero);
                    }

                    visited[pos] = true;

                    pos.AddFacing(facing, 1);
                }
            }

            throw new InvalidOperationException();
        }
    }
}
