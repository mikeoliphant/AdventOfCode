using System.Security.Policy;

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

        public LongVec2((long X, long Y) pos)
        {
            this.X = pos.X;
            this.Y = pos.Y;
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

        public static LongVec2 MaxValue { get { return new LongVec2(long.MaxValue, long.MaxValue); } }
        public static LongVec2 MinValue { get { return new LongVec2(long.MinValue, long.MinValue); } }

        public void Deconstruct(out long X, out long Y)
        {
            X = this.X;
            Y = this.Y;
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
            return !v1.Equals(v2);
        }

        public static LongVec2 operator +(LongVec2 a, LongVec2 b) => new LongVec2(a.X + b.X, a.Y + b.Y);

        public static LongVec2 operator -(LongVec2 a, LongVec2 b) => new LongVec2(a.X - b.X, a.Y - b.Y);

        public static LongVec2 operator *(LongVec2 a, LongVec2 b) => new LongVec2(a.X * b.X, a.Y * b.Y);
        public static LongVec2 operator *(LongVec2 a, long b) => new LongVec2(a.X * b, a.Y * b);
        public static LongVec2 operator *(long a, LongVec2 b) => new LongVec2(a * b.X, a * b.Y);

        public static LongVec2 operator /(LongVec2 a, LongVec2 b) => new LongVec2(a.X / b.X, a.Y / b.Y);
        public static LongVec2 operator /(LongVec2 a, long b) => new LongVec2(a.X / b, a.Y / b);
        public static LongVec2 operator /(long a, LongVec2 b) => new LongVec2(a / b.X, a / b.Y);

        public static int TurnFacing(int facing, int rotation)
        {
            return ModHelper.PosMod(facing + rotation, 4);
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

        public LongVec3(long xyz)
        {
            this.X = xyz;
            this.Y = xyz;
            this.Z = xyz;
        }

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
            get { return new LongVec3(0); }
        }

        public static LongVec3 One
        {
            get { return new LongVec3(1); }
        }

        public static LongVec3 MaxValue { get { return new LongVec3(long.MaxValue); } }
        public static LongVec3 MinValue { get { return new LongVec3(long.MinValue); } }

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

        public IEnumerable<LongVec3> GetNeighbors()
        {
            yield return (this + new LongVec3(1, 0, 0));
            yield return (this + new LongVec3(-1, 0, 0));
            yield return (this + new LongVec3(0, 1, 0));
            yield return (this + new LongVec3(0, -1, 0));
            yield return (this + new LongVec3(0, 0, 1));
            yield return (this + new LongVec3(0, 0, -1));
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
