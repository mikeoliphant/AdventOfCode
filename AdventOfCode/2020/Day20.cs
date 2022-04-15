namespace AdventOfCode._2020
{
    public class Tile : Grid<char>
    {
        public int ID { get; private set; }

        public Tile(int id)
        {
            this.ID = id;
        }

        public override string ToString()
        {
            return ID.ToString();
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

        void ReadInput()
        {
            foreach (string tile in File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day20.txt").SplitParagraphs())
            {
                string[] lines = tile.SplitLines();

                int tileID = int.Parse(lines[0].Substring(5, 4));

                tiles[tileID] = new Tile(tileID).CreateDataFromRows(lines.Skip(1).ToArray()) as Tile;
            }

            tileSize = (int)Math.Sqrt(tiles.Count);
        }

        Dictionary<int, TilePos> posDict = new Dictionary<int, TilePos>();

        void ConnectTiles(TilePos tilePos)
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
                            haveMatch = false;
                            bool flip = false;

                            if (tile.GetEdge(edge).SequenceEqual(toMatch.GetEdge(rotation)))
                            {
                                haveMatch = true;
                                flip = true;
                            }
                            else if (tile.GetEdge(edge).SequenceEqual(toMatch.GetEdge(rotation).Reverse()))
                            {
                                haveMatch = true;
                            }

                            if (haveMatch)
                            {
                                //tile.PrintToConsole();
                                //tiles[toMatch.ID].PrintToConsole();

                                if (!posDict.ContainsKey(toMatch.ID))
                                {
                                    if ((neighborEdge != rotation) || flip)
                                    {
                                        int neededRotation = neighborEdge - rotation;
                                        if (neededRotation < 0)
                                            neededRotation += 4;

                                        bool flipX = false;
                                        bool flipY = false;

                                        if (flip)
                                        {
                                            if ((edge == 0) || (edge == 2))
                                                flipX = true;
                                            else
                                                flipY = true;
                                        }

                                        Tile newTile = new Tile(toMatch.ID).CreateData(toMatch.Width, toMatch.Height) as Tile;

                                        for (int y = 0; y < newTile.Height; y++)
                                        {
                                            for (int x = 0; x < newTile.Width; x++)
                                            {
                                                newTile[x, y] = toMatch.GetTransformedValue(x, y, neededRotation, flipX, flipY);

                                                tiles[newTile.ID] = newTile;
                                            }
                                        }

                                        if (!tile.GetEdge(edge).SequenceEqual(newTile.GetEdge(neighborEdge).Reverse()))
                                        {
                                            throw new Exception();
                                        }
                                    }

                                    TilePos newPos = new TilePos { ID = toMatch.ID };
                                    posDict.Add(toMatch.ID, newPos);

                                    ConnectTiles(newPos);
                                }

                                if ((posDict[toMatch.ID].Neighbors[neighborEdge] != null) && (posDict[toMatch.ID].Neighbors[neighborEdge] != tilePos))
                                    throw new Exception();

                                tilePos.Neighbors[edge] = posDict[toMatch.ID];
                                posDict[toMatch.ID].Neighbors[neighborEdge] = tilePos;

                                //tiles[toMatch.ID].PrintToConsole();
                                //Console.WriteLine();

                                break;
                            }
                        }

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

            TilePos startPos = new TilePos { ID = tiles.Keys.First() };
            posDict[startPos.ID] = startPos;

            ConnectTiles(startPos);

            long cornerMult = 1;

            foreach (TilePos tile in posDict.Values)
            {
                int numUnmatching = (from neighbor in tile.Neighbors where neighbor != null select neighbor).Count();

                if (numUnmatching == 2)
                {
                    cornerMult *= tile.ID;
                }
            }

            return cornerMult;
        }

        bool[] bools = new bool[] { true, false };

        Grid<char> FindSeaMonsters(Grid<char> grid, Grid<char> seaMonster)
        {
            int numMonsters = 0;

            for (int rotation = 0; rotation < 4; rotation++)
            {
                foreach (bool flipX in bools)
                {
                    foreach (bool flipY in bools)
                    {
                        Grid<char> transformedGrid = grid.Transform(rotation, flipX, flipY);

                        for (int y = 0; y < transformedGrid.Height - seaMonster.Height; y++)
                        {
                            for (int x = 0; x < transformedGrid.Width - seaMonster.Width; x++)
                            {
                                if (transformedGrid.MatchesPattern(seaMonster, x, y, ' '))
                                {
                                    numMonsters++;

                                    for (int py = 0; py < seaMonster.Height; py++)
                                    {
                                        for (int px = 0; px < seaMonster.Width; px++)
                                        {
                                            if (seaMonster[px, py] == '#')
                                            {
                                                transformedGrid[x + px, y + py] = 'O';
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (numMonsters > 0)
                        {
                            return transformedGrid;
                        }
                    }
                }
            }

            return null;
        }

        public long Compute2()
        {
            ReadInput();

            TilePos startPos = new TilePos { ID = tiles.Keys.First() };
            posDict[startPos.ID] = startPos;

            ConnectTiles(startPos);

            TilePos upperLeft = null;

            foreach (TilePos tile in posDict.Values)
            {
                if ((tile.Neighbors[0] == null) && (tile.Neighbors[3] == null))
                {
                    upperLeft = tile;

                    break;
                }
            }

            Grid<char> seaMonster = new Grid<char>().CreateDataFromRows(new string[]
                {
                    "                  # ",
                    "#    ##    ##    ###",
                    " #  #  #  #  #  #   "
                });

            Grid<char> bigGrid = new Grid<char>(tileSize * 8, tileSize * 8);

            TilePos drawTile = upperLeft;

            for (int y = 0; y < tileSize; y++)
            {
                TilePos rowTile = drawTile;

                for (int x = 0; x < tileSize; x++)
                {
                    Grid<char>.Copy(tiles[drawTile.ID], bigGrid, 1, 1, 8, 8, x * 8, y * 8);

                    drawTile = drawTile.Neighbors[1];
                }

                drawTile = rowTile.Neighbors[2];
            }

            Grid<char> found = FindSeaMonsters(bigGrid, seaMonster);

            found.PrintToConsole();

            int numNotMonster = (from c in found.GetAllValues() where c == '#' select c).Count();

            return numNotMonster;
        }
    }
}
