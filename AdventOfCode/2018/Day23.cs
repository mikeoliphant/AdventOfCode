using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace AdventOfCode._2018
{
    internal class Day23
    {
        class Nanobot
        {
            public LongVec3 Position { get; set; }
            public long Range { get; set; }

            public override string ToString()
            {
                return Position.ToString() + " " + Range.ToString();
            }
        }

        List<Nanobot> bots = new List<Nanobot>();

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day23.txt"))
            {
                LongVec3 pos = new LongVec3(Regex.Match(line, "<(.*)>").Groups[1].Value.ToLongs(',').ToArray());

                long range = long.Parse(Regex.Match(line, "r=(.*)").Groups[1].Value);

                bots.Add(new Nanobot {  Position = pos, Range = range });
            }
        }

        public long Compute()
        {
            ReadInput();

            bots.Sort((a, b) => b.Range.CompareTo(a.Range));

            Nanobot strongest = bots[0];

            return bots.Where(b => b.Position.ManhattanDistance(strongest.Position) <= strongest.Range).Count();
        }


        struct OctBox
        {
            public LongVec3 Min { get; set; }
            public long Size { get; set;  }
            public int NumBots { get; set; }

            public bool InRange(LongVec3 pos, long range)
            {
                long dist = 0;

                if (pos.X < Min.X)
                    dist += (Min.X - pos.X);
                else if (pos.X >= (Min.X + Size))
                    dist += (pos.X - (Min.X + Size - 1));

                if (dist > range)
                    return false;

                if (pos.Y < Min.Y)
                    dist += (Min.Y - pos.Y);
                else if (pos.Y >= (Min.Y + Size))
                    dist += (pos.Y - (Min.Y + Size - 1));

                if (dist > range)
                    return false;

                if (pos.Z < Min.Z)
                    dist += (Min.Z - pos.Z);
                else if (pos.Z >= (Min.Z + Size))
                    dist += (pos.Z - (Min.Z + Size - 1));

                return (dist <= range);
            }

            public override string ToString()
            {
                return NumBots + " " + Min + " " + Size; 
            }
        }

        List<OctBox> boundsToSearch = new List<OctBox>();

        void DivideBounds(OctBox box)
        {
            long newSize = box.Size / 2;

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        OctBox newBox = new OctBox { Min = box.Min + new LongVec3(x * newSize, y * newSize, z * newSize), Size = newSize };

                        newBox.NumBots = bots.Where(b => newBox.InRange(b.Position, b.Range)).Count();

                        if (newBox.NumBots > 0)
                            boundsToSearch.Add(newBox);
                    }
                }
            }
        }

        // Needed some help from here: https://raw.githack.com/ypsu/experiments/master/aoc2018day23/vis.html

        public long Compute2()
        {
            ReadInput();

            long size = 1 << 30;

            DivideBounds(new OctBox { Min = new LongVec3(-size, -size, -size), Size = size * 2 });

            do
            {
                OctBox next = boundsToSearch.OrderByDescending(o => o.NumBots).ThenBy(o => o.Min.ManhattanDistance(LongVec3.Zero)).ThenBy(o => o.Size).First();

                if (next.Size == 1)
                {
                    return (long)(Math.Abs(next.Min.X) + Math.Abs(next.Min.Y) + Math.Abs(next.Min.Z));
                }

                boundsToSearch.Remove(next);

                DivideBounds(next);
            }
            while (true);

            throw new InvalidOperationException();
        }
    }
}

