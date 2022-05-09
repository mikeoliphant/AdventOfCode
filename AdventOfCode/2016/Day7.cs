namespace AdventOfCode._2016
{
    internal class Day7
    {

        public long Compute()
        {
            int numValid = 0;

            foreach (string ip in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day7.txt"))
            {
                bool inBrackets = false;

                bool abbaInBrackets = false;
                bool abbaOutsideBrackets = false;

                for (int i = 0; i < (ip.Length - 3); i++)
                {
                    if (ip[i] == '[')
                    {
                        inBrackets = true;
                    }
                    else if (ip[i] == ']')
                    {
                        inBrackets = false;
                    }

                    if ((ip[i] != ip[i + 1]) && (ip[i + 1] == ip[i + 2]) && (ip[i] == ip[i + 3]))
                    {
                        if (inBrackets)
                            abbaInBrackets = true;
                        else
                            abbaOutsideBrackets = true;
                    }
                }

                if (abbaOutsideBrackets && !abbaInBrackets)
                {
                    numValid++;
                }
            }

            return numValid;
        }

        public long Compute2()
        {
            int numValid = 0;

            foreach (string ip in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day7.txt"))
            {
                bool inBrackets = false;

                Dictionary<string, bool> inBracket = new Dictionary<string, bool>();
                Dictionary<string, bool> outBracket = new Dictionary<string, bool>();

                for (int i = 0; i < (ip.Length - 2); i++)
                {
                    if (ip[i] == '[')
                    {
                        inBrackets = true;
                    }
                    else if (ip[i] == ']')
                    {
                        inBrackets = false;
                    }

                    if ((ip[i] != ip[i + 1]) && (ip[i] == ip[i + 2]))
                    {
                        if (inBrackets)
                            inBracket[new string(new char[] { ip[i], ip[i + 1], ip[i] })] = true;
                        else
                            outBracket[new string(new char[] { ip[i], ip[i + 1], ip[i] })] = true;
                    }
                }

                foreach (string aba in outBracket.Keys)
                {
                    if (inBracket.ContainsKey(new string(new char[] { aba[1], aba[0], aba[1] })))
                    {
                        numValid++;

                        break;
                    }
                }
            }

            return numValid;
        }

    }
}
