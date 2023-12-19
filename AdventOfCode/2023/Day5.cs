namespace AdventOfCode._2023
{
    internal class Day5 : Day
    {
        struct Range<T> where T : INumber<T>
        {
            public static Range<T> Empty
            {
                get { return new Range<T> { First = T.Zero, Last = T.Zero - T.One }; } 
            }

            public static Range<T> FromLength(T first, T length)
            {
                return new Range<T> { First = first, Last = first + length - T.One };
            }

            public T First;
            public T Last;
            public T Length
            {
                get { return Last - First + T.One; }
            }

            public Range(T first, T last)
            {
                this.First = first;
                this.Last = last;
            }

            public bool Contains(T value)
            {
                return (value >= First) && (value <= Last);
            }

            public bool TryIntersect(Range<T> other, out Range<T> intersect)
            {
                intersect = Empty;

                if ((Last < other.First) || (First > other.Last))
                    return false;

                intersect = new Range<T> { First = T.Max(First, other.First), Last = T.Min(Last, other.Last) };

                return true;
            }

            public override string ToString()
            {
                return First + ".." + Last + "[" + Length + "]";
            }
        }

        class Map
        {
            public string To { get; set; }
            public Range<long> DestRange { get; set; }
            public Range<long> SourceRange { get; set; }
        }

        Dictionary<string, List<Map>> mapDict = new();

        long GetLocation(string type, long number)
        {
            if (type == "location")
                return number;

            foreach (Map map in mapDict[type])
            {
                if (map.SourceRange.Contains(number))
                {
                    return GetLocation(map.To, map.DestRange.First + (number - map.SourceRange.First));
                }
            }

            return GetLocation(mapDict[type][0].To, number);
        }

        public override long Compute()
        {
            var sections = File.ReadAllText(DataFile).SplitParagraphs();

            var seeds = sections[0].Split(':')[1].Trim().ToLongs(' ');

            foreach (string section in sections.Skip(1))
            {
                var lines = section.SplitLines();

                Match match = Regex.Match(lines[0], @"(.*)-to-(.*) map:");

                List<Map> map = new();

                mapDict[match.Groups[1].Value] = map;

                foreach (string line in lines.Skip(1))
                {
                    string[] range = line.Split(" ");

                    long length = long.Parse(range[2]);

                    map.Add(new Map { To = match.Groups[2].Value, DestRange = Range<long>.FromLength(long.Parse(range[0]), length), SourceRange = Range<long>.FromLength(long.Parse(range[1]), length) });
                }
            }

            List<long> locations = new List<long>();

            foreach (long seed in seeds)
            {
                locations.Add(GetLocation("seed", seed));
            }

            long minLocation = locations.Min();

            return minLocation;
        }

        IEnumerable<long> GetLocations(string type, Range<long> range)
        {
            if (type == "location")
                yield return range.First;
            else
            {
                bool haveIntersect = false;

                foreach (Map map in mapDict[type])
                {
                    Range<long> intersect;

                    if (map.SourceRange.TryIntersect(range, out intersect))
                    {
                        if (range.First < intersect.First)  // Have values before intersect
                        {
                            yield return GetLocations(type, new Range<long> { First = range.First, Last = intersect.First - 1 }).Min();
                        }

                        yield return GetLocations(map.To, new Range<long> { First = map.DestRange.First + (intersect.First - map.SourceRange.First), Last = map.DestRange.Last + (intersect.Last - map.SourceRange.Last) }).Min();

                        if (range.Last > intersect.Last)    // Have values after intersect
                        {
                            yield return GetLocations(type, new Range<long> { First = intersect.Last + 1, Last = range.Last }).Min();
                        }

                        haveIntersect = true;

                        break;
                    }
                }

                if (!haveIntersect)
                    yield return GetLocations(mapDict[type][0].To, range).Min();
            }
        }


        public override long Compute2()
        {
            var sections = File.ReadAllText(DataFile).SplitParagraphs();

            var seeds = sections[0].Split(':')[1].Trim().ToLongs(' ').ToArray();

            foreach (string section in sections.Skip(1))
            {
                var lines = section.SplitLines();

                Match match = Regex.Match(lines[0], @"(.*)-to-(.*) map:");

                List<Map> map = new();

                mapDict[match.Groups[1].Value] = map;

                foreach (string line in lines.Skip(1))
                {
                    string[] range = line.Split(" ");

                    long length = long.Parse(range[2]);

                    map.Add(new Map { To = match.Groups[2].Value, DestRange = Range<long>.FromLength(long.Parse(range[0]), length), SourceRange = Range<long>.FromLength(long.Parse(range[1]), length) });
                }
            }

            List<long> locations = new List<long>();

            for (int seed = 0; seed < seeds.Length; seed += 2)
            {
                locations.Add(GetLocations("seed", Range<long>.FromLength(seeds[seed], seeds[seed + 1])).Min());
            }

            long minLocation = locations.Min();

            return minLocation;
        }
    }
}
