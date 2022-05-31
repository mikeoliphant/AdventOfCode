namespace AdventOfCode._2015
{
    internal class Day14 : Day
    {
        class ReinDeer
        {
            public string Name { get; set; }
            public int Speed { get; set; }
            public int FlyTime { get; set; }
            public int RestTime { get; set; }
            public bool IsFlying { get; set; }
            public long Position { get; set; }
            public long Score { get; set; }
        }

        Dictionary<string, ReinDeer> deer = new Dictionary<string, ReinDeer>();

        void ReadInput()
        {
            foreach (string deerStr in File.ReadLines(DataFile))
            {
                var match = Regex.Match(deerStr, "(.*) can fly (.*) km/s for (.*) seconds, but then must rest for (.*) seconds.");

                if (match.Success)
                {
                    deer[match.Groups[1].Value] = new ReinDeer()
                    {
                        Name = match.Groups[1].Value,
                        Speed = int.Parse(match.Groups[2].Value),
                        FlyTime = int.Parse(match.Groups[3].Value),
                        RestTime = int.Parse(match.Groups[4].Value),
                        IsFlying = true
                    };
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public override long Compute()
        {
            ReadInput();

            long desiredSeconds = 2503;

            long maxPosition = long.MinValue;
            string maxDeer = null;

            foreach (ReinDeer d in deer.Values)
            {
                long cycles = desiredSeconds / (d.FlyTime + d.RestTime);
                long remainder = Math.Min(desiredSeconds % (d.FlyTime + d.RestTime), d.FlyTime);

                long position = ((cycles * d.FlyTime) + remainder) * d.Speed;

                if (position > maxPosition)
                {
                    maxPosition = position;
                    maxDeer = d.Name;
                }
            }

            return maxPosition;
        }

        public override long Compute2()
        {
            ReadInput();

            long desiredSeconds = 2503;

            for (int second = 1; second < desiredSeconds; second++)
            {
                foreach (ReinDeer d in deer.Values)
                {
                    long cycles = second / (d.FlyTime + d.RestTime);
                    long remainder = Math.Min(second % (d.FlyTime + d.RestTime), d.FlyTime);

                    d.Position = ((cycles * d.FlyTime) + remainder) * d.Speed;
                }

                long maxPosition = deer.Values.Max(d => d.Position);

                foreach (ReinDeer d in deer.Values.Where(d => d.Position == maxPosition))
                {
                    d.Score++;
                }
            }

            return deer.Values.Max(d => d.Score);
        }
    }
}
