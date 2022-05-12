namespace AdventOfCode._2016
{
    internal class Day19
    {
        LinkedList<int> elves = new LinkedList<int>();

        public long Compute()
        {
            int numElves = 3012210;

            for (int i = 0; i < numElves; i++)
            {
                elves.AddLast(i + 1);
            }

            var elf = elves.First;

            do
            {
                elves.Remove(elf.MoveCircular(1));

                elf = elf.MoveCircular(1);
            }
            while (elves.Count > 1);

            return elves.First();
        }

        public long Compute2()
        {
            int numElves = 3012210;

            for (int i = 0; i < numElves; i++)
            {
                elves.AddLast(i + 1);
            }

            var elf = elves.First;

            int count = numElves;

            var across = elf.MoveCircular(count / 2);

            do
            {
                var newAcross = across.MoveCircular(((count % 2) == 0) ? 1 : 2);

                elves.Remove(across);

                elf = elf.MoveCircular(1);

                count--;
                across = newAcross;
            }
            while (elves.Count > 1);

            return elves.First();
        }
    }
}
