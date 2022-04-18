namespace AdventOfCode._2018
{
    internal class Day15
    {
        class CostComparer : IComparer<((int X, int Y) Node, float Cost)>
        {
            public int Compare(((int X, int Y) Node, float Cost) x, ((int X, int Y) Node, float Cost) y)
            {
                if (x.Cost == y.Cost)
                {
                    return ReadPos(x.Node.X, x.Node.Y).CompareTo(ReadPos(y.Node.X, y.Node.Y));
                }

                return x.Cost.CompareTo(y.Cost);
            }
        }

        class Dude
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int HP { get; set; }
            public int AttackPower { get; set; }

            public virtual char Type
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public virtual char Enemy
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int ReadPos
            {
                get { return ReadPos(X, Y); }
            }

            public override string ToString()
            {
                return X + "," + Y + " (" + HP + ")";
            }
        }

        class Goblin : Dude
        {
            public override char Type => 'G';
            public override char Enemy => 'E';
        }

        class Elf : Dude
        {
            public override char Type => 'E';
            public override char Enemy => 'G';
        }

        Grid<char> grid;
        List<Dude> dudes;
        Dictionary<(int X, int Y), Dude> dudesOnGrid;
        bool done = false;
        int goblinAttackPower = 3;
        int elfAttackPower = 3;
        bool keepElvesAlive = false;

        public static int ReadPos(int x, int y)
        {
            return y * 1000 + x;
        }

        public void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day15.txt"));

            dudes = new List<Dude>();
            dudesOnGrid = new Dictionary<(int X, int Y), Dude>();

            foreach (var pos in grid.GetAll())
            {
                Dude newDude = null;

                if (grid[pos.X, pos.Y] == 'G')
                {
                    newDude = new Goblin { X = pos.X, Y = pos.Y, HP = 200, AttackPower = goblinAttackPower };
                }
                else if (grid[pos.X, pos.Y] == 'E')
                {
                    newDude = new Elf { X = pos.X, Y = pos.Y, HP = 200, AttackPower = elfAttackPower };
                }

                if (newDude != null)
                {
                    dudes.Add(newDude);

                    dudesOnGrid[(newDude.X, newDude.Y)] = newDude;
                }
            }
        }

        IEnumerable<KeyValuePair<(int X, int Y), float>> GetUnblockedNeighborCost((int X, int Y) position)
        {
            foreach (var pos in GetNeighbors(position))
            {
                if (grid[pos.X, pos.Y] == '.')
                    yield return new KeyValuePair<(int X, int Y), float>(pos, 1);
            }
        }

        IEnumerable<(int X, int Y)> GetUnblockedNeighbors((int X, int Y) position)
        {
            foreach (var pos in GetNeighbors(position))
            {
                if (grid[pos.X, pos.Y] == '.')
                    yield return (pos.X, pos.Y);
            }
        }

        IEnumerable<(int X, int Y)> GetNeighbors((int X, int Y) position)
        {
            // North
            yield return (position.X, position.Y - 1);

            // West
            yield return (position.X - 1, position.Y);

            // East
            yield return (position.X + 1, position.Y);

            // South
            yield return (position.X, position.Y + 1);
        }


        public bool GetShortestPath(int startX, int startY, int endX, int endY, out List<(int X, int Y)> path, out float cost)
        {
            DijkstraSearch<ValueTuple<int, int>> search = new DijkstraSearch<ValueTuple<int, int>>(GetUnblockedNeighborCost);

            return search.GetShortestPath((startX, startY), (endX, endY), out path, out cost);

            //DepthFirstSearch<(int X, int Y)> search = new DepthFirstSearch<(int X, int Y)>(GetUnblockedNeighbors);

            //return search.GetShortestPath((startX, startY), (endX, endY), out path, out cost);
        }

        // Check if one side has won
        bool Finished()
        {
            foreach (Dude dude in dudes)
            {
                if (dude.HP <= 0)
                    continue;

                foreach (Dude dude2 in dudes)
                {
                    if (dude2.HP <= 0)
                        continue;

                    if (dude.GetType() != dude2.GetType())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        bool DoAttack(Dude dude)
        {
            var adjacentEnemies = grid.ValidNeighbors(dude.X, dude.Y).Where(e => (grid[e.X, e.Y]) == dude.Enemy).Select(e => dudesOnGrid[(e.X, e.Y)]).Where(e => (e.HP > 0));

            if (adjacentEnemies.Any())
            {
                Dude enemy = adjacentEnemies.OrderBy(e => e.HP).ThenBy(e => e.ReadPos).First();

                enemy.HP -= dude.AttackPower;

                if (enemy.HP <= 0)
                {
                    grid[enemy.X, enemy.Y] = '.';
                    dudesOnGrid.Remove((enemy.X, enemy.Y));

                    if (keepElvesAlive && (enemy is Elf))
                    {
                        done = true;
                    }
                    else if (Finished())
                    {
                        done = true;
                    }

                    grid.PrintToConsole();
                }

                return true;
            }

            return false;
        }

        public int SimulateBattle()
        {
            int round = 0;
            done = false;

            do
            {
                var toRemove = dudes.Where(d => (d.HP <= 0)).ToArray();

                foreach (Dude dude in toRemove)
                    dudes.Remove(dude);

                dudes.Sort((a, b) => a.ReadPos.CompareTo(b.ReadPos));

                foreach (Dude dude in dudes)
                {
                    if (dude.HP <= 0)
                        continue;

                    if (done)
                        return round;

                    if (DoAttack(dude))
                    {
                        continue;
                    }

                    HashSet<(int X, int Y)> possibleTargets = new HashSet<(int X, int Y)>();

                    int minCost = int.MaxValue;
                    (int X, int Y) minStep = (0, 0);

                    foreach (Dude dude2 in dudes)
                    {
                        if (dude2.HP <= 0)
                            continue;

                        if (dude.GetType() != dude2.GetType())
                        {
                            foreach (var pos in GetUnblockedNeighbors((dude2.X, dude2.Y)))
                            {
                                possibleTargets.Add(pos);
                            }
                        }
                    }

                    foreach (var target in possibleTargets.OrderBy(t => ReadPos(t.X, t.Y)))
                    {
                        foreach (var adj in GetUnblockedNeighbors((dude.X, dude.Y)))
                        {
                            List<(int X, int Y)> path;
                            float cost;

                            if (GetShortestPath(adj.X, adj.Y, target.X, target.Y, out path, out cost))
                            {
                                if (cost < minCost)
                                {
                                    minCost = (int)cost;
                                    minStep = adj;
                                }
                            }
                        }
                    }                    

                    if (minCost < int.MaxValue)
                    {
                        grid[dude.X, dude.Y] = '.';
                        dudesOnGrid.Remove((dude.X, dude.Y));

                        dude.X = minStep.X;
                        dude.Y = minStep.Y;

                        grid[dude.X, dude.Y] = dude.Type;
                        dudesOnGrid[(dude.X, dude.Y)] = dude;
                    }

                    DoAttack(dude);
                }

                round++;
                //grid.PrintToConsole();
            }
            while (!done);

            return round;
        }

        public long Compute()
        {
            int round;

            ReadInput();

            int startElves = dudes.Where(d => (d is Elf) && (d.HP > 0)).Count();

            grid.PrintToConsole();

            round = SimulateBattle();

            int sumHP = 0;

            foreach (Dude dude in dudes)
            {
                if (dude.HP > 0)
                    sumHP += dude.HP;
            }

            return sumHP * round;
        }

        public long Compute2()
        {
            elfAttackPower = 4;
            keepElvesAlive = true;

            int round;

            do
            {
                ReadInput();

                int startElves = dudes.Where(d => (d is Elf) && (d.HP > 0)).Count();

                grid.PrintToConsole();

                round = SimulateBattle();

                int endElves = dudes.Where(d => (d is Elf) && (d.HP > 0)).Count();

                if (startElves == endElves)
                    break;

                elfAttackPower++;
            }
            while (true);

            int sumHP = 0;

            foreach (Dude dude in dudes)
            {
                if (dude.HP > 0)
                    sumHP += dude.HP;
            }

            return sumHP * round;
        }
    }
}
