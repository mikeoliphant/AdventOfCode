using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class SparseGrid<T>
    {
        Dictionary<Tuple<int, int>, T> data = new Dictionary<Tuple<int, int>, T>();

        public T this[int index1, int index2]
        {
            get
            {
                return data[new Tuple<int, int>(index1, index2)];
            }

            set
            {
                data[new Tuple<int, int>(index1, index2)] = value;
            }
        }

        public int Count { get { return data.Count; } }

        public bool TryGetValue(int index1, int index2, out T value)
        {
            Tuple<int, int> t = new Tuple<int, int>(index1, index2);

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

            foreach (Tuple<int, int> t in data.Keys)
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
    }
}
