﻿namespace AdventOfCode
{
    internal static class Extensions
    {
        public static int ManhattanDistance(this Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static float ManhattanDistance(this Vector3 p1, Vector3 p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z);
        }

        public static float ManhattanDistance(this Vector4 p1, Vector4 p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z) + Math.Abs(p1.W - p2.W);
        }
    }
}
