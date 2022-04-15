namespace AdventOfCode._2020
{
    public class Day6
    {
        string[][] answers;

        void ReadInput()
        {
            string[] answerGroups = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day6.txt").Split("\n\n");

            answers = new string[answerGroups.Length][];

            for (int pos = 0; pos < answers.Length; pos++)
            {
                answers[pos] = answerGroups[pos].Split('\n');
            }
        }

        public long Compute()
        {
            ReadInput();

            int answerSum = 0;

            foreach (string[] group in answers)
            {
                Dictionary<char, bool> questionDict = new Dictionary<char, bool>();

                foreach (string person in group)
                {
                    foreach (char a in person)
                    {
                        questionDict[a] = true;
                    }
                }

                answerSum += questionDict.Values.Count;
            }

            return answerSum;
        }

        public long Compute2()
        {
            ReadInput();

            int answerSum = 0;

            foreach (string[] group in answers)
            {
                Dictionary<char, bool> questionDict = new Dictionary<char, bool>();

                foreach (string person in group)
                {
                    foreach (char a in person)
                    {
                        questionDict[a] = true;
                    }
                }

                int numAll = 0;

                foreach (char c in questionDict.Keys)
                {
                    bool haveAll = true;

                    foreach (string person in group)
                    {
                        if (string.IsNullOrEmpty(person))
                            continue;

                        if (!person.Contains(c))
                        {
                            haveAll = false;

                            break;
                        }
                    }

                    if (haveAll)
                    {
                        numAll++;
                    }
                }

                answerSum += numAll;
            }

            return answerSum;
        }
    }
}
