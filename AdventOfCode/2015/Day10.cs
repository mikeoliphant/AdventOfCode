namespace AdventOfCode._2015
{
    internal class Day10 : Day
    {
        IEnumerable<char> IntToChars(int val)
        {
            return val.ToString();
        }

        LinkedList<char> SayString(IEnumerable<char> str)
        {
            LinkedList<char> newStr = new LinkedList<char>();

            int numInARow = 0;
            char digit = '\0';

            foreach (char c in str)
            {
                if (c == digit)
                {
                    numInARow++;
                }
                else
                {
                    if (numInARow > 0)
                    {
                        foreach (char nc in IntToChars(numInARow))
                        {
                            newStr.AddLast(nc);
                        }

                        newStr.AddLast(digit);
                    }

                    numInARow = 1;
                    digit = c;
                }
            }

            if (numInARow > 0)
            {
                foreach (char nc in IntToChars(numInARow))
                {
                    newStr.AddLast(nc);

                    newStr.AddLast(digit);
                }
            }

            return newStr;
        }

        public override long Compute()
        {
            LinkedList<char> str = new LinkedList<char>("3113322113");

            for (int round = 0; round < 50; round++)
            {
                str = SayString(str);
            }

            return str.Count;
        }
    }
}
