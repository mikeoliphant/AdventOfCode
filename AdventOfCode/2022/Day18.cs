using SkiaSharp;

namespace AdventOfCode._2022
{
    internal class Day18 : Day
    {
        Dictionary<LongVec3, bool> points = new Dictionary<LongVec3, bool>();

        void ReadInput(string file)
        {
            foreach (string point in File.ReadLines(file))
            {
                points[new LongVec3(point.ToLongs(',').ToArray())] = true;
            }
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            long surface = 0;

            foreach (LongVec3 point in points.Keys)
            {
                foreach (LongVec3 neighbor in point.GetNeighbors())
                {
                    if (!points.ContainsKey(neighbor))
                        surface++;
                }
            }

            return surface;
        }


        public override long Compute2()
        {
            ReadInput(DataFile);

            LongVec3 min = LongVec3.MaxValue;
            LongVec3 max = LongVec3.MinValue;

            foreach (LongVec3 point in points.Keys)
            {
                min.X = Math.Min(min.X, point.X);
                min.Y = Math.Min(min.Y, point.Y);
                min.Z = Math.Min(min.Z, point.Z);

                max.X = Math.Max(max.X, point.X);
                max.Y = Math.Max(max.Y, point.Y);
                max.Z = Math.Max(max.Z, point.Z);
            }

            DijkstraSearch<LongVec3> search = new DijkstraSearch<LongVec3>(delegate (LongVec3 point) { return point.GetNeighbors().Where(p => !points.ContainsKey(p)); });

            var toTest = points.Keys.SelectMany(p => p.GetNeighbors().Where(p => !points.ContainsKey(p))).Distinct();

            Dictionary<LongVec3, bool> trapped = new Dictionary<LongVec3, bool>();

            foreach (LongVec3 point in toTest)
            {
                var result = search.GetShortestPath(point, delegate (LongVec3 point) { return (point.X < min.X) || (point.X > max.X) || (point.Y < min.Y) || (point.Y > max.Y) || (point.Z < min.Z) || (point.Z > max.Z); });

                if (result.Path == null)
                {
                    trapped[point] = true;
                }
            }

            long surface = 0;

            foreach (LongVec3 point in points.Keys.SelectMany(p => p.GetNeighbors()))
            {
                if (!points.ContainsKey(point) && !trapped.ContainsKey(point))
                    surface++;
            }

            return surface;
        }
    }
}
