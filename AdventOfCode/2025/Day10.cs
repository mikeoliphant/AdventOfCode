
using System;

namespace AdventOfCode._2025
{
    internal class Day10 : Day
    {
        public string PushButtons(string state, List<int> buttons)
        {
            char[] lights = state.ToCharArray();

            foreach (int button in buttons)
            {
                lights[button] = (lights[button] == '#') ? '.' : '#';
            }

            return new string(lights);
        }

        public override long Compute()
        {
            long tot = 0;

            foreach (string line in File.ReadLines(DataFile))
            {
                var match = Regex.Match(line, @"\[(.*)\](.*)\{(.*)\}");

                var pattern = match.Groups[1].Value;

                var buttonMatch = Regex.Matches(match.Groups[2].Value, @"\((.*?)\)");

                var buttons = buttonMatch.Select(m => m.Groups[1].Value.ToInts(',').ToList()).ToList();

                var search = new DijkstraSearch<string>(delegate (string state)
                {
                    return buttons.Select(b => PushButtons(state, b));
                });

                var result = search.GetShortestPath(new string('.', pattern.Length), pattern);

                tot += (long)result.Cost;
            }

            return tot;
        }

        EquatableArray<int> GetJoltage(EquatableArray<int> state, List<int> buttons)
        {
            var newState = new EquatableArray<int>(new int[state.Array.Length]);

            Array.Copy(state.Array, newState.Array, state.Array.Length);

            foreach (int button in buttons)
            {
                newState.Array[button]++;
            }

            return newState;
        }

        bool IsOver(EquatableArray<int> state, EquatableArray<int> max)
        {
            for (int i = 0; i < state.Array.Length; i++)
            {
                if (state.Array[i] > max.Array[i])
                    return true;
            }

            return false;
        }

        public override long Compute2()
        {
            long tot = 0;

            foreach (string line in File.ReadLines(DataFileTest))
            {
                var match = Regex.Match(line, @"\[(.*)\](.*)\{(.*)\}");

                var buttonMatch = Regex.Matches(match.Groups[2].Value, @"\((.*?)\)");

                var buttons = buttonMatch.Select(m => m.Groups[1].Value.ToInts(',').ToList()).ToList();

                var joltage = match.Groups[3].Value.ToInts(',').ToArray();

                var start = new EquatableArray<int>(new int[joltage.Length]);
                var end = new EquatableArray<int>(joltage);

                var search = new DijkstraSearch<EquatableArray<int>>(delegate (EquatableArray<int> state)
                {
                    return buttons.Select(b => GetJoltage(state, b)).Where(j => !IsOver(j, end));
                });

                var result = search.GetShortestPath(start, end);

                tot += (long)result.Cost;
            }

            return tot;
        }
    }
}
