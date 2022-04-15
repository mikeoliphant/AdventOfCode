namespace AdventOfCode._2020
{
    public class Day15
    {
        int[] numbers;

        void ReadInput()
        {
            //numbers = (from numStr in "0,3,6".Split(',') select int.Parse(numStr)).ToArray();
            numbers = (from numStr in "0,8,15,2,12,1,4".Split(',') select int.Parse(numStr)).ToArray();
            
        }

        public long Compute()
        {
            ReadInput();

            long turn = 1;

            Dictionary<long, long> history = new Dictionary<long, long>();

            long lastNum = 0;
            long lastHist = 0;
            bool lastWasFirst = false;

            do
            {
                long num = 0;

                if (turn <= numbers.Length)
                {
                    num = numbers[turn - 1];
                }
                else
                {
                    if (lastWasFirst)
                    {
                        num = 0;
                    }
                    else
                    {
                        num = (turn - 1) - lastHist;
                    }
                }

                lastWasFirst = !history.ContainsKey(num);

                if (!lastWasFirst)
                    lastHist = history[num];

                history[num] = turn;

                lastNum = num;

                turn++;
            }
            while (turn <= 30000000);

            return 0;
        }
    }
}
