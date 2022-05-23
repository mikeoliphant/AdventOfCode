namespace AdventOfCode._2015
{
    internal class Day5
    {
        char[] vowels = "aeiou".ToCharArray();
        string[] badStrs = new string[] { "ab", "cd", "pq", "xy" };

        bool ContainsDouble(string str)
        {
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] == str[i + 1])
                    return true;
            }

            return false;
        }

        int NumVowels(string str)
        {
            int numVowels = 0;

            foreach (char c in str)
            {
                if (vowels.Contains(c))
                    numVowels++;
            }

            return numVowels;
        }

        bool ContainsBad(string str)
        {
            foreach (string bad in badStrs)
            {
                if (str.Contains(bad))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsNice(string str)
        {
            return ContainsDouble(str) && (NumVowels(str) >= 3) && !ContainsBad(str);
        }

        public long Compute()
        {
            int numNice = 0;

            foreach (string str in File.ReadLines(@"C:\Code\AdventOfCode\Input\2015\Day5.txt"))
            {
                if (IsNice(str))
                {
                    numNice++;
                }
            }

            return numNice;
        }

        bool IsNice2(string str)
        {
            if (!Regex.IsMatch(str, @"(.{2}).*\1"))
            {
                return false;
            }

            if (!Regex.IsMatch(str, @"(\w)\w\1"))
                return false;

            return true;
        }

        public long Compute2()
        {
            int numNice = 0;

            IsNice2("qjhvhtzxzqqjkmpb");

            foreach (string str in File.ReadLines(@"C:\Code\AdventOfCode\Input\2015\Day5.txt"))
            {
                if (IsNice2(str))
                {
                    numNice++;
                }
            }

            return numNice;
        }
    }
}
