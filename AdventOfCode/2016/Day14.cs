namespace AdventOfCode._2016
{
    internal class Day14
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        Dictionary<(int Pos, char Dupe), bool> threeDupes = new Dictionary<(int Pos, char Dupe), bool>();
        Dictionary<(int Pos, char Dupe), bool> fiveDupes = new Dictionary<(int Pos, char Dupe), bool>();

        int hashInt = 0;

        void CalcDupes(string str)
        {
            bool haveThree = false;

            for (int i = 0; i < str.Length - 2; i++)
            {
                char dupeChar = str[i];

                int dupe = 1;

                for (; dupe < 5; dupe++)
                {
                    if ((i + dupe >= str.Length) || (str[i + dupe] != dupeChar))
                    {
                        break;
                    }
                }

                if (dupe > 2)
                {
                    if (!haveThree)
                    {
                        threeDupes[(hashInt, dupeChar)] = true;

                        haveThree = true;
                    }

                    if (dupe == 5)
                    {
                        fiveDupes[(hashInt, dupeChar)] = true;
                    }
                }
            }
        }

        bool HasDupes(string str, int numDupes, char dupeChar)
        {
            for (int i = 0; i < str.Length - numDupes; i++)
            {
                bool isDupe = true;

                for (int dupe = 0; dupe < numDupes; dupe++)
                {
                    if (str[i + dupe] != dupeChar)
                    {
                        isDupe = false;

                        break;
                    }
                }

                if (isDupe)
                    return true;
            }

            return false;
        }

        string GetHash(string str)
        {
            byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));

            return string.Join(null, hash.Select(b => b.ToString("x2")));
        }

        public long Compute()
        {
            //string salt = "abc";
            string salt = "ngcjuoqr";

            int numKeys = 0;

            do
            {
                string hashStr = GetHash(salt + hashInt);

                for (int i = 0; i < 2016; i++)
                {
                    hashStr = GetHash(hashStr);
                }

                CalcDupes(hashStr);

                hashInt++;
            }
            while (hashInt <= 25000);

            foreach (var threeDupe in threeDupes.OrderBy(d => d.Key.Pos))
            {
                foreach (var fiveDupe in fiveDupes.OrderBy(d => d.Key.Pos))
                {
                    if (fiveDupe.Key.Pos <= threeDupe.Key.Pos)
                        continue;

                    if ((threeDupe.Key.Dupe == fiveDupe.Key.Dupe) && (fiveDupe.Key.Pos < (threeDupe.Key.Pos + 1000)))
                    {
                        numKeys++;

                        if (numKeys == 64)
                        {
                            return threeDupe.Key.Pos;
                        }

                        break;
                    }
                }
            }

            throw new InvalidOperationException();
        }
    }
}
