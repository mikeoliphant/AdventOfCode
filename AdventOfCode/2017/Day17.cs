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

        public long Compute2BruteForce()
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

        public long Compute2()
        {
            int stepSize = 329;

            int pos = 0;
            int listSize = 1;
            int pos1Val = 0;

            for (int turn = 0; turn < 50000000; turn++)
            {
                pos = (pos + stepSize) % listSize;

                listSize++;

                pos = (pos + 1) % listSize;

                if (pos == 1)
                {
                    pos1Val = turn + 1;
                }
            }

            return pos1Val;
        }
    }
}
