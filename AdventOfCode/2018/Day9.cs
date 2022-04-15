namespace AdventOfCode._2018
{
    internal class Day9
    {
        public long Compute()
        {
            int numPlayers = 419;
            int lastMarble = 72164;

            lastMarble *= 100;

            long[] scores = new long[numPlayers];

            LinkedList<int> marbles = new LinkedList<int>();

            marbles.AddFirst(0);
            int currentPlayer = 0;
            int currentMarble = 1;
            int lastScore = 0;

            LinkedListNode<int> currentPos = marbles.First;

            do
            {
                if ((currentMarble % 23) == 0)
                {
                    var removePos = currentPos.MoveCircular(-7);

                    scores[currentPlayer] += currentMarble + removePos.Value;

                    currentPos = removePos.MoveCircular(1);

                    marbles.Remove(removePos);

                }
                else
                {
                    currentPos = currentPos.MoveCircular(1);

                    marbles.AddAfter(currentPos, currentMarble);

                    currentPos = currentPos.MoveCircular(1);
                }

                currentPlayer = (currentPlayer + 1) % numPlayers;
                currentMarble++;
            }
            while (currentMarble <= lastMarble);

            return scores.Max();
        }
    }
}
