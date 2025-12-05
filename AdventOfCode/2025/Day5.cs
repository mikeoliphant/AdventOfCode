
namespace AdventOfCode._2025
{
    internal class Day5 : Day
    {
        List<Range> fresh;
        List<long> available;

        void ReadData()
        {
            var split = File.ReadAllText(DataFile).SplitParagraphs();

            fresh = ParseRanges(split[0].SplitLines());

            available = split[1].SplitLines().ToLongs().ToList();
        }

        List<Range> ParseRanges(IEnumerable<string> lines)
        {
            List<Range> ranges = new();

            foreach (string line in lines)
            {
                ranges.Add(Range.FromString(line));
            }

            return ranges;
        }

        public override long Compute()
        {
            ReadData();

            int numAvailable = 0;

            foreach (long id in available)
            {
                foreach (Range range in fresh)
                {
                    if (range.Contains(id))
                    {
                        numAvailable++;

                        break;
                    }
                }
            }

            return numAvailable;
        }

        public override long Compute2()
        {
            ReadData();

            List<Range> finalRanges = new();

            foreach (Range r in fresh)
            {
                List<Range> set = new List<Range>();

                set.Add(r);

                foreach (Range f in finalRanges)
                {
                    List<Range> newSet = new();

                    foreach (Range s in set)
                    {
                        foreach (Range d in s.GetDisjointFrom(f))
                        {
                            newSet.Add(d);
                        }
                    }

                    if (!set.Any())
                        break;

                    set = newSet;
                }

                finalRanges.AddRange(set);
            }

            for (int r1 = 0; r1 < finalRanges.Count; r1++)
            {
                for (int r2 = r1 + 1; r2 < finalRanges.Count; r2++)
                {
                    if (finalRanges[r1].Intersects(finalRanges[r2]))
                    {

                    }
                }
            }


            long tot = finalRanges.Select(r => r.Size()).Sum();

            return tot;
        }
    }
}
