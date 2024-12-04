using System.Security.Policy;

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

        class Ray2D
        {
            public BigInteger A;
            public BigInteger B;
            public BigInteger C;

            public Ray2D(LongVec2 p1, LongVec2 p2)
            {
                this.A = (BigInteger)p2.Y - (BigInteger)p1.Y;
                this.B = (BigInteger)p1.X - (BigInteger)p2.X;
                this.C = (this.A * (BigInteger)p1.X) + (this.B * (BigInteger)p1.Y);
            }

            public static ((BigInteger X, BigInteger Y) Point, BigInteger Det)  Intersect(Ray2D ray1, Ray2D ray2)
            {
                BigInteger det = (ray1.A * ray2.B) - (ray2.A * ray1.B);

                if (det == 0)
                {
                    return ((0, 0), 0);
                }

                return (((ray2.B * ray1.C) - (ray1.B * ray2.C), (ray1.A * ray2.C) - (ray2.A * ray1.C)), det);
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

                    Console.WriteLine("Hailstone A: " + hailStone1);
                    Console.WriteLine("Hailstone B: " + hailStone2);

                    Ray2D ray1 = new Ray2D(hailStone1.PositionXY, hailStone1.PositionXY + hailStone1.VelocityXY);
                    Ray2D ray2 = new Ray2D(hailStone2.PositionXY, hailStone2.PositionXY + hailStone2.VelocityXY);

                    var intersect = Ray2D.Intersect(ray1, ray2);

                    if (intersect.Det != 0)
                    {
                        Console.WriteLine("Intersect is: " + ((double)intersect.Point.X / (double)intersect.Det) + "," + ((double)intersect.Point.Y / (double)intersect.Det));

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
                            Console.WriteLine("**inside");

                            int signX1 = (intersect.Point.X - (hailStone1.Position.X * intersect.Det)).Sign;

                            // Flip signs if we multiplied by a negative
                            if (intersect.Det < 0)
                                signX1 = -signX1;

                            if (signX1 != Math.Sign(hailStone1.Velocity.X))
                            {
                                continue;
                            }

                            int signX2 = (intersect.Point.X - (hailStone2.Position.X * intersect.Det)).Sign;

                            // Flip signs if we multiplied by a negative
                            if (intersect.Det < 0)
                                signX2 = -signX2;

                            if (signX2 != Math.Sign(hailStone2.Velocity.X))
                            {
                                continue;
                            }

                            count++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("** Parallel **");
                    }

                    Console.WriteLine();
                }
            }

            return 0;
        }
    }
}
