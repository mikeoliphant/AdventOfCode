using OpenTK.Graphics.OpenGL;

namespace AdventOfCode._2023
{
    internal class Day11 : Day
    {
        long expandFactor = 1000000;

        public override long Compute()
        {
            Grid<char> grid = new();
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            long[] xOffsets = new long[grid.Width];
            long[] yOffsets = new long[grid.Height];

            long xOffset = 0;

            for (int x = 0; x < grid.Width; x++)
            {
                xOffsets[x] = xOffset;

                bool haveGalaxy = false;

                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[x, y] == '#')
                    {
                        haveGalaxy = true;
                        break;
                    }
                }

                if (!haveGalaxy)
                {
                    xOffset += expandFactor;
                }
                else
                {
                    xOffset++;
                }
            }

            long yOffset = 0;

            for (int y = 0; y < grid.Height; y++)
            {
                yOffsets[y] = yOffset;

                bool haveGalaxy = false;

                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == '#')
                    {
                        haveGalaxy = true;
                        break;
                    }
                }

                if (!haveGalaxy)
                {
                    yOffset += expandFactor;
                }
                else
                {
                    yOffset++;
                }
            }

            List<LongVec2> galaxies = new();

            foreach (var galaxy in grid.FindValue('#'))
            {
                galaxies.Add(new LongVec2(xOffsets[galaxy.X], yOffsets[galaxy.Y]));
            }

            List<long> dists = new();

            for (int g1 = 0; g1 < galaxies.Count; g1++)
            {
                for (int g2 = 0; g2 < g1; g2++)
                {
                    dists.Add(galaxies[g1].ManhattanDistance(galaxies[g2]));
                }
            }

            long sum = dists.Sum();

            return sum;
        }
    }
}
