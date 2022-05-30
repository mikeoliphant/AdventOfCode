namespace AdventOfCode._2015
{
    internal class Day11 : Day
    {
        bool IsValid(char[] password)
        {
            bool haveSeq3 = false;

            int seqLen = 1;
            char lastChar = password[0];

            int numDubs = 0;
            char lastDubChar = '\0';

            for (int i = 1; i < password.Length; i++)
            {
                char c = password[i];

                if ((c == 'i') || (c == 'o') || (c == 'l'))
                    return false;

                if (!haveSeq3)
                {
                    if (c == (lastChar + 1))
                    {
                        seqLen++;

                        if (seqLen == 3)
                        {
                            haveSeq3 = true;
                        }
                    }
                    else
                    {
                        seqLen = 1;
                    }
                }

                if ((numDubs < 2) && (c != lastDubChar))
                {
                    if (c == lastChar)
                    {
                        numDubs++;

                        lastDubChar = c;
                    }
                }

                if (haveSeq3 && (numDubs == 2))
                {
                    return true;
                }

                lastChar = password[i];
            }

            return false;
        }

        void Inc(char[] password, int pos)
        {
            char c = (char)(password[pos] + 1);

            if ((c > 'z'))
            {
                password[pos] = 'a';

                Inc(password, pos - 1);

                return;
            }

            password[pos] = c;
        }

        public override long Compute()
        {
            char[] password = "cqjxxyzz".ToArray();

            do
            {
                Inc(password, 7);
            }
            while (!IsValid(password));

            return 0;
        }
    }
}
