namespace AdventOfCode._2021
{
    public class Day9
    {
        int width;
        int height;
        int[,] heightMap;

        void ReadInput()
        {
            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day9.txt").ToArray();

            width = lines[0].Length;
            height = lines.Length;

            heightMap = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    heightMap[x, y] = lines[y][x] - '0';
                }
            }
        }

        int GetHeight(int x, int y)
        {
            if ((x < 0) || (x >= width) || (y < 0) || (y >= height))
                return 10;

            return heightMap[x, y];
        }

        public long Compute()
        {
            ReadInput();

            int riskSum = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int height = heightMap[x, y];

                    if ((GetHeight(x - 1, y) > height) && (GetHeight(x + 1, y) > height) && (GetHeight(x, y - 1) > height) && (GetHeight(x, y + 1) > height))
                    {
                        riskSum += (height + 1);
                    }
                }
            }

            return riskSum;
        }

        int GetBasinSize(int x, int y)
        {
            if (GetHeight(x, y) < 9)
            {
                heightMap[x, y] = 9;

                return 1 + GetBasinSize(x - 1, y) + GetBasinSize(x + 1, y) + GetBasinSize(x, y - 1) + GetBasinSize(x, y + 1);
            }

            return 0;
        }

        public long Compute2()
        {
            ReadInput();

            List<int> basinSizes = new List<int>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int height = heightMap[x, y];

                    if ((GetHeight(x - 1, y) > height) && (GetHeight(x + 1, y) > height) && (GetHeight(x, y - 1) > height) && (GetHeight(x, y + 1) > height))
                    {
                        basinSizes.Add(GetBasinSize(x, y));
                    }
                }
            }

            basinSizes.Sort();

            int length = basinSizes.Count;

            return basinSizes[length - 1] * basinSizes[length - 2] * basinSizes[length - 3];
        }
    }
}
