using System.Diagnostics;
using System.Security.Policy;

namespace AdventOfCode
{
    public class Grid
    {
        public virtual int Width { get { throw new NotImplementedException(); } }
        public virtual int Height { get { throw new NotImplementedException(); } }

        public bool IsValid(int x, int y)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return false;

            return true;
        }

        public bool IsValid((int X, int Y) cell)
        {
            if ((cell.X < 0) || (cell.X >= Width) || (cell.Y < 0) || (cell.Y >= Height))
                return false;

            return true;
        }

        public virtual bool IsDefault(int x, int y)
        {
            throw new InvalidOperationException();
        }

        public virtual IEnumerable<(int X, int Y)> GetAll()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<(int X, int Y)> GetRectangleInclusive(Rectangle rect)
        {
            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                for (int x = rect.X; x <= rect.Right; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int X, int Y)> GetRectangle(Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int X, int Y)> AllNeighbors((int X, int Y) pos)
        {
            return AllNeighbors(pos.X, pos.Y, includeDiagonal: false);
        }

        public static IEnumerable<(int X, int Y)> AllNeighbors(int x, int y)
        {
            return AllNeighbors(x, y, includeDiagonal: false);
        }

        public static IEnumerable<(int X, int Y)> AllNeighbors(int x, int y, bool includeDiagonal)
        {
            yield return (x - 1, y);
            yield return (x + 1, y);
            yield return (x, y - 1);
            yield return (x, y + 1);

            if (includeDiagonal)
            {
                yield return (x - 1, y - 1);
                yield return (x - 1, y + 1);
                yield return (x + 1, y - 1);
                yield return (x + 1, y + 1);
            }
        }

        public static IEnumerable<(int X, int Y)> DiagonalNeighbors(int x, int y)
        {
            yield return (x - 1, y - 1);
            yield return (x - 1, y + 1);
            yield return (x + 1, y - 1);
            yield return (x + 1, y + 1);
        }

        public IEnumerable<(int X, int Y)> ValidNeighbors(int x, int y)
        {
            return ValidNeighbors(x, y, includeDiagonal: false);
        }

        public virtual IEnumerable<(int X, int Y)> ValidNeighbors(int x, int y, bool includeDiagonal)
        {
            return AllNeighbors(x, y, includeDiagonal);
        }

        public virtual IEnumerable<(int X, int Y)> ValidDiagonalNeighbors(int x, int y)
        {
            return DiagonalNeighbors(x, y);
        }
    }

    public class GridBase<T> : Grid, IEquatable<GridBase<T>>
    {
        public T DefaultValue { get; set; }

        public GridBase()
        {
            DefaultValue = default(T);
        }

        public virtual T this[int index1, int index2]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual T this[(int X, int Y) pos]
        {
            get
            {
                return this[pos.X, pos.Y];
            }

            set
            {
                this[pos.X, pos.Y] = value;
            }
        }

        public override bool IsDefault(int x, int y)
        {
            return this[x, y].Equals(DefaultValue);
        }

        public virtual bool Equals(GridBase<T> other)
        {
            return this.GetAllValues().SequenceEqual(other.GetAllValues());
        }

        public override bool Equals(object other)
        {
            if (!(other is GridBase<T>))
                return false;

            return this.Equals(other as GridBase<T>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;

                foreach (var val in GetAllValues())
                {
                    hash = hash * 31 + val.GetHashCode();
                }
                return hash;
            }
        }

        public virtual T GetValue(int x, int y)
        {
            return this[x, y];
        }

        public virtual T GetValue((int X, int Y) pos)
        {
            return GetValue(pos.X, pos.Y);
        }

        public virtual bool TryGetValue(int x, int y, out T value)
        {
            throw new NotImplementedException();
        }

        public virtual void SetValue(int x, int y, T value)
        {
            this[x, y] = value;
        }

