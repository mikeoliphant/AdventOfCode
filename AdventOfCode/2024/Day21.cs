namespace AdventOfCode._2024
{
    using State = ((int X, int Y) Pos, string ToType);
    using StackState = (IEnumerable<(int X, int Y)> Pos, string ToType);
    using System.DirectoryServices.ActiveDirectory;
    using System.IO;
    using System.Collections.Frozen;

    internal class Day21 : Day
    {
        Grid<char> numPad = new Grid<char>().CreateDataFromRows(new string[] { "789", "456", "123", " 0A" });
        Grid<char> dPad = new Grid<char>().CreateDataFromRows(new string[] { " ^A", "<v>" });

        IEnumerable<string> GetShortestPaths(Grid<char> pad, char start, IEnumerable<char> code)
        {
            if (code.Any())
            {
                var nextPaths = GetPaths(pad, start, code.First()).ToList();

                var restPaths = GetShortestPaths(pad, code.First(), code.Skip(1)).ToList();

                if (restPaths.Count == 0)
                {
                    foreach (var next in nextPaths)
                        yield return next;
                }
                else
                {
                    foreach (var next in nextPaths)
                    {
                        foreach (var rest in restPaths)
                        {
                            yield return next + rest;
                        }
                    }
                }
            }
        }

        Dictionary<(char, char), string> cache = new();
        Dictionary<(char, char), List<(char, char)>> transitions = new();

        Random random = new();

        IEnumerable<string> GetPaths(Grid<char> pad, char start, char end)
        {
            var startPos = (IntVec2)pad.FindValue(start).First();

            IntVec2 diff = (IntVec2)pad.FindValue(end).First() - startPos;

            if (diff == IntVec2.Zero)
                yield return "A";
            else
            {
                char xChar = diff.X > 0 ? '>' : '<';
                char yChar = diff.Y > 0 ? 'v' : '^';

                IntVec2 abs = (Math.Abs(diff.X), Math.Abs(diff.Y));

                if (diff.X == 0)
                    yield return new string(yChar, abs.Y) + "A";
                else if (diff.Y == 0)
                    yield return new string(xChar, abs.X) + "A";
                else
                {
                    if (pad[startPos.X, startPos.Y + diff.Y] != ' ')
                        yield return new string(yChar, abs.Y) + new string(xChar, abs.X) + "A";

                    if (pad[startPos.X + diff.X, startPos.Y] != ' ')
                        yield return new string(xChar, abs.X) + new string(yChar, abs.Y) + "A";
                }
            }
        }

        int ManhattanDistance(string code)
        {
            char lastChar = 'A';

            int dist = 0;

            foreach (char c in code)
            {
                dist += ((IntVec2)dPad.FindValue(lastChar).First()).ManhattanDistance(dPad.FindValue(c).First());

                lastChar = c;
            }

            return dist;
        }

        void CacheTransitions()
        {
            cache.Clear();
            transitions.Clear();

            var chars = dPad.GetAllValues().Where(c => c != ' ');

            foreach (char c in chars)
            {
                foreach (char c2 in chars)
                {
                    var paths = GetPaths(dPad, c, c2);

                    cache[(c, c2)] = paths.Skip(random.Next(paths.Count())).First();    // Randomize because I'm too lazy to just test all permutations

                    var trans = new List<(char, char)>();

                    char lastChar = 'A';

                    foreach (char t in cache[(c, c2)])
                    {
                        trans.Add((lastChar, t));

                        lastChar = t;
                    }

                    transitions[(c, c2)] = trans;
                }
            }
        }

        List<char> GetShortestPath(Grid<char> pad, char start, List<char> code)
        {
            List<char> soFar = new();

            char lastMove = 'A';
            char lastChar = 'A';

            foreach (char c in code)
            {
                var pathToNext = cache[(lastChar, c)];

                foreach (char n in pathToNext)
                    soFar.Add(n);

                lastChar = c;
                lastMove = pathToNext[pathToNext.Length - 1];
            }

            return soFar;
        }

        string GetShortestPath(string code, int numRobots)
        {
            var paths = GetShortestPaths(numPad, 'A', code).Select(p => p.ToList());

            List<char> p = null;

            List<List<char>> results = new();

            foreach (var path in paths)
            {
                p = path;

                for (int i = 0; i < numRobots; i++)
                {
                    p = GetShortestPath(dPad, 'A', p);
                }

                results.Add(p);
            }

            return new string(results.OrderBy(p => p.Count).First().ToArray());
        }

        public override long Compute()
        {
            //long len = GetShortestPath("179A", 2);

            long complexity = 0;

            CacheTransitions();

            foreach (string code in File.ReadLines(DataFileTest))
            {
                string path = GetShortestPath(code, 2);

                complexity += (path.Length * int.Parse(code.Substring(0, 3)));
            }

            return complexity;
        }

        long GetShortestPathLength(string code, int numRobots)
        {
            var paths = GetShortestPaths(numPad, 'A', code).Select(p => p.ToList());

            List<char> p = null;

            List<List<char>> results = new();

            long minLength = long.MaxValue;

            foreach (var path in paths)
            {
                Dictionary<(char, char), long> transitionCounts = new();

                foreach (var t in transitions)
                {
                    transitionCounts[t.Key] = 0;
                }

                char lastChar = 'A';

                foreach (char c in path)
                {
                    transitionCounts[(lastChar, c)]++;

                    lastChar = c;
                }

                for (int i = 0; i < numRobots; i++)
                {
                    Dictionary<(char, char), long> newCounts = new();

                    foreach (var t in transitions)
                    {
                        newCounts[t.Key] = 0;
                    }

                    foreach (var t in transitionCounts)
                    {
                        foreach (var next in transitions[t.Key])
                        {
                            newCounts[next] += t.Value;
                        }
                    }

                    transitionCounts = newCounts;
                }

                long length = transitionCounts.Values.Sum();

                if (length < minLength)
                    minLength = length;
            }

            return minLength;
        }

        public override long Compute2()
        {
            //long len = GetShortestPath("179A", 2);

            HashSet<long> comps = new();

            for (int i = 0; i < 1000; i++)
            {
                long complexity = 0;

                CacheTransitions();

                foreach (string code in File.ReadLines(DataFile))
                {
                    long length = GetShortestPathLength(code, 25);

                    complexity += (length * long.Parse(code.Substring(0, 3)));
                }

                comps.Add(complexity);
            }

            return comps.Min();
        }
    }
}
