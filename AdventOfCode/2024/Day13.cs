using Microsoft.VisualBasic.Logging;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace AdventOfCode._2024
{
    internal class Day13 : Day
    {
        List<ClawMachine> machines = new();

        class ClawMachine
        {
            public LongVec2 A;
            public LongVec2 B;
            public LongVec2 Prize;

            public override string ToString()
            {
                return "A: [" + A + "] B: [" + B + "] Prize: [" + Prize + "]";
            }
        }

        LongVec2 ReadVec(string line)
        {
            var match = Regex.Match(line, "X.(.*), Y.(.*)");

            return new LongVec2(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
        }

        void ReadData()
        {
            foreach (string[] lines in File.ReadAllText(DataFile).SplitParagraphs().Select(p => p.SplitLines()))
            {
                ClawMachine machine = new ClawMachine()
                {
                    A = ReadVec(lines[0]),
                    B = ReadVec(lines[1]),
                    Prize = ReadVec(lines[2])
                };

                machines.Add(machine);
            }
        }

        long GetMinTokens(ClawMachine machine)
        {
            long min = 0;

            for (int a = 0; a < 100; a++)
            {
                for (int b = 0; b < 100; b++)
                {
                    if ((a * machine.A) + (b * machine.B) == machine.Prize)
                    {
                        long cost = (a * 3) + b;

                        if ((min == 0) || (cost < min))
                            min = cost; ;
                    }
                }
            }

            return min;
        }

        public override long Compute()
        {
            ReadData();

            long tokens = 0;

            foreach (ClawMachine machine in machines)
            {
                tokens += GetMinTokens(machine);
            }

            return tokens;
        }
        
        long? Divide(LongVec2 vec, LongVec2 div)
        {
            if ((vec.X % div.X == 0) && (vec.Y % div.Y == 0))
                return (vec.X / div.X);

            return null;
        }

        long GetMinTokens2(ClawMachine machine)
        {
            long min = 0;

            for (long a = 0; a < 100; a++)
            {
                LongVec2 bVec = machine.Prize - (a * machine.A);// / machine.B;

                long? b = Divide(bVec, machine.B);

                if (b != null)
                {
                    if ((a * machine.A) + (b * machine.B) == machine.Prize)
                    {
                        long cost = (a * 3) + b.Value;

                        if (min != 0)
                        {

                        }

                        if ((min == 0) || (cost < min))
                            min = cost; ;
                    }
                }
            }

            return min;
        }

        long GetMinTokens3(ClawMachine machine)
        {
            Ray2D<long> aRay = new (LongVec2.Zero, machine.A);
            Ray2D<long> bRay = new (machine.Prize, machine.Prize - machine.B);

            var intersect = Ray2D<long>.Intersect(aRay, bRay);

            if ((intersect.Point.X % intersect.Det) == 0)
            {
                long ax = (long)(intersect.Point.X / intersect.Det);
                long bx = machine.Prize.X - ax;

                long a = ax / machine.A.X;
                long b = bx / machine.B.X;

                if (((a * machine.A) + (b * machine.B)) != machine.Prize)
                {
                    return 0;
                }

                return (a * 3) + b;
            }

            return 0;
        }

        public override long Compute2()
        {
            ReadData();

            long tokens = 0;

            foreach (ClawMachine machine in machines)
            {
                machine.Prize += new LongVec2(10000000000000, 10000000000000);

                tokens += GetMinTokens3(machine);
            }

            return tokens;
        }
    }
}
