namespace AdventOfCode._2018
{
    internal class Day14
    {
        public long Compute()
        {
            LinkedList<int> list = new LinkedList<int>();

            list.AddLast(3);
            list.AddLast(7);

            var elf1 = list.First;
            var elf2 = list.First.Next;

            int recipeOffset = 084601;

            do
            {
                int sum = elf1.Value + elf2.Value;

                if (sum >= 10)
                {
                    list.AddLast(sum / 10);
                }

                list.AddLast(sum % 10);

                elf1 = elf1.MoveCircular(elf1.Value + 1);
                elf2 = elf2.MoveCircular(elf2.Value + 1);
            }
            while (list.Count < (recipeOffset + 10));

            string lastTen = string.Join("", list.Skip(recipeOffset).Take(10));

            return long.Parse(lastTen);
        }

        bool ListContains(LinkedList<int> list, int[] sequence)
        {
            var node = list.Last;

            for (int i = 0; i < sequence.Length + 1; i++)
            {
                node = node.Previous;

                if (node == null)
                    return false;
            }

            do
            {
                var seq = node;

                int i = 0;

                for (; i < sequence.Length; i++)
                {
                    if (node == null)
                        return false;

                    if (node.Value != sequence[i])
                        break;

                    node = node.Next;
                }

                if (i == sequence.Length)
                    return true;

                node = node.Next;
            }
            while (true);
        }

        public long Compute2()
        {
            LinkedList<int> list = new LinkedList<int>();

            list.AddLast(3);
            list.AddLast(7);

            var elf1 = list.First;
            var elf2 = list.First.Next;

            //int[] sequence = { 5, 9, 4, 1, 4 };
            int[] sequence = { 0, 8, 4, 6, 0, 1 };

            do
            {
                int sum = elf1.Value + elf2.Value;

                if (sum >= 10)
                {
                    list.AddLast(sum / 10);
                }

                list.AddLast(sum % 10);

                elf1 = elf1.MoveCircular(elf1.Value + 1);
                elf2 = elf2.MoveCircular(elf2.Value + 1);
            }
            while (!ListContains(list, sequence));

            return 0;
        }
    }
}
