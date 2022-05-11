namespace AdventOfCode
{
    public static class PrimeAlign
    {
        public static long Align(IEnumerable<(long Modulo, long StartOffset, long DesiredOffset)> toAlign)
        {
            int numIts = 0;

            var sorted = toAlign.OrderBy(a => a.Modulo).ToArray();

            long step = 0;
            long stepSize = 1;

            foreach (var current in sorted)
            {
                long offset = (current.StartOffset + step) % current.Modulo;

                while (offset != current.DesiredOffset)
                {
                    offset = (offset + stepSize) % current.Modulo;
                    step += stepSize;

                    numIts++;
                }

                stepSize *= current.Modulo;
            }

            return step;
        }
    }
}
