using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Tile : Grid<char>
    {
        public int ID { get; private set; }
        public char[][] AllEdges { get; private set; }
        public List<int>[] EdgeTileMatches { get; private set; }
        public List<int> UnmatchingEdges { get; private set; }

        public Tile(int id)
        {
            this.ID = id;
        }

        public char GetTransformedValue(int x, int y, int rotation, bool flipX, bool flipY)
        {
            if (flipX)
                x = Width - x - 1;

            if (flipY)
                y = Height - y - 1;

            switch (rotation)
            {
                case 0:
                    return GetValue(x, y);

                case 1:
                    return GetValue(Height - y - 1, x);

                case 2:
                    return GetValue(Width - x - 1, Height - y - 1);

                case 3:
                    return GetValue(y, Height - y - x);

            }

            throw new ArgumentException("'rotation' must be 0-3");
        }

        public IEnumerable<char> GetEdge(int edge)
        {
            switch (edge)
            {
                case 0:
                    return (from x in Enumerable.Range(0, Width) select this[x, 0]);

                case 1:
                    return (from y in Enumerable.Range(0, Height) select this[Width - 1, y]);

                case 2:
                    return (from x in Enumerable.Range(0, Width).Reverse() select this[x, Height - 1]);

                case 3:
                    return (from y in Enumerable.Range(0, Height).Reverse() select this[0, y]);
            }

            throw new ArgumentException("'edge' must be 0-3");
        }

        public void ComputeAllEdges()
        {
            AllEdges = new char[8][];

            AllEdges[0] = (from x in Enumerable.Range(0, Width) select this[x, 0]).ToArray();
            AllEdges[2] = (from y in Enumerable.Range(0, Height) select this[Width - 1, y]).ToArray();
            AllEdges[4] = (from y in Enumerable.Range(0, Height) select this[0, y]).ToArray();
            AllEdges[6] = (from x in Enumerable.Range(0, Width) select this[x, Height - 1]).ToArray();

            for (int edge = 0; edge < 8; edge += 2)
            {
                AllEdges[edge + 1] = AllEdges[edge].Reverse().ToArray();
            }

            EdgeTileMatches = new List<int>[8];

            for (int edge = 0; edge < 8; edge++)
                EdgeTileMatches[edge] = new List<int>();

            UnmatchingEdges = new List<int>();
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    public class TileGrid : Grid<TilePos>
    {
        public TileGrid()
        {
        }
    }

    public class TilePos
    {
        public int ID { get; set; }
        public TilePos[] Neighbors { get; set; }

        public TilePos()
        {
            Neighbors = new TilePos[4];
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    public class Day20
    {
        Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();
        int tileSize;
        TileGrid tileGrid;

        void ReadInput()
        {
            foreach (string tile in File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day20Test.txt").SplitParagraphs())
            {
                string[] lines = tile.SplitLines();

                int tileID = int.Parse(lines[0].Substring(5, 4));

                tiles[tileID] = new Tile(tileID).CreateDataFromRows(lines.Skip(1).ToArray()) as Tile;

                tiles[tileID].ComputeAllEdges();
            }

            tileSize = (int)Math.Sqrt(tiles.Count);
        }

        void ComputeEdges()
        {
            foreach (var tile1 in tiles)
            {
                for (int edgePos = 0; edgePos < tile1.Value.AllEdges.Length; edgePos++)
                {
                    char[] edge1 = tile1.Value.AllEdges[edgePos];

                    bool hasMatch = false;

                    foreach (var tile2 in tiles)
                    {
                        if (tile1.Key != tile2.Key)
                        {
                            foreach (char[] edge2 in tile2.Value.AllEdges)
                            {
                                if (Enumerable.SequenceEqual(edge1, edge2))
                                {
                                    hasMatch = true;

                                    tile1.Value.EdgeTileMatches[edgePos].Add(tile2.Key);

                                    break;
                                }
                            }
                        }
                    }

                    if (!hasMatch)
                    {
                        tile1.Value.UnmatchingEdges.Add(edgePos);
                    }
                }
            }
        }

        Dictionary<int, TilePos> posDict = new Dictionary<int, TilePos>();

        void ConnectTiles(TilePos tilePos)
        {
            Tile tile = tiles[tilePos.ID];

            for (int i = 0; i < 4; i++)
            {
                if (tilePos.Neighbors[i] == null)
                {
                    if (tile.EdgeTileMatches[i * 2].Count > 0)
                    {
                        int neighborID = tile.EdgeTileMatches[i * 2][0];

                        TilePos neighbor;

                        if (posDict.ContainsKey(neighborID))
                        {
                            neighbor = posDict[neighborID];
                        }
                        else
                        {
                            neighbor = new TilePos { ID = neighborID };
                        }

                        for (int neighborEdge = 0; neighborEdge < 4; neighborEdge++)
                        {
                            if ((tiles[neighborID].EdgeTileMatches[neighborEdge * 2].Count > 0) && (tiles[neighborID].EdgeTileMatches[neighborEdge * 2][0] == tile.ID))
                            {
                                neighbor.Neighbors[neighborEdge] = tilePos;
                            }
                        }

                        tilePos.Neighbors[i] = neighbor;

                        if (!posDict.ContainsKey(neighborID))
                        {
                            posDict[neighborID] = neighbor;

                            ConnectTiles(neighbor);
                        }
                    }                            
                }
            }
        }

        bool[] bools = new bool[] { false, true };

        void ConnectTiles2(TilePos tilePos)
        {
            Tile tile = tiles[tilePos.ID];

            for (int edge = 0; edge < 4; edge++)
            {
                if (tilePos.Neighbors[edge] == null)
                {
                    int neighborEdge = (edge + 2) % 4;

                    foreach (Tile toMatch in tiles.Values)
                    {
                        if (toMatch == tile)
                            continue;

                        bool haveMatch = false;

                        for (int rotation = 0; rotation < 4; rotation++)
                        {
                            foreach (bool flipX in bools)
                            {
                                foreach (bool flipY in bools)
                                {
                                    haveMatch = true;

                                    for (int pos = 0; pos < toMatch.Width; pos++)
                                    {
                                        if (tile.AllEdges[edge * 2][pos] != toMatch.GetTransformedValue(pos, 0, rotation, flipX, flipY))
                                        {
                                            haveMatch = false;

                                            break;
                                        }
                                    }

                                    if (haveMatch)
                                    {
                                        if (!posDict.ContainsKey(toMatch.ID))
                                        {
                                            Tile newTile = new Tile(toMatch.ID).CreateData(toMatch.Width, toMatch.Height) as Tile;

                                            for (int y = 0; y < newTile.Height; y++)
                                            {
                                                for (int x = 0; x < newTile.Width; x++)
                                                {
                                                    newTile[x, y] = toMatch.GetTransformedValue(x, y, rotation, flipX, flipY);

                                                    tiles[newTile.ID] = newTile;
                                                }
                                            }

                                            TilePos newPos = new TilePos { ID = tile.ID };

                                            posDict.Add(toMatch.ID, newPos);

                                            tile.PrintToConsole();

                                            toMatch.PrintToConsole();
                                            newTile.PrintToConsole();
                                        }

                                        goto outofloop;
                                    }
                                }
                            }
                        }

                    outofloop:
                        if (haveMatch)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            ComputeEdges();

            long cornerMult = 1;

            foreach (Tile tile in tiles.Values)
            {
                if (tile.UnmatchingEdges.Count == 4)
                {
                    cornerMult *= tile.ID;
                }
            }

            return cornerMult;
        }

        public long Compute2()
        {
            ReadInput();

            ComputeEdges();

            TilePos startPos = new TilePos { ID = tiles.Keys.First() };

            ConnectTiles2(startPos);

            //ConnectTiles(startPos);

            //TilePos upperLeft = null;

            //foreach (TilePos pos in posDict.Values)
            //{
            //    if ((pos.Neighbors[0] == null) && (pos.Neighbors[2] == null))
            //    {
            //        upperLeft = pos;

            //        break;
            //    }
            //}

            //tileGrid = new TileGrid().CreateData(tileSize, tileSize) as TileGrid;

            //tileGrid[0, 0] = upperLeft;

            return 0;
        }
    }
}
