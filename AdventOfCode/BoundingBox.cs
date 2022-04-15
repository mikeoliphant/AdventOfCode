namespace AdventOfCode
{
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        public long[] Max { get; set; }
        public long[] Min { get; set; }

        public BoundingBox()
        {
            Min = new long[] { 0, 0, 0 };
            Max = new long[] { 0, 0, 0 };
        }

        public BoundingBox(long[] min, long[] max)
        {
            this.Min = min;
            this.Max = max;
        }

        public static bool GetIntersection(BoundingBox box1, BoundingBox box2, out BoundingBox intersection)
        {
            intersection = new BoundingBox();

            for (int i = 0; i < 3; i++)
            {
                if ((box1.Min[i] > box2.Max[i]) || (box2.Min[i] > box1.Max[i]))
                    return false;
            }

            for (int i = 0; i < 3; i++)
            {
                intersection.Min[i] = Math.Max(box1.Min[i], box2.Min[i]);
                intersection.Max[i] = Math.Min(box1.Max[i], box2.Max[i]);
            }

            return true;
        }

        public long GetVolume()
        {
            long volume = 1;

            for (int i = 0; i < 3; i++)
            {
                volume *= (Max[i] - Min[i]) + 1;
            }

            return volume;
        }

        public List<BoundingBox> Slice(BoundingBox otherBox)
        {
            List<BoundingBox> slices = new List<BoundingBox> { this };

            for (int i = 0; i < 3; i++)
            {
                List<BoundingBox> newSlices = new List<BoundingBox>();

                foreach (BoundingBox box in slices)
                {
                    newSlices.AddRange(box.Slice(otherBox, i));
                }

                slices = newSlices;
            }

            return slices;
        }

        public List<BoundingBox> Slice(BoundingBox otherBox, int axis)
        {
            List<BoundingBox> slices = new List<BoundingBox>();

            bool haveSlice = false;

            if (otherBox.Min[axis] > this.Min[axis])
            {
                BoundingBox slice1 = new BoundingBox();
                BoundingBox slice2 = new BoundingBox();

                for (int i = 0; i < 3; i++)
                {
                    if (i == axis)
                    {
                        slice1.Min[i] = this.Min[i];
                        slice1.Max[i] = otherBox.Min[i] - 1;

                        slice2.Min[i] = otherBox.Min[i];
                        slice2.Max[i] = otherBox.Max[i];
                    }
                    else
                    {
                        slice1.Min[i] = slice2.Min[i] = this.Min[i];
                        slice1.Max[i] = slice2.Max[i] = this.Max[i];
                    }
                }

                slices.Add(slice1);
                slices.Add(slice2);

                haveSlice = true;
            }

            if (otherBox.Max[axis] < this.Max[axis])
            {
                BoundingBox slice1 = new BoundingBox();
                BoundingBox slice2 = new BoundingBox();

                for (int i = 0; i < 3; i++)
                {
                    if (i == axis)
                    {
                        if (!haveSlice)
                        {
                            slice1.Min[i] = otherBox.Min[i];
                            slice1.Max[i] = otherBox.Max[i];
                        }

                        slice2.Min[i] = otherBox.Max[i] + 1;
                        slice2.Max[i] = this.Max[i];
                    }
                    else
                    {
                        if (!haveSlice)
                        {
                            slice1.Min[i] = this.Min[i];
                            slice1.Max[i] = this.Max[i];
                        }

                        slice2.Min[i] = this.Min[i];
                        slice2.Max[i] = this.Max[i];
                    }
                }

                if (!haveSlice)
                {
                    slices.Add(slice1);
                }

                slices.Add(slice2);

                haveSlice = true;
            }

            if (!haveSlice)
            {
                slices.Add(this);
            }

            return slices;
        }

        public bool Contains(BoundingBox otherBox)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((otherBox.Min[i] < Min[i]) || (otherBox.Max[i]) > Max[i])
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return "[" + String.Join(",", Min) + "]-[" + String.Join(",", Max) + "]";
        }

        public bool Equals(BoundingBox other)
        {
            return Min.SequenceEqual(other.Min) && Max.SequenceEqual(other.Max);
        }
    }
}
