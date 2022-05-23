namespace AdventOfCode._2015
{
    internal class Day6 : Day
    {
        public override long Compute()
        {
            Grid<int> grid = new Grid<int>(1000, 1000);

            foreach (string cmd in File.ReadLines(DataFile))
            {
                var match = Regex.Match(cmd, "(turn on|turn off|toggle) (.*) through (.*)");

                if (match.Success)
                {
                    string onOffToggle = match.Groups[1].Value;

                    int[] pos1 = match.Groups[2].Value.ToInts(',').ToArray();
                    int[] pos2 = match.Groups[3].Value.ToInts(',').ToArray();

                    switch (onOffToggle)
                    {
                        case "turn on":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] = 1;
                            }
                            break;

                        case "turn off":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] = 0;
                            }
                            break;

                        case "toggle":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] = (grid.GetValue(pos) == 1) ? 0 : 1;
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }

                }
                else throw new InvalidOperationException();
            }

            return grid.CountValue(1);
        }

        public override long Compute2()
        {
            Grid<int> grid = new Grid<int>(1000, 1000);

            foreach (string cmd in File.ReadLines(DataFile))
            {
                var match = Regex.Match(cmd, "(turn on|turn off|toggle) (.*) through (.*)");

                if (match.Success)
                {
                    string onOffToggle = match.Groups[1].Value;

                    int[] pos1 = match.Groups[2].Value.ToInts(',').ToArray();
                    int[] pos2 = match.Groups[3].Value.ToInts(',').ToArray();

                    switch (onOffToggle)
                    {
                        case "turn on":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] += 1;
                            }
                            break;

                        case "turn off":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] = Math.Max(grid[pos] - 1, 0);
                            }
                            break;

                        case "toggle":
                            foreach (var pos in Grid.GetRectangleInclusive(new Rectangle(pos1[0], pos1[1], pos2[0] - pos1[0], pos2[1] - pos1[1])))
                            {
                                grid[pos] += 2;
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }

                }
                else throw new InvalidOperationException();
            }

            return grid.GetAllValues().Select(g => (long)g).Sum();
        }
    }
}
