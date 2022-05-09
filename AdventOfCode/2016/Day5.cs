namespace AdventOfCode._2016
{
    internal class Day5
    {
        public long Compute()
        {
            string doorID = "abc";
            //string doorID = "ugkcyxxp";

            string password = "";

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                int hashInt = 0;

                do
                {
                    byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(doorID + hashInt.ToString()));

                    //if (hashInt == 3231929)
                    //{

                    //}

                    if ((hash[0] == 0) && (hash[1] == 0) && ((hash[2] >> 4) == 0))
                    {
                        password += (hash[2] & 0x0f).ToString("x");
                    }

                    hashInt++;
                }
                while (password.Length < 8);               
            }

            return 0;
        }

        public long Compute2()
        {
            //string doorID = "abc";
            string doorID = "ugkcyxxp";

            Dictionary<int, char> passwordDict = new Dictionary<int, char>();

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                long hashInt = 0;

                do
                {
                    byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(doorID + hashInt.ToString()));

                    string hashStr = string.Join(null, hash.Select(b => b.ToString("x2")));

                    bool isFiveZeroes = true;

                    for (int i = 0; i < 5; i++)
                    {
                        if (hashStr[i] != '0')
                        {
                            isFiveZeroes = false;

                            break;
                        }
                    }

                    if (isFiveZeroes)
                    {
                        int pos = hashStr[5] - '0';

                        if (pos < 8)
                        {
                            if (!passwordDict.ContainsKey(pos))
                                passwordDict[pos] = hashStr[6];
                        }
                    }

                    hashInt++;
                }
                while (passwordDict.Count < 8);
            }

            string password = new string(passwordDict.OrderBy(p => p.Key).Select(p => p.Value).ToArray());

            return 0;
        }
    }
}
