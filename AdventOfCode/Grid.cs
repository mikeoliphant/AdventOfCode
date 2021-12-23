using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Grid<T> : IEquatable<Grid<T>>
    {
        public int Width { get { return data.GetLength(0); } }
        public int Height { get { return data.GetLength(1); } }

        public T InvalidValue { get; set; }

        T[,] data;

        public Grid()
        {
            InvalidValue = default(T);
        }

        public Grid(int width, int height)
            : this()
        {
            data = new T[width, height];
        }

        public Grid(Grid<T> srcGrid)
        {
            data = new T[srcGrid.Width, srcGrid.Height];

            Copy(srcGrid, this);
        }

        public T this[int index1, int index2]
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

        public T GetValue(int x, int y)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return InvalidValue;

            return data[x, y];
        }

        public void SetValue(int x, int y, T value)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
                return;

            data[x, y] = value;
        }

        public virtual bool Equals(Grid<T> other)
        {
            return this.GetAllValues().SequenceEqual(other.GetAllValues());
        }

        public static void Copy(Grid<T> src, Grid<T> dest)
        {
            Copy(src, dest, 0, 0);
        }

        public static void Copy(Grid<T> src, Grid<T> dest, int destX, int destY)
        {
            for (int x = 0; x < src.Width; x++)
            {
                for (int y = 0; y < src.Height; y++)
                {
                    dest.SetValue(destX + x, destY + y, src[x, y]);
                }
            }
        }

        public static void Copy(Grid<T> src, Grid<T> dest, int srcX, int srcY, int srcWidth, int srcHeight, int destX, int destY)
        {
            for (int x = 0; x < srcWidth; x++)
            {
                for (int y = 0; y < srcHeight; y++)
                {
                    dest.SetValue(destX + x, destY + y, src[x + srcX, y + srcY]);
                }
            }
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
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    data[x, y] = value;
                }
            }
        }

        public IEnumerable<T> GetAllValues()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return data[x, y];
                }
            }
        }

        public IEnumerable<T> GetRow(int row)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return data[x, row];
            }
        }

        public IEnumerable<T> GetCol(int col)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return data[col, y];
            }
        }

        public IEnumerable<T> GetWindow(int x, int y, int size, bool includeSelf)
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

        public IEnumerable<T> AllNeighbors(int x, int y, bool includeDiagonal)
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

        public IEnumerable<T> ValidNeighbors(int x, int y, bool includeDiagonal)
        {
            if (x > 0)
                yield return GetValue(x - 1, y);

            if (x < (Width - 1))
                yield return GetValue(x + 1, y);

            if (y > 0)
                yield return GetValue(x, y - 1);

            if (y < (Height - 1))
                yield return GetValue(x, y + 1);

            if (includeDiagonal)
            {
                if (x > 0)
                {
                    if (y > 0)
                        yield return GetValue(x - 1, y - 1);

                    if (y < (Height - 1))
                        yield return GetValue(x - 1, y + 1);
                }

                if (x < (Width - 1))
                {
                    if (y > 0)
                        yield return GetValue(x + 1, y - 1);

                    if (y < (Height - 1))
                        yield return GetValue(x + 1, y + 1);
                }
            }
        }


        public Grid<T> CreateData(int width, int height)
        {
            data = new T[width, height];

            return this;
        }

        public Grid<T> CreateDataFromRows(IEnumerable<IEnumerable<T>> rows)
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

        public void PrintToConsole()
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
}
