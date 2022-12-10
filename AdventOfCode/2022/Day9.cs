namespace AdventOfCode._2022
{
    internal class Day9 : Day
    {
        void PrintRope(LongVec2[] segments)
        {
            SparseGrid<char> grid = new SparseGrid<char>();
            grid.DefaultValue = '.';

            for (int i = segments.Length - 1; i >= 0; i--) 
            {
                grid[(int)segments[i].X, (int)segments[i].Y] = (char)('0' + i);
            }

            grid.PrintToConsole();
        }

        int Simulate(int numSegments)
        {
            LongVec2[] segments = new LongVec2[numSegments];

            Dictionary<LongVec2, bool> tailVisited = new Dictionary<LongVec2, bool>();

            tailVisited[segments[numSegments - 1]] = true;

            foreach (string move in File.ReadLines(DataFile))
            {
                string[] moveSplit = move.Split(' ');

                for (int i = 0; i < int.Parse(moveSplit[1]); i++)
                {
                    switch (moveSplit[0][0])
                    {
                        case 'U':
                            segments[0].Y -= 1;
                            break;
                        case 'D':
                            segments[0].Y += 1;
                            break;
                        case 'L':
                            segments[0].X -= 1;
                            break;
                        case 'R':
                            segments[0].X += 1;
                            break;
                    }

                    for (int seg = 1; seg < numSegments; seg++)
                    {
                        LongVec2 diff = segments[seg - 1] - segments[seg];

                        if ((Math.Abs(diff.X) > 1) && (Math.Abs(diff.Y) > 1))
                        {
                            segments[seg].X += Math.Sign(diff.X);
                            segments[seg].Y += Math.Sign(diff.Y);
                        }
                        else
                        {
                            if (Math.Abs(diff.X) > 1)
                            {
                                segments[seg].X += Math.Sign(diff.X);

                                if (diff.Y != 0)
                                {
                                    segments[seg].Y = segments[seg - 1].Y;
                                }
                            }
                            else if (Math.Abs(diff.Y) > 1)
                            {
                                segments[seg].Y += Math.Sign(diff.Y);

                                if (diff.X != 0)
                                {
                                    segments[seg].X = segments[seg - 1].X;
                                }
                            }
                        }
                    }

                    tailVisited[segments[numSegments - 1]] = true;

                    //PrintRope(segments);
                    //Console.ReadLine();
                }
            }

            return tailVisited.Count;
        }

        public override long Compute()
        {
            return Simulate(2);
        }

        public override long Compute2()
        {
            return Simulate(10);
        }
    }
}
