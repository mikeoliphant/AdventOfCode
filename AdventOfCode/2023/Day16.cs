namespace AdventOfCode._2023
{
    internal class Day16 : Day
    {
        Grid<char> grid = new();
        Grid<char> light = new();
        Dictionary<(int X, int Y, int DX, int DY), bool> visited = new();

        void PropogateLight((int X, int Y) cell, int dx, int dy)
        {
            if (!grid.IsValid(cell))
                return;

            if (visited.ContainsKey((cell.X, cell.Y, dx, dy)))
                return;

            visited[(cell.X, cell.Y, dx, dy)] = true;

            light[cell] = '#';

            switch (grid[cell])
            {
                case '.':
                    PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    break;
                case '|':
                    if (dx == 0)
                    {
                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    else
                    {
                        PropogateLight((cell.X, cell.Y - 1), 0, -1);
                        PropogateLight((cell.X, cell.Y + 1), 0, 1);
                    }
                    break;
                case '-':
                    if (dy == 0)
                    {
                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    else
                    {
                        PropogateLight((cell.X - 1, cell.Y), -1, 0);
                        PropogateLight((cell.X + 1, cell.Y), 1, 0);
                    }
                    break;
                case '/':
                    if (dx == 0)
                    {
                        dx = -dy;
                        dy = 0;

                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    else
                    {
                        dy = -dx;
                        dx = 0;

                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    break;
                    case '\\':
                    if (dx == 0)
                    {
                        dx = dy;
                        dy = 0;

                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    else
                    {
                        dy = dx;
                        dx = 0;

                        PropogateLight((cell.X + dx, cell.Y + dy), dx, dy);
                    }
                    break;
            }
        }

        public override long Compute()
        {
            grid.CreateDataFromRows(File.ReadAllLines(DataFile));
            light = grid.CloneEmpty();
            light.Fill('.');

            PropogateLight((0, 0), 1, 0);

            //light.PrintToConsole();

            int count = light.FindValue('#').Count();

            return count;
        }

        public override long Compute2()
        {
            grid.CreateDataFromRows(File.ReadAllLines(DataFile));
            light = grid.CloneEmpty();
            light.DefaultValue = '.';

            long maxCount = long.MinValue;

            for (int x = 0; x < grid.Width; x++)
            {
                // Down
                PropogateLight((x, 0), 0, 1);

                maxCount = Math.Max(maxCount, light.FindValue('#').Count());
                visited.Clear();
                light.Clear();

                // Up
                PropogateLight((x, grid.Height - 1), 0, -1);

                maxCount = Math.Max(maxCount, light.FindValue('#').Count());
                visited.Clear();
                light.Clear();
            }

            for (int y = 0; y < grid.Height; y++)
            {
                // Right
                PropogateLight((0, y), 1, 0);

                maxCount = Math.Max(maxCount, light.FindValue('#').Count());
                visited.Clear();
                light.Clear();

                // Left
                PropogateLight((grid.Width - 1, y), -1, 0);

                maxCount = Math.Max(maxCount, light.FindValue('#').Count());
                visited.Clear();
                light.Clear();
            }

            //light.PrintToConsole();

            return maxCount;
        }
    }
}
