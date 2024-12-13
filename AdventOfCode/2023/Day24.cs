using SkiaSharp;

namespace AdventOfCode._2023
{
    internal class Day24 : Day
    {
        class HailStone
        {
            public LongVec3 Position;
            public LongVec3 Velocity;

            public LongVec2 PositionXY { get { return new LongVec2(Position.X, Position.Y); } }
            public LongVec2 VelocityXY { get { return new LongVec2(Velocity.X, Velocity.Y); } }

            public override string ToString()
            {
                return Position + " @ " + Velocity;
            }
        }

        List<HailStone> hailStones = new();

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] posVel = line.Split(" @ ");

                hailStones.Add(new HailStone()
                {
                    Position = new LongVec3(posVel[0].ToLongs(',').ToArray()),
                    Velocity = new LongVec3(posVel[1].ToLongs(',').ToArray())
                });
            }
        }

        //BigInteger min = 7;
        //BigInteger max = 27;
        BigInteger min = 200000000000000;
        BigInteger max = 400000000000000;

        (double X, double Y)? Intersect(HailStone hailStone1, HailStone hailStone2)
        {
            //Console.WriteLine("Hailstone A: " + hailStone1);
            //Console.WriteLine("Hailstone B: " + hailStone2);

            Ray2D<long> ray1 = new (hailStone1.PositionXY, hailStone1.PositionXY + hailStone1.VelocityXY);
            Ray2D<long> ray2 = new (hailStone2.PositionXY, hailStone2.PositionXY + hailStone2.VelocityXY);

            var intersect = Ray2D<long>.Intersect(ray1, ray2);

            if (intersect.Det != 0)
            {
                //Console.WriteLine("Intersect is: " + ((double)intersect.Point.X / (double)intersect.Det) + "," + ((double)intersect.Point.Y / (double)intersect.Det));

                bool inside = false;

                // Flip signs if we multiplied by a negative
                if (intersect.Det > 0)
                {
                    inside = (intersect.Point.X >= (min * intersect.Det)) && (intersect.Point.X <= (max * intersect.Det)) && (intersect.Point.Y >= (min * intersect.Det)) && (intersect.Point.Y <= (max * intersect.Det));
                }
                else
                {
                    inside = (intersect.Point.X <= (min * intersect.Det)) && (intersect.Point.X >= (max * intersect.Det)) && (intersect.Point.Y <= (min * intersect.Det)) && (intersect.Point.Y >= (max * intersect.Det));
                }

                if (inside)
                {
                    //Console.WriteLine("**inside");

                    int signX1 = Math.Sign((intersect.Point.X - (hailStone1.Position.X * intersect.Det)));

                    // Flip signs if we multiplied by a negative
                    if (intersect.Det < 0)
                        signX1 = -signX1;

                    if (signX1 != Math.Sign(hailStone1.Velocity.X))
                    {
                        return null;
                    }

                    int signX2 = Math.Sign(intersect.Point.X - (hailStone2.Position.X * intersect.Det));

                    // Flip signs if we multiplied by a negative
                    if (intersect.Det < 0)
                        signX2 = -signX2;

                    if (signX2 != Math.Sign(hailStone2.Velocity.X))
                    {
                        return null;
                    }

                    return ((double)intersect.Point.X / (double)intersect.Det, (double)intersect.Point.Y / (double)intersect.Det);
                }
            }

            //Console.WriteLine("** Parallel **");

            return null;
        }

        public override long Compute()
        {
            ReadData();

            long count = 0;

            for (int h1 = 0; h1 < hailStones.Count; h1++)
            {
                for (int h2 = h1 + 1; h2 < hailStones.Count; h2++)
                {
                    HailStone hailStone1 = hailStones[h1];
                    HailStone hailStone2 = hailStones[h2];

                    var intersect = Intersect(hailStone1, hailStone2);

                    if (intersect != null)
                        count++;
                }
            }

            return count;
        }

        public override long Compute2()
        {
            ReadData();

            long maxDist = 0;
            (int, int, (double, double)) max = new();

            for (int h1 = 0; h1 < hailStones.Count; h1++)
            {
                for (int h2 = h1 + 1; h2 < hailStones.Count; h2++)
                {
                    HailStone hailStone1 = hailStones[h1];
                    HailStone hailStone2 = hailStones[h2];

                    var intersect = Intersect(hailStone1, hailStone2);

                    if (intersect != null)
                    {
                        long dist = hailStone1.VelocityXY.ManhattanDistance(hailStone2.VelocityXY);

                        if (dist > maxDist)
                        {
                            maxDist = dist;
                            max = (h1, h2, intersect.Value);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
