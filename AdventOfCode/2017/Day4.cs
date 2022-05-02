namespace AdventOfCode._2017
{
    internal class Day4
    {
        public long Compute()
        {
            int numValid = 0;

            foreach (string phrase in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day4.txt"))
            {
                string[] words = phrase.Split(' ');

                Dictionary<string, bool> dict = new Dictionary<string, bool>();

                bool isValid = true;

                foreach (string word in words)
                {
                    if (dict.ContainsKey(word))
                    {
                        isValid = false;

                        break;
                    }

                    dict[word] = true;
                }

                if (isValid)
                    numValid++;
            }

            return numValid;
        }

        bool IsAnagram(string str1, string str2)
        {
            if (str1.Length != str2.Length)
                return false;

            Dictionary<char, int> dict = new Dictionary<char, int>();

            foreach (char c in str1)
            {
                if (!dict.ContainsKey(c))
                {
                    dict[c] = 1;
                }
                else
                {
                    dict[c]++;
                }
            }

            foreach (var letter in dict)
            {
                if (str2.Count(letter.Key) != letter.Value)
                    return false;
            }

            return true;
        }

        public long Compute2()
        {
            int numValid = 0;

            foreach (string phrase in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day4.txt"))
            {
                string[] words = phrase.Split(' ');

                bool isValid = true;

                for (int pos1 = 0; pos1 < words.Length; pos1++)
                {
                    for (int pos2 = pos1 + 1; pos2 < words.Length; pos2++)
                    {
                        if (IsAnagram(words[pos1], words[pos2]))
                        {
                            isValid = false;

                            break;
                        }
                    }
                }

                if (isValid)
                    numValid++;
            }

            return numValid;
        }
    }
}
