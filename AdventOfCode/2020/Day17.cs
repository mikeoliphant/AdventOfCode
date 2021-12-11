using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day17
    {
        Dictionary<string, bool> grid = new Dictionary<string, bool>();

        string GetKey(int x, int y, int z, int w)
        {
            return x + "|" + y + "|" + z + "|" + w;
        }

        void ParseKey(string key, out int x, out int y, out int z, out int w)
        {
            string[] split = key.Split('|');

            x = int.Parse(split[0]);
            y = int.Parse(split[1]);
            z = int.Parse(split[2]);
            w = int.Parse(split[3]);
        }

        void SetGrid(Dictionary<string, bool> grid, int x, int y, int z, int w, bool value)
        {
            string key = GetKey(x, y, z, w);

            if (value)
            {
                grid[key] = true;
            }
            else
            {
                grid.Remove(key);
            }
        }

        bool GetGrid(Dictionary<string, bool> grid, int x, int y, int z, int w)
        {
            string key = GetKey(x, y, z, w);

            return grid.ContainsKey(key);
        }

        void ReadInput()
        {
            string[] startGrid = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day17.txt").ToArray();

            for (int y = 0; y < startGrid.Length; y++)
            {
                for (int x = 0; x < startGrid[y].Length; x++)
                {
                    if (startGrid[y][x] == '#')
                        SetGrid(grid, x, y, 0, 0, true);
                }
            }
        }

        public void CycleGrid()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            int minZ = int.MaxValue;
            int maxZ = int.MinValue;
            int minW = int.MaxValue;
            int maxW = int.MinValue;

            Dictionary<string, bool> newGrid = new Dictionary<string, bool>();

            foreach (string key in grid.Keys)
            {
                int x;
                int y;
                int z;
                int w;

                ParseKey(key, out x, out y, out z, out w);

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                minZ = Math.Min(minZ, z);
                minW = Math.Min(minW, w);

                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                maxZ = Math.Max(maxZ, z);
                maxW = Math.Max(maxW, w);
            }

            for (int x = minX - 1; x <= maxX + 1; x++)
            {
                for (int y = minY - 1; y <= maxY + 1; y++)
                {
                    for (int z = minZ - 1; z <= maxZ + 1; z++)
                    {
                        for (int w = minW - 1; w <= maxW + 1; w++)
                        {
                            int neighbors = 0;

                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dz = -1; dz <= 1; dz++)
                                    {
                                        for (int dw = -1; dw <= 1; dw++)
                                        {
                                            if ((dx != 0) || (dy != 0) || (dz != 0) || (dw != 0))
                                            {
                                                if (GetGrid(grid, x + dx, y + dy, z + dz, w + dw))
                                                {
                                                    neighbors++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (GetGrid(grid, x, y, z, w))
                            {
                                if ((neighbors == 2) || (neighbors == 3))
                                {
                                    SetGrid(newGrid, x, y, z, w, true);
                                }
                            }
                            else if (neighbors == 3)
                            {
                                SetGrid(newGrid, x, y, z, w, true);
                            }
                        }
                    }
                }
            }

            grid = newGrid;
        }

        public long Compute()
        {
            ReadInput();

            for (int i = 0; i < 6; i++)
                CycleGrid();

            return grid.Values.Count();
        }
    }
}
