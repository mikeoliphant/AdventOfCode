namespace AdventOfCode._2018
{
    internal class Day5
    {
        string Reduce(string polymer)
        {
            LinkedList<char> list = new LinkedList<char>(polymer);

            var current = list.First;

            while ((current != null) && (current.Next != null))
            {
                if (Math.Abs(current.Value - current.Next.Value) == 32)
                {
                    var next = current.Previous;

                    list.Remove(current.Next);
                    list.Remove(current);

                    current = next;
                    if (current == null)
                        current = list.First;
                }
                else
                {
                    current = current.Next;
                }
            }

            return new string(list.ToArray());
        }

        public long Compute()
        {
            //string polymer = "dabAcCaCBAcCcaDA";
            string polymer = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2018\Day5.txt").Trim();

            string reduced = Reduce(polymer);

            return reduced.Length;
        }

        public long Compute2()
        {
            //string polymer = "dabAcCaCBAcCcaDA";
            string polymer = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2018\Day5.txt").Trim();

            char minChar = '\0';
            int minLength = int.MaxValue;

            for (char c = 'a'; c <= 'z'; c++)
            {
                string removed = polymer.Replace(c.ToString(), "").Replace(char.ToUpper(c).ToString(), "");

                string reduced = Reduce(removed);

                if (reduced.Length < minLength)
                {
                    minLength = reduced.Length;
                    minChar = c;
                }
            }

            return minLength;
        }
    }
}
