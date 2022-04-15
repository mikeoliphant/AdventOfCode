namespace AdventOfCode._2021
{
    internal class Day24
    {
        enum EALUVar
        {
            W,
            X,
            Y,
            Z
        }

        int runDigits;
        string[] lines = null;

        bool RunProgram(string inputStr)
        {
            long[] variables = new long[4];

            List<long> input = new List<long>();

            foreach (char c in inputStr)
            {
                input.Add(c - '0');
            }

            int inputPos = 0;

            if (lines == null)
            {
                lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day24.txt").SkipLast(18 * (14 - runDigits)).ToArray();
            }

            foreach (string cmdStr in lines)
            {
                string[] split = cmdStr.Split(' ');

                EALUVar var1 = EALUVar.W;
                EALUVar var2 = EALUVar.W;

                Enum.TryParse(split[1], ignoreCase: true, out var1);

                bool isVal = true;
                long val2 = 0;

                if (split.Length > 2)
                {
                    if (!long.TryParse(split[2], out val2))
                    {
                        isVal = false;

                        if (!Enum.TryParse<EALUVar>(split[2], ignoreCase: true, out var2))
                        {
                            throw new Exception();
                        }
                    }

                    if (!isVal)
                    {
                        val2 = variables[(int)var2];
                    }
                }

                switch (split[0])
                {
                    case "inp":
                        variables[(int)var1] = input[inputPos];
                        inputPos++;
                        break;

                    case "add":
                        variables[(int)var1] += val2;
                        break;

                    case "mul":
                        variables[(int)var1] *= val2;
                        break;

                    case "div":
                        variables[(int)var1] /= val2;
                        break;

                    case "mod":
                        variables[(int)var1] %= val2;
                        break;

                    case "eql":
                        variables[(int)var1] = (variables[(int)var1] == val2) ? 1 : 0;
                        break;
                }
            }

            // Check for conditions where x==0
            if (variables[1] == 0)
            {

            }
            else
            {

            }

            return (variables[3] == 0);
        }

        long maxNum = 0;
        long minNum = long.MaxValue;

        void RunCombos(string inputStr)
        {
            if (!inputStr.Contains('*'))
            {
                if (RunProgram(inputStr))
                {
                    maxNum = Math.Max(maxNum, long.Parse(inputStr));
                    minNum = Math.Min(minNum, long.Parse(inputStr));
                }
            }

            for (int pos = 0; pos < inputStr.Length; pos++)
            {
                if (inputStr[pos] == '*')
                {
                    foreach (int i in Enumerable.Range(1, 9))
                    {
                        RunCombos(inputStr.Substring(0, pos) + i + inputStr.Substring(pos + 1, inputStr.Length - pos - 1));
                    }

                    return;
                }
            }
        }

        public long Compute()
        {
            string inputStr = "211*1861151**1";
            //string inputStr = "****";
            runDigits = inputStr.Length;

            RunCombos(inputStr);


            foreach (string p1 in new string[] { "18" }) // "18", "29" })
            {
                foreach (string p2 in new string[] { "61" })// "61", "72", "83", "94" })
                {
                    foreach (string p3 in new string[] { "51" })// "73", "84", "95", "51", "62" })
                    {
                        inputStr = "****" + p1 + p2 + "1" + p3 + "***";

                        runDigits = inputStr.Length;

                        RunCombos(inputStr);
                    }
                }
            }


            return minNum;
        }
    }
}
