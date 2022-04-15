using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day2
    {
        public long Compute()
        {
            int numValid = 0;

            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day2.txt"))
            {
                string[] splitStr = Regex.Split(line, "[^a-zA-Z0-9]+");

                int minLetters = int.Parse(splitStr[0]);
                int maxLetters = int.Parse(splitStr[1]);
                char letter = splitStr[2][0];
                string pswd = splitStr[3];

                int numChars = 0;

                foreach (char c in pswd)
                {
                    if (c == letter)
                        numChars++;
                }

                if ((numChars >= minLetters) && (numChars <= maxLetters))
                {
                    numValid++;
                }
            }

            return numValid;
        }

        public long Compute2()
        {
            int numValid = 0;

            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day2.txt"))
            {
                string[] splitStr = Regex.Split(line, "[^a-zA-Z0-9]+");

                int pos1 = int.Parse(splitStr[0]);
                int pos2 = int.Parse(splitStr[1]);
                char letter = splitStr[2][0];
                string pswd = splitStr[3];

                int match = 0;

                if (pswd[pos1 - 1] == letter) match++;
                if (pswd[pos2 - 1] == letter) match++;

                if (match == 1)
                    numValid++;
            }

            return numValid;
        }
    }
}
