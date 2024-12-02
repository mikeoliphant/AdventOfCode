using System.IO.Compression;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace AdventOfCode._2023
{
    internal class Day22 : Day
    {
        static Dictionary<LongVec3, Brick> brickDict;
        List<Brick> bricks;

        class Brick
        {
            public LongVec3 Start { get; set; }
            public LongVec3 End { get; set; }
            public LongVec3 Delta
            {
                get { return End - Start; }
            }

            public override string ToString()
            {
                return Start + ".." + End;
            }

            public void AddToDict()
            {
                foreach (LongVec3 pos in GetPoints())
                {
                    brickDict[pos] = this;
                }
            }

            public void RemoveFromDict()
            {
                foreach (LongVec3 pos in GetPoints())
                {
                    brickDict.Remove(pos);
                }
            }

            public bool Fall()
            {
                if (GetSupports().Count() == 0)
                {
                    RemoveFromDict();

                    do
                    {
                        Start = new LongVec3(Start.X, Start.Y, Start.Z - 1);
                        End = new LongVec3(End.X, End.Y, End.Z - 1);
                    }
                    while (GetSupports().Count() == 0);

                    AddToDict();

                    return true;
                }

                return false;
            }

            public IEnumerable<LongVec3> GetPoints()
            {
                LongVec3 delta = Delta;

                long mag = delta.X + delta.Y + delta.Z;
                if (mag != 0)
                    delta.Divide(mag);

                LongVec3 pos = Start;

                yield return pos;

                while (pos != End)
                {
                    pos += delta;

                    yield return pos;
                }
            }

            public IEnumerable<Brick> GetSupports()
            {
                if ((Start.Z == 1) || (End.Z == 1))
                    yield return null;

                foreach (LongVec3 pos in GetPoints())
                {
                    LongVec3 below = pos;
                    below.Z--;

                    if (brickDict.ContainsKey(below))
                    {
                        Brick belowBrick = brickDict[below];

                        if (belowBrick != this)
                            yield return belowBrick;
                    }
                }
            }
        }

        void ReadData()
        {
            brickDict = new Dictionary<LongVec3, Brick>();
            bricks = new List<Brick>();

            foreach (string line in File.ReadLines(DataFile))
            {
                string[] startEnd = line.Split('~');

                Brick brick = new Brick()
                {
                    Start = new LongVec3(startEnd[0].ToLongs(',').ToArray()),
                    End = new LongVec3(startEnd[1].ToLongs(',').ToArray())
                };

                bricks.Add(brick);

                brick.AddToDict();
            }
        }

        int DoFall()
        {
            HashSet<Brick> fallDict = new();

            bool fall = false;

            do
            {
                fall = false;

                foreach (Brick brick in bricks)
                {
                    if (brick.Fall())
                    {
                        fallDict.Add(brick);

                        fall = true;
                    }
                }
            }
            while (fall);

            return fallDict.Count;
        }

        HashSet<Brick> GetNeeded()
        {
            HashSet<Brick> need = new();

            foreach (Brick brick in bricks)
            {
                var supports = brick.GetSupports().Distinct();

                if (supports.Count() == 1)
                {
                    foreach (Brick support in supports)
                    {
                        if (support != null)
                        {
                            need.Add(support);
                        }
                    }
                }
            }

            return need;
        }

        void SanityCheck()
        {
            foreach (Brick brick in bricks)
            {
                foreach (LongVec3 pos in brick.GetPoints())
                {
                    if (brickDict[pos] != brick)
                        throw new Exception();
                }
            }
        }

        public override long Compute()
        {
            ReadData();

            DoFall();

            var need = GetNeeded();

            int numSmashable = bricks.Count - need.Count;

            return numSmashable;
        }

        public override long Compute2()
        {
            ReadData();

            DoFall();

            var need = GetNeeded();

            var backup = new Dictionary<LongVec3, Brick>(brickDict);
            var brickBackup = new List<Brick>(bricks);

            long totFall = 0;

            SanityCheck();

            // Ugly and inefficient, but it works fast enough
            foreach (Brick brick in need)
            {
                ReadData();

                DoFall();

                // Make sure we use the new ref
                Brick b = brickDict[brick.Start];

                b.RemoveFromDict();

                bricks.Remove(b);

                long numFall = DoFall();

                totFall += numFall;
            }

            return totFall;
        }
    }
}
