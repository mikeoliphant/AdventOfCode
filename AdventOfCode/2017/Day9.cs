namespace AdventOfCode._2017
{
    internal class Day9
    {
        int totScore = 0;
        int numGarbage = 0;

        int Score(ReadOnlySpan<char> data, int score)
        {
            int nesting = 0;
            int lastPos = 0;
            bool inGarbage = false;

            for (int pos = 0; pos < data.Length; pos++)
            {
                if (inGarbage)
                {
                    if (data[pos] == '>')
                    {
                        if (inGarbage)
                            inGarbage = false;
                    }
                    else
                    {
                        if (score == 0)
                            numGarbage++;
                    }
                }
                else
                {
                    if (data[pos] == '<')
                    {
                        if (!inGarbage)
                            inGarbage = true;
                    }
                    else if (data[pos] == '{')
                    {
                        if (nesting == 0)
                            lastPos = pos + 1;

                        nesting++;
                    }
                    else if (data[pos] == '}')
                    {
                        nesting--;

                        if (nesting == 0)
                        {
                            Score(data.Slice(lastPos, pos - lastPos), score + 1);
                        }
                    }
                }
            }

            totScore += score;

            return 0;
        }

        public long Compute()
        {
            string data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day9.txt").Trim();
            //string data = "<<<<>";

            data = Regex.Replace(data, "!.", "");

            Score(data.AsSpan(), 0);

            return numGarbage; // totScore;
        }
    }
}
