using System;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day10
    {
        Grid<char> map;
        List<Vector2> asteroids = new List<Vector2>();

        public void ReadInput()
        {
            map = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day10.txt"));

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[x, y] == '#')
                        asteroids.Add(new Vector2(x + 0.5f, y + 0.5f));
                }
            }
        }

        Vector2 GetMinObscuredAsteroid(out int minObscured)
        {
            minObscured = int.MaxValue;
            Vector2 minObscuredAsteroid = Vector2.Zero;

            foreach (Vector2 asteroid1 in asteroids)
            {
                int numObscured = 0;

                foreach (Vector2 asteroid2 in asteroids)
                {
                    if (asteroid2 == asteroid1)
                        continue;

                    bool isObscured = false;

                    Vector2 diff = asteroid2 - asteroid1;
                    Vector2 diffNorm = Vector2.Normalize(diff);
                    float length = diff.Length();

                    float slope = diff.X / diff.Y;

                    foreach (Vector2 asteroid3 in asteroids)
                    {
                        if ((asteroid3 == asteroid1) || (asteroid3 == asteroid2))
                            continue;

                        Vector2 diff2 = asteroid3 - asteroid1;

                        if ((Vector2.Normalize(diff2) - diffNorm).Length() < 0.0001f)
                        {
                            if (diff2.Length() < length)    // asteroid3 is directly between asteroid1 and asteroid2
                            {
                                isObscured = true;

                                break;
                            }
                        }
                    }

                    if (isObscured)
                        numObscured++;
                }

                if (numObscured < minObscured)
                {
                    minObscured = numObscured;
                    minObscuredAsteroid = asteroid1;
                }
            }

            return minObscuredAsteroid;
        }

        public long Compute()
        {
            ReadInput();

            int numObscured = 0;

            Vector2 minObscuredAsteroid = GetMinObscuredAsteroid(out numObscured);

            return asteroids.Count - numObscured - 1;
        }

        float GetAngle(Vector2 vector)
        {
            float angle = (float)Math.Atan2(vector.X, -vector.Y);

            if (angle < 0)
                angle = (float)(Math.PI * 2) + angle;

            return angle;
        }

        List<Vector2> GetUnobscured(Vector2 asteroid1)
        {
            List<Vector2> unobscured = new List<Vector2>();

            foreach (Vector2 asteroid2 in asteroids)
            {
                if (asteroid2 == asteroid1)
                    continue;

                bool isObscured = false;

                Vector2 diff = asteroid2 - asteroid1;
                Vector2 diffNorm = Vector2.Normalize(diff);
                float length = diff.Length();

                float slope = diff.X / diff.Y;

                foreach (Vector2 asteroid3 in asteroids)
                {
                    if ((asteroid3 == asteroid1) || (asteroid3 == asteroid2))
                        continue;

                    Vector2 diff2 = asteroid3 - asteroid1;

                    if ((Vector2.Normalize(diff2) - diffNorm).Length() < 0.0001f)
                    {
                        if (diff2.Length() < length)    // asteroid3 is directly between asteroid1 and asteroid2
                        {
                            isObscured = true;

                            break;
                        }
                    }
                }

                if (!isObscured)
                {
                    unobscured.Add(asteroid2);
                }
            }

            return unobscured;
        }

        public long Compute2()
        {
            ReadInput();

            int numObscured = 0;

            Vector2 minObscuredAsteroid = GetMinObscuredAsteroid(out numObscured);

            asteroids.Remove(minObscuredAsteroid);

            List<Vector2> vaporized = new List<Vector2>();

            do
            {
                List<Vector2> unobscuredAsteroids = GetUnobscured(minObscuredAsteroid);

                unobscuredAsteroids.Sort((a, b) => GetAngle(Vector2.Normalize(a - minObscuredAsteroid)).CompareTo(GetAngle(Vector2.Normalize(b - minObscuredAsteroid))));

                vaporized.AddRange(unobscuredAsteroids);

                foreach (Vector2 asteroid in unobscuredAsteroids)
                {
                    asteroids.Remove(asteroid);
                }
            }
            while (asteroids.Count > 0);

            Vector2 vaporized200 = vaporized[199];

            return ((int)vaporized200.X * 100) + (int)vaporized200.Y;
        }
    }
}
