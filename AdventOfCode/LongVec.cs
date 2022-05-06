namespace AdventOfCode
{
    struct LongVec2 : IEquatable<LongVec2>
    {
        public long X { get; set; }
        public long Y { get; set; }

        public LongVec2(long x, long y)
        {
            this.X = x;
            this.Y = y;
        }

        public LongVec2(long[] components)
        {
            this.X = components[0];
            this.Y = components[1];
        }

        public static LongVec2 Zero
        {
            get { return new LongVec2(0, 0); }
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        public long ManhattanDistance(LongVec2 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public override bool Equals(object obj)
        {
            return (obj is LongVec2) && this.Equals(((LongVec2)obj));
        }

        public bool Equals(LongVec2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public static bool operator ==(LongVec2 v1, LongVec2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(LongVec2 v1, LongVec2 v2)
        {
            return v1 != v2;
        }

        public static LongVec2 operator +(LongVec2 a, LongVec2 b) => new LongVec2(a.X + b.X, a.Y + b.Y);

        public static LongVec2 operator -(LongVec2 a, LongVec2 b) => new LongVec2(a.X - b.X, a.Y - b.Y);

        public static LongVec2 operator *(LongVec2 a, LongVec2 b) => new LongVec2(a.X * b.X, a.Y * b.Y);

        public static LongVec2 operator /(LongVec2 a, LongVec2 b) => new LongVec2(a.X / b.X, a.Y / b.Y);

        public static int TurnFacing(int facing, int rotation)
        {
            return MathHelper.PosMod(facing + rotation, 4);
        }

        public void AddFacing(int facing, long amount)
        {
            switch (facing)
            {
                case 0:
                    Y -= amount;
                    break;

                case 1:
                    X += amount;
                    break;

                case 2:
                    Y += amount;
                    break;

                case 3:
                    X -= amount;
                    break;
            }
        }
    }

    struct LongVec3 : IEquatable<LongVec3>
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public LongVec3(long x, long y, long z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public LongVec3(long[] components)
        {
            this.X = components[0];
            this.Y = components[1];
            this.Z = components[2];
        }

        public static LongVec3 Zero
        {
            get { return new LongVec3(0, 0, 0); }
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }

        public long ManhattanDistance(LongVec3 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public override bool Equals(object obj)
        {
            return (obj is LongVec3) && ((LongVec3)obj).Equals(this);
        }

        public bool Equals(LongVec3 other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        public static bool operator ==(LongVec3 v1, LongVec3 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(LongVec3 v1, LongVec3 v2)
        {
            return v1 != v2;
        }

        public static LongVec3 operator +(LongVec3 a, LongVec3 b) => new LongVec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static LongVec3 operator -(LongVec3 a, LongVec3 b) => new LongVec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static LongVec3 operator *(LongVec3 a, LongVec3 b) => new LongVec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static LongVec3 operator /(LongVec3 a, LongVec3 b) => new LongVec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
}
