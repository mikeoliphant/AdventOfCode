using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public struct Range
    {
        public long Min { get; set; }
        public long Max { get; set; }

        public bool IsEmpty()
        {
            return Min > Max;
        }

        public long Size()
        {
            if (IsEmpty())
                return 0;

            return (Max - Min) + 1;
        }

        public static Range FromString(string str)
        {
            return FromString(str, "-");
        }

        public static Range FromString(string str, string split)
        {
            var vals = str.Split(split);

            return new Range(long.Parse(vals[0]), long.Parse(vals[1]));
        }

        public Range(long min, long max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return Min + ".." + Max;
        }

        public Range Below(long splitVal)
        {
            return new Range(Min, splitVal - 1);
        }

        public Range Above(long splitVal)
        {
            return new Range(splitVal + 1, Max);
        }

        public bool Intersects(Range otherRange)
        {
            return otherRange.Contains(Min) || otherRange.Contains(Max);

        }

        public Range Intersect(Range otherRange)
        {
            return new Range(Math.Max(Min, otherRange.Min), Math.Min(Max, otherRange.Max));
        }

        // Assumes you have alread check intersect
        public Range Union(Range otherRange)
        {
            return new Range(Math.Min(Min, otherRange.Min), Math.Max(Max, otherRange.Max));
        }

        public Range Subtract(Range otherRange)
        {
            if (otherRange.Min > Min)
                return new Range(Min, otherRange.Min - 1);

            return new Range(otherRange.Max + 1, Max);
        }

        public IEnumerable<Range> GetDisjointFrom(Range otherRange)
        {
            if ((Max < otherRange.Min) || (Min > otherRange.Max))
            {
                yield return this;
            }
            else
            {
                if (Min < otherRange.Min)
                {
                    yield return new Range(Min, otherRange.Min - 1);
                }

                if (Max > otherRange.Max)
                {
                    yield return new Range(otherRange.Max + 1, Max);
                }
            }
        }

        public bool Contains(long value)
        {
            return value >= Min && value <= Max;
        }
    }
}
