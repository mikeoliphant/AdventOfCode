namespace AdventOfCode._2016
{
    internal class Day9
    {
        string Decompress(string data)
        {
            string decompressed = "";

            bool inCmd = false;
            int cmdStart = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (!inCmd)
                {
                    if (data[i] == '(')
                    {
                        inCmd = true;

                        cmdStart = i + 1;
                    }
                    else
                    {
                        decompressed += data[i];
                    }
                }
                else
                {
                    if (data[i] == ')')
                    {
                        int[] vals = data.Substring(cmdStart, i - cmdStart).ToInts('x').ToArray();

                        i++;

                        string subStr = data.Substring(i, vals[0]);

                        for (int repeat = 0; repeat < vals[1]; repeat++)
                        {
                            decompressed += subStr;
                        }

                        i += vals[0] - 1;

                        inCmd = false;
                    }
                }
            }

            return decompressed;
        }

        public long Compute()
        {
            //string blah = Decompress("(6x1)(1x3)A");

            string data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2016\Day9.txt").Trim();

            return Decompress(data).Length;
        }

        class CmdStr
        {
            public int Repeats { get; set; } = 1;
            public string Terminal { get; set; }
            public List<CmdStr> Components { get; set; } = new List<CmdStr>();

            public override string ToString()
            {
                if (Terminal != null)
                    return Terminal;

                return String.Join("", Components.Select(c => c.ToString()));
            }

            public long GetSize()
            {
                if (Terminal != null)
                {
                    return Terminal.Length * Repeats;
                }

                return Repeats * Components.Sum(c => c.GetSize());
            }
        }

        CmdStr Parse(string data)
        {
            bool inCmd = false;
            int lastPos = 0;

            CmdStr cmd = new CmdStr();

            int i = 0;

            for (; i < data.Length; i++)
            {
                if (!inCmd)
                {
                    if (data[i] == '(')
                    {
                        if (i > lastPos)
                        {
                            cmd.Components.Add(Parse(data.Substring(lastPos, i - lastPos)));
                        }

                        inCmd = true;

                        lastPos = i + 1;
                    }
                }
                else
                {
                    if (data[i] == ')')
                    {
                        int[] vals = data.Substring(lastPos, i - lastPos).ToInts('x').ToArray();

                        i++;

                        string subStr = data.Substring(i, vals[0]);

                        CmdStr comp = Parse(subStr);
                        comp.Repeats = vals[1];

                        cmd.Components.Add(comp);

                        i += vals[0] - 1;

                        inCmd = false;

                        lastPos = i + 1;
                    }
                }
            }

            if (cmd.Components.Count == 0)  // We have a terminal string
            {
                cmd.Terminal = data;
            }
            else
            {
                if (i > lastPos)
                {
                    cmd.Components.Add(Parse(data.Substring(lastPos, i - lastPos)));
                }
            }

            return cmd;
        }

        public long Compute2()
        {
            //CmdStr blah = Parse("(27x12)(20x12)(13x14)(7x10)(1x12)A");

            string data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2016\Day9.txt").Trim();

            CmdStr parsed = Parse(data);

            return parsed.GetSize();
        }
    }
}
