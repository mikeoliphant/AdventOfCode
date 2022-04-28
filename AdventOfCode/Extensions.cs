namespace AdventOfCode
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


        public static LinkedListNode<T> MoveCircular<T>(this LinkedListNode<T> node, int move)
        {
            int dir = Math.Sign(move);

            while (move != 0)
            {
                if (dir == 1)
                {
                    if (node.Next != null)
                        node = node.Next;
                    else
                        node = node.List.First;
                }
                else
                {
                    if (node.Previous != null)
                        node = node.Previous;
                    else
                        node = node.List.Last;
                }

                move -= dir;
            }

            return node;
        }

    }
}
