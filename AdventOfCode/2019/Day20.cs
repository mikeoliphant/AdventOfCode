using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day20
    {
        Grid<char> grid = null;
        Dictionary<(int, int), (int X, int Y)> portals = new Dictionary<(int, int), (int X, int Y)>();
        Dictionary<(char, char), (int X, int Y)> nameLookup = new Dictionary<(char, char), (int X, int Y)>();
        (int X, int Y) startPosition, endPosition;

        bool AddPortal((char, char) name, (int, int) position)
        {
            if (name.Item1 == name.Item2)
            {
                if (name.Item1 == 'A')
                {
                    startPosition = position;

                    return true;
                }
                else if (name.Item1 == 'Z')
                {
                    endPosition = position;

                    return true;
                }
            }

            if (nameLookup.ContainsKey(name))
            {
                portals[position] = nameLookup[name];
                portals[nameLookup[name]] = position;
            }
            else
            {
                nameLookup[name] = position;
            }

            return false;
        }

        void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day20.txt"));

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (char.IsUpper(grid[x, y]))
                    {
                        bool isStartFinish = false;

                        if (char.IsUpper(grid[x, y + 1]))
                        {
                            if (grid.GetValue(x, y + 2) == '.')  // Top-down portal
                            {
                                isStartFinish = AddPortal((grid[x, y], grid[x, y + 1]), (x, y + 1));
                            }
                            else  // Bottom-up portal
                            {
                                isStartFinish = AddPortal((grid[x, y], grid[x, y + 1]), (x, y));
                            }

                            char c = '.';

                            if (!isStartFinish)
                            {
                                if ((y == 0) || (y == (grid.Height - 2)))
                                    c = '^';
                                else
                                    c = 'v';
                            }

                            grid[x, y] = c;
                            grid[x, y + 1] = c;
                        }
                        else
                        {
                            if (grid.GetValue(x + 2, y) == '.')  // Left-right portal
                            {
                                isStartFinish = AddPortal((grid[x, y], grid[x + 1, y]), (x + 1, y));
                            }
                            else  // Right-left portal
                            {
                                isStartFinish = AddPortal((grid[x, y], grid[x + 1, y]), (x, y));
                            }

                            char c = '.';

                            if (!isStartFinish)
                            {
                                if ((x == 0) || (x == (grid.Width - 2)))
                                    c = '<';
                                else
                                    c = '>';

                                grid[x, y] = c;
                                grid[x + 1, y] = c;
                            }
                        }
                    }
                }
            }
        }

        IEnumerable<KeyValuePair<(int, int), float>> GetNeighbors((int X, int Y) pos)
        {
            foreach ((int X, int Y) neighbor in grid.ValidNeighbors(pos.X, pos.Y, includeDiagonal: false))
            {
                char c = grid[neighbor.X, neighbor.Y];

                if (c == '.')
                    yield return new KeyValuePair<(int, int), float>((neighbor.X, neighbor.Y), 1);
                else if ("^v<>".Contains(c))
                {
                    if (portals.ContainsKey((neighbor.X, neighbor.Y)))
                    {
                        yield return new KeyValuePair<(int, int), float>(portals[(neighbor.X, neighbor.Y)], 0); // Portals cost zero
                    }
                    else
                    {
                        yield return new KeyValuePair<(int, int), float>((neighbor.X, neighbor.Y), 1);
                    }
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            grid.PrintToConsole();

            DijkstraSearch<(int, int)> search = new DijkstraSearch<(int, int)>(GetNeighbors);

            List<(int X, int Y)> path;
            float cost;

            if (search.GetShortestPath(startPosition, endPosition, out path, out cost))
            {
                foreach (var pos in path)
                {
                    grid[pos.X, pos.Y] = 'O';
                }

                grid.PrintToConsole();

                return (int)cost - 2;   // Subtract 2 because we start and end one square outside the maze
            }

            throw new InvalidOperationException();
        }

        IEnumerable<KeyValuePair<(int, int, int), float>> GetNeighbors2((int X, int Y, int Level) pos)
        {
            foreach ((int X, int Y) neighbor in grid.ValidNeighbors(pos.X, pos.Y, includeDiagonal: false))
            {
                char c = grid[neighbor.X, neighbor.Y];

                if (c == '.')
                    yield return new KeyValuePair<(int, int, int), float>((neighbor.X, neighbor.Y, pos.Level), 1);
                else if ((pos.Level > 0) && "^<".Contains(c))
                {
                    if (portals.ContainsKey((neighbor.X, neighbor.Y)))
                    {
                        yield return new KeyValuePair<(int, int, int), float>((portals[(neighbor.X, neighbor.Y)].X, portals[(neighbor.X, neighbor.Y)].Y, pos.Level - 1), 0); // Portals cost zero
                    }
                    else
                    {
                        yield return new KeyValuePair<(int, int, int), float>((neighbor.X, neighbor.Y, pos.Level), 1);
                    }
                }
                else if ("v>".Contains(c))
                {
                    if (portals.ContainsKey((neighbor.X, neighbor.Y)))
                    {
                        yield return new KeyValuePair<(int, int, int), float>((portals[(neighbor.X, neighbor.Y)].X, portals[(neighbor.X, neighbor.Y)].Y, pos.Level + 1), 0); // Portals cost zero
                    }
                    else
                    {
                        yield return new KeyValuePair<(int, int, int), float>((neighbor.X, neighbor.Y, pos.Level), 1);
                    }
                }
            }
        }

        public long Compute2()
        {
            ReadInput();

            grid.PrintToConsole();

            DijkstraSearch<(int, int, int)> search = new DijkstraSearch<(int, int, int)>(GetNeighbors2);

            List<(int X, int Y, int Level)> path;
            float cost;

            if (search.GetShortestPath((startPosition.X, startPosition.Y, 0), (endPosition.X, endPosition.Y, 0), out path, out cost))
            {
                return (int)cost - 2;   // Subtract 2 because we start and end one square outside the maze
            }

            throw new InvalidOperationException();
        }
    }
}
