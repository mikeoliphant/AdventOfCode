using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class Day15
    {
        Grid<int> grid;
        Grid<int> cost;

        void ReadInput()
        {
            grid = new Grid<int>().CreateDataFromRows(ParseHelpers.ReadZeroToNineGrid(File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day15.txt")));

            cost = new Grid<int>(grid.Width, grid.Height);
        }

        IEnumerable<KeyValuePair<string, float>> GetNeighbors(string node)
        {
            string[] xy = node.Split('-');

            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);

            if (x < (grid.Width - 1))
                yield return GetNeighbor(x + 1, y);

            if (y < (grid.Height - 1))
                yield return GetNeighbor(x, y + 1);

            if (x > 0)
                yield return GetNeighbor(x - 1, y);

            if (y > 0)
                yield return GetNeighbor(x, y - 1);
        }

        KeyValuePair<string, float> GetNeighbor(int nx, int ny)
        {
            return new KeyValuePair<string, float>(nx + "-" + ny, grid[nx, ny]);
        }

        public long Compute()
        {
            ReadInput();

            //grid.PrintToConsole();

            DijkstraSearch<string> pathSearch = new DijkstraSearch<string>(GetNeighbors);

            List<string> path;
            float cost;

            pathSearch.GetShortestPath("0-0", (grid.Width - 1) + "-" + (grid.Height - 1), out path, out cost);

            return (long)cost;
        }

        public long Compute2()
        {
            ReadInput();

            Grid<int> bigGrid = new Grid<int>(grid.Width * 5, grid.Height * 5);

            for (int xdup = 0; xdup < 5; xdup++)
            {
                for (int ydup = 0; ydup < 5; ydup++)
                {
                    for (int x = 0; x < grid.Width; x++)
                    {
                        for (int y = 0; y < grid.Height; y++)
                        {
                            int val = (grid[x, y] + xdup + ydup);

                            if (val > 9)
                                val -= 9;

                            bigGrid[(xdup * grid.Width) + x, (ydup * grid.Height) + y] = val;
                        }
                    }
                }
            }

            grid = bigGrid;

            //grid.PrintToConsole();

            DijkstraSearch<string> pathSearch = new DijkstraSearch<string>(GetNeighbors);

            List<string> path;
            float cost;

            pathSearch.GetShortestPath("0-0", (grid.Width - 1) + "-" + (grid.Height - 1), out path, out cost);

            return (long)cost;
        }
    }
}