        public virtual void SetValue((int X, int Y) pos, T value)
        {
            this[pos.X, pos.Y] = value;
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual void CopyTo(GridBase<T> dest)
        {
            foreach (var pos in GetAll())
            {
                T srcVal = this[pos.X, pos.Y];

                if (!srcVal.Equals(DefaultValue))
                    dest.SetValue(pos.X, pos.Y, srcVal);
            }
        }

        public static void Copy(GridBase<T> src, GridBase<T> dest, int destX, int destY)
        {
            foreach (var pos in src.GetAll())
            {
                T srcVal = src[pos.X, pos.Y];

                if (!srcVal.Equals(src.DefaultValue))
                    dest.SetValue(destX + pos.X, destY + pos.Y, srcVal);
            }
        }

        public static void Copy(Grid<T> src, Grid<T> dest, int srcX, int srcY, int srcWidth, int srcHeight, int destX, int destY)
        {
            for (int x = 0; x < srcWidth; x++)
            {
                for (int y = 0; y < srcHeight; y++)
                {
                    T srcVal = src[x + srcX, y + srcY];

                    if (!srcVal.Equals(src.DefaultValue))
                        dest.SetValue(destX + x, destY + y, srcVal);
                }
            }
        }

        public virtual GridBase<T> Clone()
        {
            var newGrid = CloneEmpty();

            this.CopyTo(newGrid);

            return newGrid;
        }

        public virtual GridBase<T> CloneEmpty()
        {
            return Activator.CreateInstance(this.GetType()) as GridBase<T>;
        }

        public bool MatchesPattern(Grid<T> patternGrid, int xOffset, int yOffset, T wildcard)
        {
            for (int y = 0; y < patternGrid.Height; y++)
            {
                for (int x = 0; x < patternGrid.Width; x++)
                {
                    if (!(patternGrid.GetValue(x, y).Equals(wildcard) || patternGrid.GetValue(x, y).Equals(GetValue(xOffset + x, yOffset + y))))
                        return false;
                }
            }

            return true;
        }

        public void Fill(T value)
        {
            foreach (var pos in GetAll())
            {
                this[pos.X, pos.Y] = value;
            }
        }

        public void Replace(T src, T dest)
        {
            foreach (var pos in GetAll())
            {
                if (this[pos.X, pos.Y].Equals(src))
                    this[pos.X, pos.Y] = dest;
            }
        }

        public IEnumerable<(int X, int Y)> FindValue(T value)
        {
            return GetAll().Where(g => this[g.X, g.Y].Equals(value));
        }

        public long CountValue(T value)
        {
            return GetAllValues().Where(v => v.Equals(value)).LongCount();
        }

        public virtual IEnumerable<T> GetAllValues()
        {
            foreach (var pos in GetAll())
            {
                yield return this[pos.X, pos.Y];
            }
        }

        public IEnumerable<T> GetAllRectangleValues(Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    yield return this[x, y];
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

        public IEnumerable<T> GetWindowValues(int x, int y, int size, bool includeSelf)
        {
            for (int dy = -size; dy <= size; dy++)
            {
                for (int dx = -size; dx <= size; dx++)
                {
                    if (includeSelf || (dx != 0 && dy != 0))
                    {
                        yield return GetValue(x + dx, y + dy);
                    }
                }
            }
        }

        public IEnumerable<T> AllNeighborValues(int x, int y)
        {
            return AllNeighborValues(x, y, includeDiagonal: false);
        }

        public IEnumerable<T> AllNeighborValues(int x, int y, bool includeDiagonal)
        {
            yield return GetValue(x - 1, y);
            yield return GetValue(x + 1, y);
            yield return GetValue(x, y - 1);
            yield return GetValue(x, y + 1);

            if (includeDiagonal)
            {
                yield return GetValue(x - 1, y - 1);
                yield return GetValue(x - 1, y + 1);
                yield return GetValue(x + 1, y - 1);
                yield return GetValue(x + 1, y + 1);
            }
        }

        public IEnumerable<T> ValidNeighborValues(int x, int y)
        {
            return ValidNeighborValues(x, y, includeDiagonal: false);
        }

        public virtual IEnumerable<T> ValidNeighborValues(int x, int y, bool includeDiagonal)
        {
            return ValidNeighbors(x, y, includeDiagonal).Select(n => this[n.X, n.Y]);
        }

        public virtual GridBase<T> CreateDataFromRows(IEnumerable<IEnumerable<T>> rows)
        {
            throw new NotImplementedException();
        }

        public virtual void PrintToConsole()
        {
            throw new NotImplementedException();
        }
    }

    public class Grid<T> : GridBase<T>
    {
        public override int Width { get { return data.GetLength(0); } }
        public override int Height { get { return data.GetLength(1); } }

        T[,] data;

        public Grid()
            : base()
        {

        }

        public Grid(int width, int height)
            : base()
        {
            data = new T[width, height];
        }

        public Grid(Grid<T> srcGrid)
        {
            data = new T[srcGrid.Width, srcGrid.Height];

            srcGrid.CopyTo(this);
        }

        public override T this[int index1, int index2]
        {
            get
            {
                return data[index1, index2];
            }

            set
            {
                data[index1, index2] = value;
            }
        }

        public override T GetValue(int x, int y)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return DefaultValue;

            return data[x, y];
        }

        public override void SetValue(int x, int y, T value)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return;

            data[x, y] = value;
        }

