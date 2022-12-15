namespace AdventOfCode._2022
{
    internal class Day13 : Day
    {
        int Compare(string list1, string list2)
        {
            if ((list1[0] != '[') && (list2[0] != '['))
            {
                return int.Parse(list1).CompareTo(int.Parse(list2));
            }

            if (list1[0] == '[')
            {
                list1 = list1.Substring(1, list1.Length - 2);
            }

            if (list2[0] == '[')
            {
                list2 = list2.Substring(1, list2.Length - 2);
            }

            var split1 = ParseHelper.SplitTopLevel(list1, ',', '[', ']').ToArray();
            var split2 = ParseHelper.SplitTopLevel(list2, ',', '[', ']').ToArray();

            foreach (var pair in split1.Zip(split2))
            {
                int result = Compare(pair.First, pair.Second);

                if (result != 0)
                    return result;
            }

            return split1.Length.CompareTo(split2.Length);
        }


        public override long Compute()
        {
            int index = 1;

            int indexSum = 0;

            foreach (string pairStr in File.ReadAllText(DataFile).SplitParagraphs())
            {
                string[] pair = pairStr.SplitLines().ToArray();

                if (Compare(pair[0], pair[1]) < 0)
                {
                    indexSum += index;
                }

                index++;
            }

            return indexSum;
        }

        public override long Compute2()
        {
            var packets = File.ReadLines(DataFile).Where(p => !String.IsNullOrEmpty(p)).ToList();

            packets.Add("[[2]]");
            packets.Add("[[6]]");

            packets.Sort((a, b) => Compare(a, b));

            return (packets.IndexOf("[[2]]") + 1) * (packets.IndexOf("[[6]]") + 1);
        }
    }
}
