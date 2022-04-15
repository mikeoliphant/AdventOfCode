namespace AdventOfCode._2021
{
    class PodPositions : Grid<Point>, IEquatable<PodPositions>
    {
        public static int NumPods { get; set; }

        public PodPositions()
            : base(4, NumPods)
        {

        }

        public PodPositions(PodPositions src)
            : base(src)
        {            
        }

        public bool Equals(PodPositions other)
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (Point p in this.GetColValues(i))
                {
                    if (!other.GetColValues(i).Contains(p))
                        return false;
                }
            }

            return true;
        }
    }

    internal class Day23
    {
        Grid<char> startGrid;
        Grid<char> template;
        Grid<char> blank;
        List<Point> hallwayPoints = new List<Point>();
        PodPositions startPoints;
        PodPositions destPoints;
        float[] cost = new float[] { 1, 10, 100, 1000 };
        bool doUnfold = false;

        Grid<char> fold = new Grid<char>().CreateDataFromRows(new string[]
            {
               "  #D#C#B#A#  ",
               "  #D#B#A#C#  "
            });

        Grid<char> templateFold = new Grid<char>().CreateDataFromRows(new string[]
            {
                "  #A#B#C#D#  ",
                "  #A#B#C#D#  "
            });

        Grid<char> blankFold = new Grid<char>().CreateDataFromRows(new string[]
            {
                "  #.#.#.#.#  ",
                "  #.#.#.#.#  "
            });

        Grid<char> Unfold(Grid<char> grid, Grid<char> foldData)
        {
            Grid<char> unfolded = new Grid<char>(grid.Width, grid.Height + 2);

            Grid<char>.Copy(grid, unfolded, 0, 0, grid.Width, 3, 0, 0);
            Grid<char>.Copy(foldData, unfolded, 0, 0, grid.Width, 2, 0, 3);
            Grid<char>.Copy(grid, unfolded, 0, 3, grid.Width, grid.Height - 3, 0, 5);

            return unfolded;
        }

        void ReadInput()
        {
            startPoints = new PodPositions();
            destPoints = new PodPositions();

            startGrid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day23.txt"));
            template = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day23Template.txt"));
            blank = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day23Blank.txt"));

            if (doUnfold)
            {
                startGrid = Unfold(startGrid, fold);
                template = Unfold(template, templateFold);
                blank = Unfold(blank, blankFold);
            }

            int[] pos = new int[4];

            for (int y = 0; y < startGrid.Height; y++)
            {
                for (int x = 0; x < startGrid.Width; x++)
                {
                    char c = startGrid[x, y];

                    if (((c - 'A') >= 0) && ((c - 'A') < 4))
                    {
                        int type = (c - 'A');

                        startPoints[type, pos[type]] = new Point(x, y);

                        pos[type]++;
                    }
                }
            }

            Array.Clear(pos);

            for (int y = 0; y < template.Height; y++)
            {
                for (int x = 0; x < template.Width; x++)
                {
                    char c = template[x, y];

                    if (c == 'H')
                    {
                        hallwayPoints.Add(new Point(x, y));
                    }
                    else if (((c - 'A') >= 0) && ((c - 'A') < 4))
                    {
                        int type = (c - 'A');

                        destPoints[type, pos[type]] = new Point(x, y);

                        pos[type]++;
                    }
                }
            }
        }

        bool IsHallway(Point p)
        {
            return p.Y == 1;
        }

        bool IsHome(int podType, int pod, Point p, bool alreadyThere, PodPositions currentState)
        {
            if (!alreadyThere)  // If we're already there, no need to check for blockage, just whether we're really done
            {            
                for (int y = 2; y <= p.Y; y++)
                {
                    if (IsOccupied(new Point(p.X, y), currentState))    // Someone is in the spot or blocking it
                        return false;
                }
            }

            for (int y = p.Y + 1; y < (PodPositions.NumPods + 2); y++)
            {
                if (!currentState.GetColValues(podType).Contains(new Point(p.X, y)))  // Only go home if home spots below are occupied with buddies
                    return false;
            }

            return true;
        }

        bool IsOccupied(Point p, Grid<Point> currentState)
        {
            foreach (Point podPos in currentState.GetAllValues())
            {
                if (p == podPos) // Someone else is there
                {
                    return true;
                }
            }

            return false;
        }

        bool CanReach(Point start, Point dest, bool hallStart, PodPositions currentState)
        {
            int xDir = Math.Sign(dest.X - start.X);

            do
            {
                if (hallStart)
                {
                    if (start.X != dest.X)
                    {
                        start.X += xDir;
                    }
                    else
                    {
                        start.Y++;
                    }
                }
                else
                {
                    if (start.Y != dest.Y)
                    {
                        start.Y--;
                    }
                    else
                    {
                        start.X += xDir;
                    }
                }

                if (IsOccupied(start, currentState))
                    return false;
            }
            while (start != dest);

            return true;
        }

        int ManhattanDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        List<KeyValuePair<PodPositions, float>> GetValidMoves(PodPositions currentState)
        {
            //Console.WriteLine("Start State");
            //Console.WriteLine("===========");
            //Console.WriteLine();

            //PrintState(currentState);

            //Console.WriteLine();
            //Console.WriteLine("Moves:");
            //Console.WriteLine();

            List<KeyValuePair<PodPositions, float>> moves = new List<KeyValuePair<PodPositions, float>>();

            foreach (bool passOne in new bool[] { true, false })
            {
                for (int podType = 0; podType < 4; podType++)
                {
                    for (int pod = 0; pod < PodPositions.NumPods; pod++)
                    {
                        Point p = currentState[podType, pod];

                        if (destPoints.GetColValues(podType).Contains(p) && IsHome(podType, pod, p, alreadyThere: true, currentState))     // If we're home, don't move again
                        {
                            continue;
                        }

                        if (IsHallway(p))
                        {
                            if (!passOne)
                                continue;

                            foreach (Point dest in destPoints.GetColValues(podType))
                            {
                                if (IsHome(podType, pod, dest, alreadyThere: false, currentState))
                                {
                                    if (!IsOccupied(dest, currentState) && CanReach(p, dest, hallStart: true, currentState))
                                    {
                                        PodPositions newState = new PodPositions(currentState);
                                        newState[podType, pod] = dest;

                                        // If someone can go home, that's always the best move
                                        return new List<KeyValuePair<PodPositions, float>> { new KeyValuePair<PodPositions, float>(newState, ManhattanDistance(p, dest) * cost[podType]) };
                                    }
                                }
                            }
                        }
                        else  // We're in a side room
                        {
                            if (passOne)
                                continue;

                            if ((p.Y > 2) && IsOccupied(new Point(p.X, p.Y - 1), currentState)) // Quick out if someone is above us
                                continue;

                            foreach (Point hall in hallwayPoints)
                            {
                                if (!IsOccupied(hall, currentState) && CanReach(p, hall, hallStart: false, currentState))
                                {
                                    PodPositions newState = new PodPositions(currentState);
                                    newState[podType, pod] = hall;

                                    moves.Add(new KeyValuePair<PodPositions, float>(newState, ManhattanDistance(p, hall) * cost[podType]));
                                }
                            }
                        }
                    }
                }
            }

            //foreach (var move in moves)
            //{
            //    PrintState(move.Key);
            //}

            return moves;
        }

        void PrintState(Grid<Point> state)
        {
            Grid<char> grid = new Grid<char>(blank);

            for (int podType = 0; podType < 4; podType++)
            {
                for (int pod = 0; pod < PodPositions.NumPods; pod++)
                {
                    Point p = state[podType, pod];

                    grid[p.X, p.Y] = (char)('A' + podType);
                }
            }

            grid.PrintToConsole();
        }

        public long Compute()
        {
            PodPositions.NumPods = 2;

            ReadInput();

            DijkstraSearch<PodPositions> search = new DijkstraSearch<PodPositions>(GetValidMoves);

            float cost = 0;
            List<PodPositions> path;

            if (!search.GetShortestPath(startPoints, destPoints, out path, out cost))
            {
                throw new Exception();
            }

            return (long)cost;
        }

        public long Compute2()
        {
            PodPositions.NumPods = 4;
            doUnfold = true;

            ReadInput();

            DijkstraSearch<PodPositions> search = new DijkstraSearch<PodPositions>(GetValidMoves);

            //foreach (var moves in GetValidMoves(startPoints))
            //{
            //    PrintState(moves.Key);
            //}

            float cost;
            List<PodPositions> path;

            if (search.GetShortestPath(startPoints, destPoints, out path, out cost))
            {

            }

            return (long)cost;
        }
    }
}
