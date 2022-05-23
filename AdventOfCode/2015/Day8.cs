namespace AdventOfCode._2015
{
    internal class Day8 : Day
    {
        public override long Compute()
        {
            long numEscaped = 0;
            long numUnescaped = 0;

            foreach (string str in File.ReadLines(DataFile))
            {
                string unescaped = Regex.Unescape(str.Substring(1, str.Length - 2));

                numEscaped += str.Length;
                numUnescaped += unescaped.Length;
            }

            return numEscaped - numUnescaped;
        }

        string Escape(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("\"", "\\\"");

            return str;
        }

        public override long Compute2()
        {
            long numEscaped = 0;
            long numDoubleEscaped = 0;

            foreach (string str in File.ReadLines(DataFile))
            {
                string doubleEscaped = "\"" + Escape(str) + "\"";

                numEscaped += str.Length;
                numDoubleEscaped += doubleEscaped.Length;
            }

            return numDoubleEscaped - numEscaped;
        }
    }
}
