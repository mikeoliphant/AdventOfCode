namespace AdventOfCode._2016
{
    internal class Day15
    {
        int numDiscs;
        int[] discPos = null;
        int[] discSize = null;

        public long Compute()
        {
            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day15Part2.txt").ToArray();

            numDiscs = lines.Length;
            discPos = new int[numDiscs];
            discSize = new int[numDiscs];

            for (int disc = 0; disc < numDiscs; disc++)
            {
                var match = Regex.Match(lines[disc], "^Disc .* has (.*) positions; at time=0, it is at position (.*).$");

                discSize[disc] = int.Parse(match.Groups[1].Value);
                discPos[disc] = int.Parse(match.Groups[2].Value);
            }

            //int time = 0;
            //bool aligned = true;

            //do
            //{
            //    for (int disc = 0; disc < numDiscs; disc++)
            //    {
            //        discPos[disc] = (discPos[disc] + 1) % discSize[disc];
            //    }

            //    time++;

            //    aligned = true;

            //    int desiredPosition = 0;

            //    for (int disc = numDiscs - 1; disc >= 0; disc--)
            //    {
            //        if (discPos[disc] != (desiredPosition % discSize[disc]))
            //        {
            //            aligned = false;

            //            break;
            //        }

            //        desiredPosition++;
            //    }
            //}
            //while (!aligned);

            //return time - numDiscs;

            return ModHelper.Align(Enumerable.Range(0, numDiscs).Select(d => ((long)discSize[d], (long)discPos[d], (long)ModHelper.PosMod(0 - (d + 1), discSize[d]))));
        }
    }
}
