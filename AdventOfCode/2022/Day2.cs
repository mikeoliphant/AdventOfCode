namespace AdventOfCode._2022
{
    internal class Day2 : Day
    {
        int Score(int p1, char p2)
        {
            int val1 = p1 - 'A';
            int val2 = p2 - 'X';

            if (val1 == val2)
                return 3 + val2 + 1;

            if (((val1 + 1) % 3) == val2)
                return 6 + val2 + 1;

            return val2 + 1;
        }

        public override long Compute()
        {
            var rounds = File.ReadLines(DataFile);

            long score = 0;

            foreach (string round in rounds)
            {
                score += Score(round[0], round[2]);
            }

            return score;
        }

        public override long Compute2()
        {
            var rounds = File.ReadLines(DataFile);

            long score = 0;

            foreach (string round in rounds)
            {
                int val1 = round[0] - 'A';
                int val2 = round[2] - 'X';

                switch (val2)
                {
                    case 0:
                        score += ((val1 + 2) % 3) + 1;
                        break;
                    case 1:
                        score += 3;
                        score += val1 + 1;
                        break;
                    case 2:
                        score += 6;
                        score += ((val1 + 1) % 3) + 1;
                        break;
                }
            }

            return score;
        }
    }
}
