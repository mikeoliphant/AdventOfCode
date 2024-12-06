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
}
