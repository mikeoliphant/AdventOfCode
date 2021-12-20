﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Grid<T>
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