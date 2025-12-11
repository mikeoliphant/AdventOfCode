
namespace AdventOfCode._2025
{
    internal class Day9 : Day
    {
        long GetArea(Vec2<long> a, Vec2<long> b)
        {
            long width = Math.Abs(a.X - b.X) + 1;
            long height = Math.Abs(a.Y - b.Y) + 1;

            return width * height;
        }

        public override long Compute()
        {
            List<Vec2<long>> tiles = new();

            foreach (string line in File.ReadLines(DataFile))
            {
                tiles.Add(new Vec2<long>(line.ToLongs(',').ToArray()));
            }

            long maxArea = 0;

            foreach (var pair in CollectionHelper.GetIndexPairs(tiles.Count))
            {
                long area = GetArea(tiles[pair.I1], tiles[pair.I2]);

                if (area > maxArea)
                {
                    maxArea = area;
                }

            }

            return maxArea;
        }

        public override long Compute2()
        {
            List<Vec2<long>> tiles = new();

            foreach (string line in File.ReadLines(DataFileTest))
            {
                tiles.Add(new Vec2<long>(line.ToLongs(',').ToArray()));
            }

            List<(int X, int Y)> winding = new();

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                var nextTile = tiles[(i + 1) % tiles.Count];

                winding.Add((Math.Sign(nextTile.X - tile.X), Math.Sign(nextTile.Y - tile.Y)));
            }

            long maxArea = 0;

            foreach (var pair in CollectionHelper.GetIndexPairs(tiles.Count))
            {
                var corner1 = tiles[pair.I1];
                var corner2 = tiles[pair.I2];

                if ((Math.Sign(corner2.X - corner1.X) != winding[pair.I1].X) || (Math.Sign(corner2.Y - corner1.Y) != winding[pair.I1].Y))
                    continue;

                Range width = (corner1.X < corner2.X) ? new Range(corner1.X, corner2.X) : new Range(corner2.X, corner1.X);
                Range height = (corner1.Y < corner2.Y) ? new Range(corner1.Y, corner2.Y) : new Range(corner2.Y, corner1.Y);

                long area = width.Size() * height.Size();

                if (area > maxArea)
                {
                    bool haveInteriorCorner = false;

                    foreach (var tile in tiles)
                    {
                        if (width.ContainsNonInclusive(tile.X) && height.ContainsNonInclusive(tile.Y))
                        {
                            haveInteriorCorner = true;

                            break;
                        }
                    }

                    if (haveInteriorCorner)
                        continue;

                    maxArea = area;
                }
            }

            return maxArea;
        }
    }
}
