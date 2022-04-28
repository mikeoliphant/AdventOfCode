namespace AdventOfCode._2018
{
    internal class Day23
    {
        class Nanobot
        {
            public Vector3 Position { get; set; }
            public float Range { get; set; }

            public override string ToString()
            {
                return Position.ToString() + " " + Range.ToString();
            }
        }

        List<Nanobot> bots = new List<Nanobot>();

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day23.txt"))
            {
                Vector3 pos = new Vector3(Regex.Match(line, "<(.*)>").Groups[1].Value.ToFloats(',').ToArray());

                float range = float.Parse(Regex.Match(line, "r=(.*)").Groups[1].Value);

                bots.Add(new Nanobot {  Position = pos, Range = range });
            }
        }

        public long Compute()
        {
            ReadInput();

            bots.Sort((a, b) => b.Range.CompareTo(a.Range));

            Nanobot strongest = bots[0];

            return bots.Where(b => b.Position.ManhattanDistance(strongest.Position) <= strongest.Range).Count();
        }

        public long Compute2()
        {
            ReadInput();

            //Dictionary<Vector3, int> candidatePoints = new Dictionary<Vector3, int>();

            //for (int pos1 = 0; pos1 < bots.Count; pos1++)
            //{
            //    for (int pos2 = pos1 + 1; pos2 < bots.Count; pos2++)
            //    {
            //        if (bots[pos1].Position.ManhattanDistance(bots[pos2].Position) == (bots[pos1].Range + bots[pos2].Range))
            //        {
            //            Vector3 pos = (bots[pos1].Position + bots[pos2].Position) / 2;

            //            candidatePoints[pos] = bots.Where(b => b.Position.ManhattanDistance(pos) <= b.Range).Count();
            //        }
            //    }
            //}

            //Vector3 bestPos = candidatePoints.OrderByDescending(p => p.Value).First().Key;


            Dictionary<Nanobot, int> inRangeOf = new Dictionary<Nanobot, int>();

            foreach (Nanobot bot in bots)
            {
                inRangeOf[bot] = bots.Where(b => b.Position.ManhattanDistance(bot.Position) <= bot.Range).Count();
            }

            var sorted = inRangeOf.OrderBy(r => r.Value).ToArray();

            return 0;
        }
    }
}
