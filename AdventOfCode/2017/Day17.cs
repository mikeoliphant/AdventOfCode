namespace AdventOfCode._2017
{
    internal class Day17
    {
        public long Compute()
        {
            int stepSize = 329;

            LinkedList<int> list = new LinkedList<int>();

            list.AddLast(0);

            var pos = list.First;

            for (int turn = 0; turn < 2017; turn++)
            {
                pos = pos.MoveCircular(stepSize);

                list.AddAfter(pos, turn + 1);

                pos = pos.MoveCircular(1);
            }

            return pos.MoveCircular(1).Value;
        }

        public long Compute2()
        {
            int stepSize = 329;

            LinkedList<int> list = new LinkedList<int>();

            list.AddLast(0);

            var pos = list.First;

            for (int turn = 0; turn < 50000000; turn++)
            {
                pos = pos.MoveCircular(stepSize);

                list.AddAfter(pos, turn + 1);

                pos = pos.MoveCircular(1);
            }

            return pos.MoveCircular(1).Value;
        }
    }
}
