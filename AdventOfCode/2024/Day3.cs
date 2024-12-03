namespace AdventOfCode._2024
{
    internal class Day3 : Day
    {
        long DoMult(string str)
        {
            long tot = 0;

            var matches = Regex.Matches(str, @"mul\((\d+),(\d+)\)");

            foreach (Match match in matches)
            {
                tot += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }

            return tot;
        }

        public override long Compute()
        {
            //long mult = DoMult(@"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))");

            long mult = DoMult(File.ReadAllText(DataFile));

            return mult;
        }

        IEnumerable<string> GetInstructions(string str)
        {
            var matches = Regex.Matches(str, @"mul\(\d+,\d+\)|do\(\)|don't\(\)");

            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }

        public override long Compute2()
        {
            //var instructions = GetInstructions(@"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))");
            var instructions = GetInstructions(File.ReadAllText(DataFile));

            long tot = 0;

            bool doMult = true;

            foreach (string instruction in instructions)
            {
                if (instruction == "do()")
                {
                    doMult = true;
                }
                else if (instruction == "don't()")
                {
                    doMult = false;
                }
                else
                {
                    if (doMult)
                    {
                        var match =Regex.Match(instruction, @"mul\((\d+),(\d+)\)");

                        tot += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
                    }
                }
            }

            return tot;
        }
    }
}
