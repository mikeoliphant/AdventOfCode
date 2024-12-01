namespace AdventOfCode._2023
{
    internal class Day19 : Day
    {
        public override long Compute()
        {
            string[] sections = File.ReadAllText(DataFileTest).SplitParagraphs();

            foreach (string rule in sections[0].SplitLines())
            {
                var match = Regex.Match(rule, @"^(.*)\{(.*)\}$");

                string name = match.Groups[1].Value;

                var ruleMatch = Regex.Match(match.Groups[2].Value, "[(.*)[<>](.*):(.*)");
            }

            return base.Compute();
        }
    }
}
