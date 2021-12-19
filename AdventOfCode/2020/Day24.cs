using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal class HexTile
    {
        public int GridX { get; set; }
        public int GridY { get; set; }

        public bool Flipped { get; set;  }

        public HexTile E { get; set; }
        public HexTile SE { get; set; }
        public HexTile SW { get; set; }
        public HexTile W { get; set; }
        public HexTile NW { get; set; }
        public HexTile NE { get; set; }

        public IEnumerable<KeyValuePair<int, int>> Neighbors()
        {
            yield return new KeyValuePair<int, int>(GridX + 1, GridY);  // e
            yield return new KeyValuePair<int, int>(GridX, GridY + 1);  // se
            yield return new KeyValuePair<int, int>(GridX - 1, GridY + 1);  // sw
            yield return new KeyValuePair<int, int>(GridX - 1, GridY);  // w
            yield return new KeyValuePair<int, int>(GridX, GridY - 1);  // nw
            yield return new KeyValuePair<int, int>(GridX + 1, GridY - 1);  //ne
        }

        public override string ToString()
        {
            return GridX + "," + GridY;
        }
    }

    internal class Day24
    {
        string[] flipCommands;
        Dictionary<string, HexTile> tiles = new Dictionary<string, HexTile>();

        void ReadInput()
        {
            flipCommands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day24.txt").ToArray();
        }

        string[] directions = new string[] { "se", "sw", "ne", "nw", "e", "w" };

        List<string> SplitCommands(string command)
        {
            List<string> split = new List<string>();

            for (int i = 0; i < command.Length;)
            {
                string toMatch = (i == (command.Length - 1)) ?  command.Substring(i, 1) : command.Substring(i, 2);

                foreach (string dir in directions)
                {
                    if (toMatch.StartsWith(dir))
                    {
                        split.Add(dir);

                        i += dir.Length;

                        break;
                    }
                }
            }

            return split;
        }

        public long Compute()
        {
            ReadInput();

            HexTile centerTile = new HexTile { GridX = 0, GridY = 0 };
            tiles[centerTile.ToString()] = centerTile;

            foreach (string cmdList in flipCommands)
            {
                int gridX = 0;
                int gridY = 0;

                foreach (string cmd in SplitCommands(cmdList))
                {
                    switch (cmd)
                    {
                        case "e":
                            gridX += 1;
                            break;

                        case "se":
                            gridY += 1;
                            break;

                        case "sw":
                            gridX -= 1;
                            gridY += 1;
                            break;

                        case "w":
                            gridX -= 1;
                            break;

                        case "nw":
                            gridY -= 1;
                            break;

                        case "ne":
                            gridX += 1;
                            gridY -= 1;
                            break;
                    }
                }

                string hash = gridX + "," + gridY;

                if (!tiles.ContainsKey(hash))
                {
                    tiles[hash] = new HexTile { GridX = gridX, GridY = gridY };
                }

                tiles[hash].Flipped = !tiles[hash].Flipped;

            }

            int numFlipped = 0;

            foreach (HexTile tile in tiles.Values)
            {
                if (tile.Flipped)
                    numFlipped++;
            }

            return numFlipped;
        }

        void Cycle()
        {
            // First populate any non-existing neighbors
            foreach (HexTile tile in tiles.Values.ToArray())
            {
                foreach (var neighborCoords in tile.Neighbors())
                {
                    string neighborHash = neighborCoords.Key + "," + neighborCoords.Value;

                    if (!tiles.ContainsKey(neighborHash))
                    {
                        tiles[neighborHash] = new HexTile { GridX = neighborCoords.Key, GridY = neighborCoords.Value };
                    }
                }
            }

            Dictionary<string, HexTile> newTiles = new Dictionary<string, HexTile>();

            foreach (HexTile tile in tiles.Values)
            {
                int numFlippedNeighbors = 0;

                foreach (var neighborCoords in tile.Neighbors())
                {
                    string neighborHash = neighborCoords.Key + "," + neighborCoords.Value;

                    if (tiles.ContainsKey(neighborHash))
                    {
                        HexTile neighbor = tiles[neighborHash];

                        if (neighbor.Flipped)
                            numFlippedNeighbors++;
                    }
                }

                HexTile newTile = new HexTile { GridX = tile.GridX, GridY = tile.GridY, Flipped = tile.Flipped };

                if (newTile.Flipped)
                {
                    if ((numFlippedNeighbors == 0) || (numFlippedNeighbors > 2))
                    {
                        newTile.Flipped = false;
                    }
                }
                else
                {
                    if (numFlippedNeighbors == 2)
                    {
                        newTile.Flipped = true;
                    }
                }

                newTiles[newTile.ToString()] = newTile;
            }

            tiles = newTiles;
        }

        public long Compute2()
        {
            Compute();

            int numFlipped = 0;

            for (int turn = 0; turn < 100; turn++)
            {
                Cycle();
                numFlipped = 0;

                foreach (HexTile tile in tiles.Values)
                {
                    if (tile.Flipped)
                        numFlipped++;
                }
            }
            
            return numFlipped;
        }
    }
}
