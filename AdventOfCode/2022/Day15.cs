namespace AdventOfCode._2022
{
    internal class Day15 : Day
    {
        List<Sensor> sensors = new List<Sensor>();

        class Sensor
        {
            public LongVec2 Position { get; set; }
            public LongVec2 ClosestBeacon { get; set; }

            public long Range()
            {
                return Position.ManhattanDistance(ClosestBeacon);
            }
        }

        void ReadInput()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                var match = Regex.Match(line, "Sensor at x=(.*), y=(.*): closest beacon is at x=(.*), y=(.*)");

                sensors.Add(new Sensor { Position = new LongVec2(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value)), ClosestBeacon = new LongVec2(long.Parse(match.Groups[3].Value), long.Parse(match.Groups[4].Value)) });
            }

            foreach (Sensor sensor in sensors)
            {
                long maxDist = sensor.Range();
            }
        }

        long InvalidPositionsForRow(long row)
        {
            Dictionary<long, bool> visibleDict = new Dictionary<long, bool>();

            foreach (Sensor sensor in sensors)
            {
                long maxDist = sensor.Range();

                long dy = Math.Abs(sensor.Position.Y - row);

                if (dy > maxDist)
                    continue;

                long minX = sensor.Position.X - (maxDist - dy);
                long maxX = sensor.Position.X + (maxDist - dy);

                for (long x = minX; x <= maxX; x++)
                {
                    visibleDict[x] = true;
                }
            }

            // Remove spots that contain beacons
            foreach (Sensor sensor in sensors)
            {
                if ((sensor.ClosestBeacon.Y == row) && visibleDict.ContainsKey(sensor.ClosestBeacon.X))
                    visibleDict.Remove(sensor.ClosestBeacon.X);
            }

            return visibleDict.Count;
        }

        public override long Compute()
        {
            ReadInput();

            return InvalidPositionsForRow(2000000);
        }

        public override long Compute2()
        {
            ReadInput();

            long maxVal = 4000000;

            Dictionary<LongVec2, bool> candidates = new Dictionary<LongVec2, bool>();

            foreach (Sensor sensor in sensors)
            {
                long dist = sensor.Range() + 1;

                for (long x = sensor.Position.X - dist; x <= sensor.Position.X + dist; x++)
                {
                    long dy = dist - Math.Abs(sensor.Position.X - x);

                    candidates[new LongVec2(x, sensor.Position.Y - dy)] = true;
                    candidates[new LongVec2(x, sensor.Position.Y + dy)] = true;
                }
            }

            foreach (LongVec2 candidate in candidates.Keys)
            {
                if ((candidate.X >= 0) && (candidate.X <= maxVal) && (candidate.Y >=0) && (candidate.Y <= maxVal))
                {
                    bool unreachable = true;

                    foreach (Sensor sensor in sensors)
                    {
                        if (sensor.Position.ManhattanDistance(candidate) <= sensor.Range())
                        {
                            unreachable = false;

                            break;
                        }
                    }

                    if (unreachable)
                    {
                        return (candidate.X * maxVal) + candidate.Y;
                    }
                }
            }

            throw new Exception();
        }
    }
}
