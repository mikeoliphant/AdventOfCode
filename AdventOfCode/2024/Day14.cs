using SkiaSharp;

namespace AdventOfCode._2024
{
    internal class Day14 : Day
    {
        List<Robot> robots = new();

        class Robot
        {
            public LongVec2 Position;
            public LongVec2 Velocity;

            public override string ToString()
            {
                return "[" + Position + "] [" + Velocity + "]";
            }
        }

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                var match = Regex.Match(line, @"p=(.+),(.+) v=(.+),(.+)");

                robots.Add(new Robot()
                {
                    Position = new LongVec2(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value)),
                    Velocity = new LongVec2(long.Parse(match.Groups[3].Value), long.Parse(match.Groups[4].Value)),
                });
            }
        }

        //const int Width = 11;
        //const int Height = 7;
        const int Width = 101;
        const int Height = 103;

        void Step()
        {
            foreach (Robot robot in robots)
            {
                robot.Position += robot.Velocity;

                robot.Position.X = ModHelper.PosMod(robot.Position.X, Width);
                robot.Position.Y = ModHelper.PosMod(robot.Position.Y, Height);
            }
        }

        (int, int, int, int) QuadrantCount()
        {
            int halfwidth = Width / 2;
            int halfHeight = Height / 2;

            int q1 = 0;
            int q2 = 0;
            int q3 = 0;
            int q4 = 0;

            foreach (Robot robot in robots)
            {
                if (robot.Position.X < halfwidth)
                {
                    if (robot.Position.Y < halfHeight)
                    {
                        q1++;
                    }
                    else if (robot.Position.Y > halfHeight)
                    {
                        q3++;
                    }
                }
                else if (robot.Position.X > halfwidth)
                {
                    if (robot.Position.Y < halfHeight)
                    {
                        q2++;
                    }
                    else if (robot.Position.Y > halfHeight)
                    {
                        q4++;
                    }
                }
            }

            return (q1, q2, q3, q4);
        }

        public override long Compute()
        {
            ReadData();

            for (int i = 0; i < 100; i++)
            {
                Step();
            }

            var count = QuadrantCount();

            long safety = count.Item1 * count.Item2 * count.Item3 * count.Item4;

            return safety;
        }

        class RobotDrawable : PlotDrawable
        {
            List<Robot> robots;

            public RobotDrawable(List<Robot> robots)
            {
                this.robots = robots;
            }

            public override void Draw(SKCanvas canvas)
            {
                foreach (Robot robot in robots)
                {
                    canvas.DrawPoint(robot.Position.X, robot.Position.Y, SKColors.Black);
                }
            }
        }

        public override long Compute2()
        {
            ReadData();

            PlotDisplay plot = new PlotDisplay(Width, Height);

            plot.AddDrawable(new RobotDrawable(robots));

            long step = 0;

            do
            {
                step++;

                Step();

                if (((step - 50) % 103) == 0)
                {
                    Console.WriteLine(step);

                    plot.ReDraw();

                    Console.ReadLine();
                }
            }
            while (true);

            return 0;
        }
    }
}
