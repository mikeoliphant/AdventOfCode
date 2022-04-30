namespace AdventOfCode
{
    class LongVec3
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

        public long ManhattanDistance(LongVec3 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public static LongVec3 operator +(LongVec3 a, LongVec3 b) => new LongVec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static LongVec3 operator -(LongVec3 a, LongVec3 b) => new LongVec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static LongVec3 operator *(LongVec3 a, LongVec3 b) => new LongVec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static LongVec3 operator /(LongVec3 a, LongVec3 b) => new LongVec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
}
