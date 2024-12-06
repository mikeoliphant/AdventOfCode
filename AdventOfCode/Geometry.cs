global using IntRect = AdventOfCode.Rect<int>;
global using LongRect = AdventOfCode.Rect<long>;

namespace AdventOfCode
{
    /// <summary>
    /// A 2D line segment which is either horizontal or vertical
    /// </summary>
    public struct LineHV
    {
        public Range Range { get; set; } = new Range(1, 0);
        public long AxisValue { get; set; } = 0;
        public bool IsVertical { get; set; } = false;
        public long Length { get { return Range.Size(); } }
        public static LineHV Empty
        {
            get
            {
                return new LineHV();
            }
        }

        public LineHV()
        {
        }

        public LineHV(LongVec2 p1, LongVec2 p2)
        {
            if (p1.X == p2.X)
            {
                IsVertical = true;
                Range = (p1.Y < p2.Y) ? new Range(p1.Y, p2.Y) : new Range(p2.Y, p1.Y);
                AxisValue = p1.X;
            }
            else if (p1.Y == p2.Y)
            {
                IsVertical = false;
                Range = (p1.X < p2.X) ? new Range(p1.X, p2.X) : new Range(p2.X, p1.X);
                AxisValue = p1.Y;
            }
            else
            {
                throw new InvalidDataException("Points must represent either a horizontal or vertical line");
            }
        }

        public override string ToString()
        {
            return (IsVertical ? 'y' : 'x') + Range.ToString();
        }

        public LineHV? GetIntersection(LineHV other)
        {
            if (IsVertical == other.IsVertical)
            {
                // Parallel

                if (AxisValue == other.AxisValue)
                {
                    // Co-linear

                    return new LineHV()
                    {
                        IsVertical = this.IsVertical,
                        AxisValue = this.AxisValue,
                        Range = Range.Intersect(other.Range)
                    };
                }
                else
                    return null;
            }

            if (Range.Contains(other.AxisValue))
            {
                // Intersect
                return new LineHV()
                {
                    IsVertical = this.IsVertical,
                    AxisValue = AxisValue,
                    Range = new Range(other.AxisValue, other.AxisValue)
                };
            }

            return null;
        }

        public IEnumerable<LineHV> GetDisjoint(LineHV other)
        {
            if (IsVertical == other.IsVertical)
            {
                // Parallel

                if (AxisValue == other.AxisValue)
                {
                    // Co-linear

                    foreach (Range range in Range.GetDisjointFrom(other.Range))
                    {
                        yield return new LineHV()
                        {
                            IsVertical = this.IsVertical,
                            AxisValue = this.AxisValue,
                            Range = range
                        };
                    }
                }
                else
                {
                    yield return this;
                }
            }
            else
            {
                if (Range.Contains(other.AxisValue))
                {
                    // Intersect, so split in two

                    if (other.AxisValue > Range.Min)
                    {
                        yield return new LineHV()
                        {
                            IsVertical = this.IsVertical,
                            AxisValue = this.AxisValue,
                            Range = new Range(Range.Min, other.AxisValue - 1)
                        };
                    }

                    if (other.AxisValue < Range.Max)
                    {
                        yield return new LineHV()
                        {
                            IsVertical = this.IsVertical,
                            AxisValue = this.AxisValue,
                            Range = new Range(other.AxisValue + 1, Range.Max)
                        };
                    }
                }
                else
                {
                    yield return this;
                }
            }
        }
    }

    public class AreaCalculator<T> where T : INumber<T>, IMinMaxValue<T>, IEquatable<T>
    {
        private List<Rect<T>> rectangles = new();

        public AreaCalculator(List<Rect<T>> rectangles)
        {
            this.rectangles = rectangles;
        }

        public T Calculate()
        {
            T area = T.Zero;

            for (int rectangle = 0; rectangle < rectangles.Count; rectangle++)
                area += Calculate(rectangles[rectangle], T.One, rectangle + 1);

            return area;
        }

        //A depth-first search for overlaps.
        //Each consecutive overlap alternates inclusionExclusion.
        private T Calculate(Rect<T> currentRectangle, T inclusionExclusion, int nextRectangle)
        {
            T area = currentRectangle.Area() * inclusionExclusion;

            for (; nextRectangle < rectangles.Count; nextRectangle++)
            {
                Rect<T>? overlap = currentRectangle.FindOverlap(rectangles[nextRectangle]);

                if (overlap != null)
                    area += Calculate(overlap.Value, inclusionExclusion * -T.One, nextRectangle + 1);
            }

            return area;
        }
    }

    public struct Rect<T> : IEquatable<Rect<T>> where T : INumber<T>, IMinMaxValue<T>, IEquatable<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Width { get; set; }
        public T Height { get; set; }

        public Rect(T x, T y, T width, T height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public override bool Equals(object obj)
        {
            return (obj is Rect<T>) && this.Equals(((Rect<T>)obj));
        }

        public bool Equals(Rect<T> other)
        {
            return (X == other.X) && (Y == other.Y) && (Width == other.Width) && (Height == other.Height);
        }

        public static bool operator ==(Rect<T> v1, Rect<T> v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Rect<T> v1, Rect<T> v2)
        {
            return !v1.Equals(v2);
        }

        public T Area()
        {
            return Width * Height;
        }

        public Rect<T>? FindOverlap(Rect<T> other)
        {
            T left = T.Max(this.X, other.X);
            T right = T.Min(this.X + this.Width, other.X + other.Width);
            T bottom = T.Max(this.Y, other.Y);
            T top = T.Min(this.Y + this.Height, other.Y + other.Height);
            T height = top - bottom;
            T width = right - left;

            if (height > T.Zero && width > T.Zero) //If both are positive, there is overlap.
                return new Rect<T>(left, bottom, width, height);

            return null;
        }
    }
}
