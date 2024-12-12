using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace AdventOfCode._2024
{
    internal class Day12 : Day
    {
        Grid<char> grid = null;

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
        }

        LinkedList<(int X, int Y)>? FloodFill(GridBase<char> grid, (int X, int Y) pos)
        {
            if (grid[pos] == '#')
                return null;

            LinkedList<(int X, int Y)> cells = new();

            cells.AddLast(pos);

            char cellVal = grid[pos];
            grid[pos] = '#';

            var currentCell = cells.First;

            while (currentCell != null)
            {
                foreach (var neighbor in grid.ValidNeighbors(currentCell.Value.X, currentCell.Value.Y))
                {
                    if (grid[neighbor] == cellVal)
                    {
                        grid[neighbor] = '#';

                        cells.AddAfter(currentCell, neighbor);
                    }
                }

                currentCell = currentCell.Next;
            }

            return cells;
        }

        int FenceCount((int X, int Y) pos)
        {
            char cellValue = grid[pos];

            return Grid.AllNeighbors(pos.X, pos.Y).Where(g => !grid.IsValid(g) || (grid[g] != cellValue)).Count();
        }

        public override long Compute()
        {
            ReadData();

            grid.PrintToConsole();

            List<LinkedList<(int X, int Y)>> regions = new();

            foreach (var pos in grid.GetAll())
            {
                var cells = FloodFill(grid, pos);

                if (cells != null)
                    regions.Add(cells);
            }

            ReadData();

            long cost = 0;

            foreach (var region in regions)
            {
                cost += (region.Count * region.Sum(g => FenceCount(g)));
            }

            return cost;
        }

        IEnumerable<((int X, int Y) Pos, bool IsVert, bool IsPos)> GetFences((int X, int Y) pos)
        {
            char cellValue = grid[pos.X, pos.Y];

            foreach (var neighbor in Grid.AllNeighbors(pos.X, pos.Y).Where(g => !grid.IsValid(g) || (grid[g] != cellValue)))
            {
                yield return (pos, pos.Y == neighbor.Y, ((neighbor.X - pos.X) > 0) || ((neighbor.Y - pos.Y) > 0));
            }
        }

        int CountSides(List<((int X, int Y) Pos, bool IsVert, bool IsPos)> fences)
        {
            List<((int X, int Y) Pos, bool IsVert, bool IsPos)> toRemove = new();

            int fenceCount = 0;

            while (fences.Count > 0)
            {
                fenceCount++;

                var fence = fences[0];

                toRemove.Add(fence);

                if (fence.IsVert)
                {
                    int y = fence.Pos.Y;

                    do
                    {
                        y++;

                        if (fences.Contains(((fence.Pos.X, y), true, fence.IsPos)))
                        {
                            toRemove.Add(((fence.Pos.X, y), true, fence.IsPos));
                        }
                        else
                            break;
                    }
                    while (true);

                    y = fence.Pos.Y;

                    do
                    {
                        y--;

                        if (fences.Contains(((fence.Pos.X, y), true, fence.IsPos)))
                        {
                            toRemove.Add(((fence.Pos.X, y), true, fence.IsPos));
                        }
                        else
                            break;
                    }
                    while (true);
                }
                else
                {
                    int x = fence.Pos.X;

                    do
                    {
                        x++;

                        if (fences.Contains(((x, fence.Pos.Y), false, fence.IsPos)))
                        {
                            toRemove.Add(((x, fence.Pos.Y), false, fence.IsPos));
                        }
                        else
                            break;
                    }
                    while (true);

                    x = fence.Pos.X;

                    do
                    {
                        x--;

                        if (fences.Contains(((x, fence.Pos.Y), false, fence.IsPos)))
                        {
                            toRemove.Add(((x, fence.Pos.Y), false, fence.IsPos));
                        }
                        else
                            break;
                    }
                    while (true);
                }

                Console.WriteLine(string.Join(',', toRemove));

                foreach (var f in toRemove)
                {
                    fences.Remove(f);
                }

                toRemove.Clear();
            }

            Console.WriteLine();

            return fenceCount;
        }

        public override long Compute2()
        {
            ReadData();

            List<LinkedList<(int X, int Y)>> regions = new();

            foreach (var pos in grid.GetAll())
            {
                var cells = FloodFill(grid, pos);

                if (cells != null)
                    regions.Add(cells);
            }

            ReadData();

            long cost = 0;

            foreach (var region in regions)
            {
                var fences = region.SelectMany(cell => GetFences(cell)).ToList();

                int sides = CountSides(fences);

                cost += region.Count * sides;
            }

            return cost;
        }
    }
}
