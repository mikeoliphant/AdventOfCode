namespace AdventOfCode._2020
{
    public class Day3
    {
        string[] terrain;

        void ReadInput()
        {
            terrain = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day3.txt").ToArray();
        }

        long GetCollisions(int xSlope, int ySlope)
        {
            int terrainWidth = terrain[0].Length;

            int x = 0;
            int y = 0;

            int numCollisions = 0;

            do
            {
                if (terrain[y][x] == '#')
                {
                    numCollisions++;
                }

                x = (x + xSlope) % terrainWidth;
                y += ySlope;
            }
            while (y < terrain.Length);

            return numCollisions;
        }

        public long Compute()
        {
            ReadInput();

            return GetCollisions(3, 1);
        }

        public long Compute2()
        {
            ReadInput();

            return GetCollisions(1, 1) * GetCollisions(3, 1) * GetCollisions(5, 1) * GetCollisions(7, 1) * GetCollisions(1, 2);
        }
    }
}
