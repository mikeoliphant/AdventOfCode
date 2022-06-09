namespace AdventOfCode._2015
{
    internal class Day25 : Day
    {
        public override long Compute()
        {
            int codeRow = 2981;
            int codeCol = 3075;

            long code = 20151125;

            int size = 1;

            do
            {
                size++;

                int row = size;
                int col = 1;

                do
                {
                    code = (code * 252533) % 33554393;

                    if ((row == codeRow) && (col == codeCol))
                        return code;

                    row--;
                    col++;
                }
                while (row > 0);
            }
            while (true);
        }
    }
}
