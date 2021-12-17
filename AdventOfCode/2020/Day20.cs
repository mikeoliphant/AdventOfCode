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

        public void ComputeAllEdges()
        {
            AllEdges = new char[8][];

            AllEdges[0] = (from x in Enumerable.Range(0, Width) select this[x, 0]).ToArray();
            AllEdges[2] = (from x in Enumerable.Range(0, Width) select this[x, Height - 1]).ToArray();
            AllEdges[4] = (from y in Enumerable.Range(0, Height) select this[0, y]).ToArray();
            AllEdges[6] = (from y in Enumerable.Range(0, Height) select this[Width - 1, y]).ToArray();

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

            ConnectTiles(startPos);

            TilePos upperLeft = null;

            foreach (TilePos pos in posDict.Values)
            {
                if ((pos.Neighbors[0] == null) && (pos.Neighbors[2] == null))
                {
                    upperLeft = pos;

                    break;
                }
            }



            tileGrid = new TileGrid().CreateData(tileSize, tileSize) as TileGrid;

            tileGrid[0, 0] = upperLeft;

            return 0;
        }
    }
}
