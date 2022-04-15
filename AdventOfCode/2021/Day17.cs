namespace AdventOfCode._2021
{
    internal class Day17
    {
        Point areaStart;
        Point areaEnd;

        int areaMinX;
        int areaMaxX;
        int areaTopY;
        int areaBottomY;


        void ReadInput()
        {
            //areaStart = new Point(20, -10);
            //areaEnd = new Point(30, -5);
            areaStart = new Point(14, -267);
            areaEnd = new Point(50, -225);

            areaMinX = Math.Min(areaStart.X, areaEnd.X);
            areaMaxX = Math.Max(areaStart.X, areaEnd.X);
            areaTopY = Math.Max(areaStart.Y, areaEnd.Y);
            areaBottomY = Math.Min(areaStart.Y, areaEnd.Y);
        }

        int GetTerminal(int x)
        {
            return (x * (x + 1)) / 2;
        }

        bool GetMaxHeight(int dx, int dy, out int maxHeight)
        {
            int x = 0;
            int y = 0;

            maxHeight = 0;
            bool isValid = false;

            do
            {
                maxHeight = Math.Max(maxHeight, y);

                if ((x >= areaMinX) && (y <= areaTopY))
                {
                    isValid = true;
                }

                x += dx;
                y += dy;

                if (dx > 0)
                    dx -= 1;

                dy -= 1;
            }
            while ((x <= areaMaxX) && (y >= areaBottomY));

            return isValid;
        }

        List<Point> allVelocities = new List<Point>();

        void BruteForce()
        {
            int maxDX = Math.Max(areaStart.X, areaEnd.X);
            int maxDY = Math.Max(Math.Abs(areaStart.Y), Math.Abs(areaEnd.Y));

            int maxHeight = 0;

            for (int dx = 0; dx <= maxDX; dx++)
            {
                for (int dy = -maxDY; dy <= maxDY; dy++)
                {
                    int height;

                    if (GetMaxHeight(dx, dy, out height))
                    {
                        maxHeight = Math.Max(maxHeight, height);

                        allVelocities.Add(new Point(dx, dy));
                    }
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            BruteForce();

            return 0;
        }
    }
}
