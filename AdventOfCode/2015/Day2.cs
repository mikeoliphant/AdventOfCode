namespace AdventOfCode._2015
{
    internal class Day2
    {
        IEnumerable<int> GetSideAreas(int[] dimensions)
        {
            for (int side1 = 0; side1 < dimensions.Length; side1++)
            {
                for (int side2 = side1 + 1; side2 < dimensions.Length; side2++)
                {
                    yield return dimensions[side1] * dimensions[side2];
                }
            }
        }

        public long Compute()
        {
            long paperAmount = 0;

            foreach (string present in File.ReadLines(@"C:\Code\AdventOfCode\Input\2015\Day2.txt"))
            {
                int[] dimensions = present.ToInts('x').ToArray();

                var sideAreas = GetSideAreas(dimensions);

                paperAmount += sideAreas.Sum() * 2;

                paperAmount += sideAreas.Min();
            }

            return paperAmount;
        }

        public long Compute2()
        {
            long ribbonAmount = 0;

            foreach (string present in File.ReadLines(@"C:\Code\AdventOfCode\Input\2015\Day2.txt"))
            {
                var sides = present.ToInts('x').OrderBy(s => s).ToArray();

                ribbonAmount += (sides[0] + sides[1]) * 2;

                ribbonAmount += sides[0] * sides[1] * sides[2];
            }

            return ribbonAmount;
        }
    }
}
