using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public struct Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public bool IsEmpty()
        {
            return Min > Max;
        }

        public int Size()
        {
            if (IsEmpty())
                return 0;

            return (Max - Min) + 1;
        }

        public Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return Min + ".." + Max;
        }

        public Range Below(int splitVal)
        {
            return new Range(Min, splitVal - 1);
        }

        public Range Above(int splitVal)
        {
            return new Range(splitVal + 1, Max);
        }

        public Range Intersect(Range otherRange)
        {
            return new Range(Math.Max(Min, otherRange.Min), Math.Min(Max, otherRange.Max));
        }

        public Range Subtract(Range otherRange)
        {
            if (otherRange.Min > Min)
                return new Range(Min, otherRange.Min - 1);

            return new Range(otherRange.Max + 1, Max);
        }
    }
}
