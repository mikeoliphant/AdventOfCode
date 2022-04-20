namespace AdventOfCode
{
    public class SparseGrid<T>
    {
        protected Dictionary<ValueTuple<int, int>, T> data = new Dictionary<ValueTuple<int, int>, T>();

        public T DefaultValue { get; set; }

        public SparseGrid()
        {
            DefaultValue = default(T);
        }

        public virtual T this[int index1, int index2]
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

        public SparseGrid<T> CreateDataFromRows(IEnumerable<IEnumerable<T>> rows)
        {
            T[][] array = rows.Select(a => a.ToArray()).ToArray();

            for (int y = 0; y < array.Length; y++)
            {
                for (int x = 0; x < array[0].Length; x++)
                {
                    data[(x, y)] = array[y][x];
                }
            }

            return this;
        }

        public bool TryGetValue(int index1, int index2, out T value)
        {
            ValueTuple<int, int> t = (index1, index2);

            if (data.ContainsKey(t))
            {
                value = data[t];

                return true;
            }

            value = DefaultValue;

            return false;
        }

        public IEnumerable<(int X, int Y)> GetAll()
        {
            return data.Keys;
        }

        public IEnumerable<T> GetAllValues()
        {
            return data.Values;
        }

        public IEnumerable<(int X, int Y)> GetRectangle(Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public IEnumerable<T> GetAllRectangleValues(Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    yield return data[(x, y)];
                }
            }
        }

        public IEnumerable<T> GetValidRectangleValues(Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    T value;

                    if (TryGetValue(x, y, out value))
                        yield return value;
                }
            }
        }

        public void GetBounds(out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = int.MaxValue;
            maxX = int.MinValue;
            minY = int.MaxValue;
            maxY = int.MinValue;

            foreach (ValueTuple<int, int> t in data.Keys)
            {
                minX = Math.Min(minX, t.Item1);
                maxX = Math.Max(maxX, t.Item1);
                minY = Math.Min(minY, t.Item2);
                maxY = Math.Max(maxY, t.Item2);
            }
        }

        public void PrintToConsole()
        {
            int minX;
            int maxX;
            int minY;
            int maxY;

            GetBounds(out minX, out minY, out maxX, out maxY);

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

        public Grid<T> ToGrid()
        {
            int minX;
            int maxX;
            int minY;
            int maxY;

            GetBounds(out minX, out minY, out maxX, out maxY);

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            Grid<T> grid = new Grid<T>(width, height);

            T value = DefaultValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (TryGetValue(x, y, out value))
                        grid[x - minX, y - minY] = value;
                }
            }

            return grid;
        }

        public IEnumerable<(int X, int Y)> GetValidNeighbors(int x, int y)
        {
            if (data.ContainsKey((x - 1, y)))
                yield return (x - 1, y);
            if (data.ContainsKey((x - 1, y)))
                yield return ((x - 1, y));
            if (data.ContainsKey((x, y - 1)))
                yield return (x, y - 1);
            if (data.ContainsKey((x, y + 1)))
                yield return (x, y + 1);
        }

        T blocker;

        IEnumerable<KeyValuePair<ValueTuple<int, int>, float>> GetUnblockedNeighbors(ValueTuple<int, int> position)
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

            DijkstraSearch<ValueTuple<int, int>> search = new DijkstraSearch<ValueTuple<int, int>>(GetUnblockedNeighbors);

            return search.GetShortestPath((startX, startY), (endX, endY), out path, out cost);
        }

    }
}
