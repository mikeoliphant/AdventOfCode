﻿using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class SparseGrid<T>
    {
        Dictionary<ValueTuple<int, int>, T> data = new Dictionary<ValueTuple<int, int>, T>();

        public T this[int index1, int index2]
        {
            get
            {
                return data[(index1, index2)];
            }

            set
            {
                data[(index1, index2)] = value;
            }
        }

        public int Count { get { return data.Count; } }

        public bool TryGetValue(int index1, int index2, out T value)
        {
            ValueTuple<int, int> t = (index1, index2);

            if (data.ContainsKey(t))
            {
                value = data[t];

                return true;
            }

            value = default(T);

            return false;
        }

        public IEnumerable<T> GetAllValues()
        {
            return data.Values;
        }

        public void PrintToConsole()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (ValueTuple<int, int> t in data.Keys)
            {
                minX = Math.Min(minX, t.Item1);
                maxX = Math.Max(maxX, t.Item1);
                minY = Math.Min(minY, t.Item2);
                maxY = Math.Max(maxY, t.Item2);
            }

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    T value;

                    TryGetValue(x, y, out value);

                    Console.Write(value);
                }

                Console.WriteLine();
            }
        }

        T blocker;

        IEnumerable<KeyValuePair<ValueTuple<int, int>, float>> GetNeighbors(ValueTuple<int, int> position)
        {
            ValueTuple<int, int> north = (position.Item1, position.Item2 - 1);

            if (!data[north].Equals(blocker))
                yield return new KeyValuePair<(int, int), float>(north, 1);

            ValueTuple<int, int> south = (position.Item1, position.Item2 + 1);

            if (!data[south].Equals(blocker))
                yield return new KeyValuePair<(int, int), float>(south, 1);

            ValueTuple<int, int> west = (position.Item1 - 1, position.Item2);

            if (!data[west].Equals(blocker))
                yield return new KeyValuePair<(int, int), float>(west, 1);

            ValueTuple<int, int> east = (position.Item1 + 1, position.Item2);

            if (!data[east].Equals(blocker))
                yield return new KeyValuePair<(int, int), float>(east, 1);
        }

        public bool DijkstraSearch(int startX, int startY, int endX, int endY, T blocker, out List<ValueTuple<int, int>> path, out float cost)
        {
            this.blocker = blocker;

            DijkstraSearch<ValueTuple<int, int>> search = new DijkstraSearch<ValueTuple<int, int>>(GetNeighbors);

            return search.GetShortestPath((startX, startY), (endX, endY), out path, out cost);
        }

    }
}