        public override bool TryGetValue(int x, int y, out T value)
        {
            value = default(T);

            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return false;

            value = data[x, y];

            return true;
        }

        public override void Clear()
        {
            Fill(DefaultValue);
        }

        public override Grid<T> CloneEmpty()
        {
            return new Grid<T>(Width, Height);
        }

        public Grid<T> Clone()
        {
            var newGrid = CloneEmpty();

            this.CopyTo(newGrid);

            return newGrid;
        }

        public void CopyTo(Grid<T> dest)
        {
            dest.data = data.Clone() as T[,];
        }


        public override IEnumerable<(int X, int Y)> GetAll()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public override IEnumerable<T> GetAllValues()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return data[x, y];
                }
            }
        }

        public IEnumerable<T> GetRowValues(int row)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return data[x, row];
            }
        }

        public IEnumerable<T> GetColValues(int col)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return data[col, y];
            }
        }

        public T GetTransformedValue(int x, int y, int rotation, bool flipX, bool flipY)
        {
            if (flipX)
                x = Width - x - 1;

            if (flipY)
                y = Height - y - 1;

            switch (rotation)
            {
                case 0:
                    return GetValue(x, y);

                case 1:
                    return GetValue(y, Width - x - 1);

                case 2:
                    return GetValue(Width - x - 1, Height - y - 1);

                case 3:
                    return GetValue(Height - y - 1, x);

            }

            throw new ArgumentException("'rotation' must be 0-3");
        }

        static bool[] bools = new bool[] { true, false };

        public bool MatchSquareWithTransforms(int subX, int subY, Grid<T> toMatch, out int rotation, out bool flipX, out bool flipY)
        {
            foreach (bool fx in bools)
            {
                foreach (bool fy in bools)
                {
                    for (int rot = 0; rot < 4; rot++)
                    {
                        bool isMatch = true;

                        for (int y = 0; y < toMatch.Height; y++)
                        {
                            for (int x = 0; x < toMatch.Width; x++)
                            {
                                if (!toMatch.GetTransformedValue(x, y, rot, fx, fy).Equals(GetValue(subX + x, subY + y)))
                                {
                                    isMatch = false;

                                    break;
                                }                               
                            }

                            if (!isMatch)
                                break;
                        }

                        if (isMatch)
                        {
                            flipX = fx;
                            flipY = fy;
                            rotation = rot;

                            return true;
                        }
                    }
                }
            }

            rotation = 0;
            flipX = false;
            flipY = false;

            return false;
        }

        public IEnumerable<T> GetEdge(int edge)
        {
            switch (edge)
            {
                case 0:
                    return (from x in Enumerable.Range(0, Width) select this[x, 0]);

                case 1:
                    return (from y in Enumerable.Range(0, Height) select this[Width - 1, y]);

                case 2:
                    return (from x in Enumerable.Range(0, Width).Reverse() select this[x, Height - 1]);

                case 3:
                    return (from y in Enumerable.Range(0, Height).Reverse() select this[0, y]);
            }

            throw new ArgumentException("'edge' must be 0-3");
        }

        public Grid<T> Transform(int rotation, bool flipX, bool flipY)
        {
            Grid<T> newGrid = new Grid<T>(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newGrid[x, y] = GetTransformedValue(x, y, rotation, flipX, flipY);
                }
            }

            return newGrid;
        }

        public override IEnumerable<(int X, int Y)> ValidNeighbors(int x, int y, bool includeDiagonal)
        {
            if (x > 0)
                yield return (x - 1, y);

            if (x < (Width - 1))
                yield return (x + 1, y);

            if (y > 0)
                yield return (x, y - 1);

            if (y < (Height - 1))
                yield return (x, y + 1);

            if (includeDiagonal)
            {
                if (x > 0)
                {
                    if (y > 0)
                        yield return (x - 1, y - 1);

                    if (y < (Height - 1))
                        yield return (x - 1, y + 1);
                }

                if (x < (Width - 1))
                {
                    if (y > 0)
                        yield return (x + 1, y - 1);

                    if (y < (Height - 1))
                        yield return (x + 1, y + 1);
                }
            }
        }

        public override IEnumerable<(int X, int Y)> ValidDiagonalNeighbors(int x, int y)
        {
            if (x > 0)
            {
                if (y > 0)
                    yield return (x - 1, y - 1);

                if (y < (Height - 1))
                    yield return (x - 1, y + 1);
            }

            if (x < (Width - 1))
            {
                if (y > 0)
                    yield return (x + 1, y - 1);

                if (y < (Height - 1))
                    yield return (x + 1, y + 1);
            }
        }

        public Grid<T> CreateData(int width, int height)
        {
            data = new T[width, height];

            return this;
        }

        public override Grid<T> CreateDataFromRows(IEnumerable<IEnumerable<T>> rows)
        {
            T[][] array = rows.Select(a => a.ToArray()).ToArray();

            data = new T[array[0].Length, array.Length];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    data[x, y] = array[y][x];
                }
            }

            return this;
        }

        public override void PrintToConsole()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(data[x, y]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    public class SparseGrid<T> : GridBase<T>
    {
        protected Dictionary<ValueTuple<int, int>, T> data = new Dictionary<ValueTuple<int, int>, T>();


        public SparseGrid()
            : base()
        {
        }

        public SparseGrid(SparseGrid<T> srcGrid)
        {
            this.data = new Dictionary<(int, int), T>(srcGrid.data);
            this.DefaultValue = srcGrid.DefaultValue;
        }

        public override T this[int index1, int index2]
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

        public override T this[(int X, int Y) pos]
        {
            get
            {
                return data[pos];
            }

            set
            {
                data[pos] = value;
            }
        }

        public int Count { get { return data.Count; } }

        public override SparseGrid<T> CreateDataFromRows(IEnumerable<IEnumerable<T>> rows)
        {
            T[][] array = rows.Select(a => a.ToArray()).ToArray();

            for (int y = 0; y < array.Length; y++)
            {
                for (int x = 0; x < array[y].Length; x++)
                {
                    data[(x, y)] = array[y][x];
                }
            }

            return this;
        }

        public override T GetValue(int x, int y)
        {
            T value = DefaultValue;

            TryGetValue(x, y, out value);

            return value;
        }

        public bool IsValid((int X, int Y) pos)
        {
            return data.ContainsKey(pos);
        }

        public override bool TryGetValue(int index1, int index2, out T value)
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

        public void RemoveValue((int X, int Y) pos)
        {
            data.Remove(pos);
        }

        public override void Clear()
        {
            data.Clear();
        }

        public override IEnumerable<(int X, int Y)> GetAll()
        {
            return data.Keys;
        }

        public override IEnumerable<T> GetAllValues()
        {
            return data.Values;
        }

        public override IEnumerable<(int X, int Y)> ValidNeighbors(int x, int y, bool includeDiagonal)
        {
            foreach (var pos in AllNeighbors(x, y, includeDiagonal))
            {
                T val = DefaultValue;

                if (TryGetValue(pos.X, pos.Y, out val))
                {
                    yield return pos;
                }
            }
        }

        public (int MinX, int MinY, int MaxX, int MaxY) GetBounds()
        {
            int minX;
            int minY;
            int maxX;
            int maxY;

            GetBounds(out minX, out minY, out maxX, out maxY);

            return (minX, minY, maxX, maxY);
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

        public override void PrintToConsole()
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

            Console.WriteLine();
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
            grid.DefaultValue = DefaultValue;
            grid.Fill(DefaultValue);

            T value = DefaultValue;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (TryGetValue(x, y, out value))
                    {
                        grid[x - minX, y - minY] = value;
                    }
                }
            }

            return grid;
        }

    }
}
