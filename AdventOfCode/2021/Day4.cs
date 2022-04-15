using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class Day4
    {
        List<int> drawnNumbers;
        List<int[,]> boards = new List<int[,]>();

        void ReadInput()
        {
            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2021\DayFourInput.txt"))
            {
                string[] numbers = reader.ReadLine().Split(',');

                drawnNumbers = new List<int>(from numberString in numbers select int.Parse(numberString));

                reader.ReadLine();

                bool done = false;

                do
                {
                    int[,] board = new int[5, 5];

                    for (int row = 0; row < 5; row++)
                    {
                        string line = reader.ReadLine();

                        if (String.IsNullOrEmpty(line))
                        {
                            done = true;
                            break;
                        }

                        line = line.Trim();

                        string[] lineNumbers = Regex.Split(line, @"\s+");

                        for (int col = 0; col < 5; col++)
                        {
                            board[row, col] = int.Parse(lineNumbers[col]);
                        }
                    }

                    if (done)
                        break;

                    reader.ReadLine();

                    boards.Add(board);
                }
                while (true);
            }
        }

        int CheckScore(int[,] board, int row, int col)
        {
            bool haveRowBingo = true;
            bool haveColBingo = true;

            for (int r = 0; r < 5; r++)
                if (board[r, col] > 0)
                {
                    haveColBingo = false;

                    break;
                }

            for (int c = 0; c < 5; c++)
                if (board[row, c] > 0)
                {
                    haveRowBingo = false;

                    break;
                }

            if (!haveRowBingo && !haveColBingo)
                return 0;

            int score = 0;

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (board[r, c] > 0)
                        score += board[r, c];
                }
            }

            return score;
        }

        int CheckNumberAndScore(int[,] board, int number)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (board[row, col] == number)
                    {
                        board[row, col] = -1;

                        return CheckScore(board, row, col);
                    }
                }
            }

            return 0;
        }

        public void Compute()
        {
            ReadInput();

            foreach (int number in drawnNumbers)
            {
                foreach (int[,] board in boards)
                {
                    int score = CheckNumberAndScore(board, number);

                    if (score > 0)
                    {
                        score = score * number;
                    }
                }
            }
        }

        public void Compute2()
        {
            ReadInput();

            int lastScore = 0;

            foreach (int number in drawnNumbers)
            {
                List<int[,]> boardCopy = new List<int[,]>(boards);

                int pos = 0;

                foreach (int[,] board in boardCopy)
                {
                    int score = CheckNumberAndScore(board, number);

                    if (score > 0)
                    {
                        score = score * number;

                        lastScore = score;

                        boards.RemoveAt(pos);

                        pos--;
                    }

                    pos++;
                }
            }
        }
    }
}
