namespace AdventOfCode._2024
{
    internal class Day11 : Day
    {
        Dictionary<(long, int), long> blinkCache = new();

        long BlinkCount(long stone, int numBlinks)
        {
            long count = 0;

            if (blinkCache.TryGetValue((stone, numBlinks), out count))
                return count;

            if (numBlinks == 0)
                return 1;

            if (stone == 0)
            {
                count = BlinkCount(1, numBlinks - 1);
            }
            else
            {
                string stoneStr = stone.ToString();

                if ((stoneStr.Length % 2) == 0)
                {
                    count = BlinkCount(long.Parse(stoneStr.Substring(0, stoneStr.Length / 2)), numBlinks - 1) + BlinkCount(long.Parse(stoneStr.Substring(stoneStr.Length / 2)), numBlinks - 1);
                }
                else
                {
                    count = BlinkCount(stone * 2024, numBlinks - 1);
                }
            }

            blinkCache[(stone, numBlinks)] = count;

            return count;
        }
       
        public override long Compute()
        {
            //string stoneStr = "125 17";

            string stoneStr = File.ReadAllText(DataFile);

            long count = 0;

            foreach (long stone in stoneStr.ToLongs(' '))
            {
                count += BlinkCount(stone, 75);
            }

            return count;
        }
    }
}
