namespace AdventOfCode._2022
{
    internal class Day1 : Day
    {
        public override long Compute()
        {
            var sums = from elf in File.ReadAllText(DataFile).SplitParagraphs() select elf.SplitLines().ToInts().Sum();

            return sums.Max();
        }

        public override long Compute2()
        {
            var sums = from elf in File.ReadAllText(DataFile).SplitParagraphs() select elf.SplitLines().ToInts().Sum();

            return sums.OrderByDescending(elf => elf).Take(3).Sum();
        }
    }
}
