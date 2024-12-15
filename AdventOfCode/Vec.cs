global using IntVec2 = AdventOfCode.Vec2<int>;
global using LongVec2 = AdventOfCode.Vec2<long>;

global using IntVec3 = AdventOfCode.Vec3<int>;
global using LongVec3 = AdventOfCode.Vec3<long>;

namespace AdventOfCode
{
    public interface IPos2D<T>
    {
        public T X { get; }
        public T Y { get; }
    }

    public struct Vec2<T> : IEquatable<Vec2<T>>, IPos2D<T> where T : INumber<T>, IMinMaxValue<T>, IEquatable<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Vec2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vec2((T X, T Y) pos)
        {
            this.X = pos.X;
            this.Y = pos.Y;
        }

        public Vec2((long X, long Y) pos)
        {
            this.X = T.CreateChecked(pos.X);
            this.Y = T.CreateChecked(pos.Y);
        }

        public Vec2((int X, int Y) pos)
        {
            this.X = T.CreateChecked(pos.X);
            this.Y = T.CreateChecked(pos.Y);
        }

        public Vec2(T[] components)
        {
            this.X = components[0];
            this.Y = components[1];
        }

        public static Vec2<T> Zero
        {
            get { return new Vec2<T>(T.Zero, T.Zero); }
        }

        public static Vec2<T> MaxValue { get { return new Vec2<T>(T.MaxValue, T.MaxValue); } }
        public static Vec2<T> MinValue { get { return new Vec2<T>(T.MinValue, T.MinValue); } }

        public void Deconstruct(out T X, out T Y)
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

        public T ManhattanDistance(Vec2<T> other)
        {
            return T.Abs(X - other.X) + T.Abs(Y - other.Y);
        }

        public override bool Equals(object obj)
        {
            return (obj is Vec2<T>) && this.Equals(((Vec2<T>)obj));
        }

        public bool Equals(Vec2<T> other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public static bool operator ==(Vec2<T> v1, Vec2<T> v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vec2<T> v1, Vec2<T> v2)
        {
            return !v1.Equals(v2);
        }

        public static Vec2<T> operator +(Vec2<T> a, Vec2<T> b) => new Vec2<T>(a.X + b.X, a.Y + b.Y);

        public static Vec2<T> operator -(Vec2<T> a, Vec2<T> b) => new Vec2<T>(a.X - b.X, a.Y - b.Y);

        public static Vec2<T> operator *(Vec2<T> a, Vec2<T> b) => new Vec2<T>(a.X * b.X, a.Y * b.Y);
        public static Vec2<T> operator *(Vec2<T> a, T b) => new Vec2<T>(a.X * b, a.Y * b);
        public static Vec2<T> operator *(T a, Vec2<T> b) => new Vec2<T>(a * b.X, a * b.Y);

        public static Vec2<T> operator /(Vec2<T> a, Vec2<T> b) => new Vec2<T>(a.X / b.X, a.Y / b.Y);
        public static Vec2<T> operator /(Vec2<T> a, T b) => new Vec2<T>(a.X / b, a.Y / b);
        public static Vec2<T> operator /(T a, Vec2<T> b) => new Vec2<T>(a / b.X, a / b.Y);

        public static int TurnFacing(int facing, int rotation)
        {
            return ModHelper.PosMod(facing + rotation, 4);
        }

        public static int GetFacing(char facingChar)
        {
            switch (facingChar)
            {
                case '^':
                    return 0;
                case '>':
                    return 1;
                case 'v':
                    return 2;
                case '<':
                    return 3;
                default:
                    throw new Exception("Facing must be one of: [^>v<]");

            }
        }

        public static Vec2<T> GetFacingVector(char facingChar)
        {
            switch (facingChar)
            {
                case '^':
                    return new Vec2<T>(T.Zero, -T.One);
                case '>':
                    return new Vec2<T>(T.One, T.Zero);
                case 'v':
                    return new Vec2<T>(T.Zero, T.One);
                case '<':
                    return new Vec2<T>(-T.One, T.Zero);
                default:
                    throw new Exception("Facing must be one of: [^>v<]");

            }
        }


        public void AddFacing(int facing, T amount)
        {
            // Start with 0 being up, and go clockwise
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

    struct Vec3<T> : IEquatable<Vec3<T>> where T : INumber<T>, IMinMaxValue<T>, IEquatable<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public Vec3(T xyz)
        {
            this.X = xyz;
            this.Y = xyz;
            this.Z = xyz;
        }

        public Vec3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vec3(T[] components)
        {
            this.X = components[0];
            this.Y = components[1];
            this.Z = components[2];
        }

        public static Vec3<T> Zero
        {
            get { return new Vec3<T>(T.Zero); }
        }

        public static Vec3<T> One
        {
            get { return new Vec3<T>(T.One); }
        }

        public static Vec3<T> MaxValue { get { return new Vec3<T>(T.MaxValue); } }
        public static Vec3<T> MinValue { get { return new Vec3<T>(T.MinValue); } }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }

        public T ManhattanDistance(Vec3<T> other)
        {
            return T.Abs(X - other.X) + T.Abs(Y - other.Y) + T.Abs(Z - other.Z);
        }

        public IEnumerable<Vec3<T>> GetNeighbors()
        {
            yield return (this + new Vec3<T>(T.One, T.Zero, T.Zero));
            yield return (this + new Vec3<T>(-T.One, T.Zero, T.Zero));
            yield return (this + new Vec3<T>(T.Zero, T.One, T.Zero));
            yield return (this + new Vec3<T>(T.Zero, -T.One, T.Zero));
            yield return (this + new Vec3<T>(T.Zero, T.Zero, T.One));
            yield return (this + new Vec3<T>(T.Zero, T.Zero, -T.One));
        }

        public void Divide(T val)
        {
            X /= val;
            Y /= val;
            Z /= val;
        }

        public override bool Equals(object obj)
        {
            return (obj is Vec3<T>) && ((Vec3<T>)obj).Equals(this);
        }

        public bool Equals(Vec3<T> other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        public static bool operator ==(Vec3<T> v1, Vec3<T> v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vec3<T> v1, Vec3<T> v2)
        {
            return !(v1.Equals(v2));
        }

        public static Vec3<T> operator +(Vec3<T> a, Vec3<T> b) => new Vec3<T>(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vec3<T> operator -(Vec3<T> a, Vec3<T> b) => new Vec3<T>(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vec3<T> operator *(Vec3<T> a, Vec3<T> b) => new Vec3<T>(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static Vec3<T> operator /(Vec3<T> a, Vec3<T> b) => new Vec3<T>(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
}
