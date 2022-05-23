namespace AdventOfCode._2015
{
    internal class Day4
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        string GetHash(string str)
        {
            byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));

            return string.Join(null, hash.Select(b => b.ToString("x2")));
        }

        public long Compute()
        {
            string salt = "iwrupvqb";

            long intVal = 0;

            do
            {
                string hexHash = GetHash(salt + intVal.ToString());

                if (hexHash.StartsWith("000000"))
                    break;

                intVal++;
            }
            while (true);

            return intVal;
        }
    }
}
