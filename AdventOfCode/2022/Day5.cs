namespace AdventOfCode._2022
{
    internal class Day5 : Day
    {
        public override long Compute()
        {
            int numStacks = 9;

            var sections = File.ReadAllText(DataFile).SplitParagraphs();

            var initialState = sections[0].SplitLines().SkipLast(1).Reverse();
            var moves = sections[1].SplitLines();

            var stacks = new Stack<char>[numStacks];

            for (int stackNum = 0; stackNum < numStacks; stackNum++)
            {
                stacks[stackNum] = new Stack<char>();
            }

            foreach (string stateStr in initialState)
            {
                for (int stackNum = 0; stackNum < numStacks; stackNum++)
                {
                    char c = stateStr[(stackNum * 4) + 1];

                    if (c != ' ')
                        stacks[stackNum].Push(c);
                }
            }

            foreach (string move in moves)
            {
                var match = Regex.Match(move, "move (.*) from (.*) to (.*)");

                if (!match.Success)
                {
                    throw new Exception();
                }

                for (int toMove = 0; toMove < int.Parse(match.Groups[1].Value); toMove++)
                {
                    char c = stacks[int.Parse(match.Groups[2].Value) - 1].Pop();

                    stacks[int.Parse(match.Groups[3].Value) - 1].Push(c);
                }
            }

            string tops = "";

            foreach (var stack in stacks)
            {
                tops += stack.Peek();
            }

            return base.Compute();
        }

        public override long Compute2()
        {
            int numStacks = 9;

            var sections = File.ReadAllText(DataFile).SplitParagraphs();

            var initialState = sections[0].SplitLines().SkipLast(1);
            var moves = sections[1].SplitLines();

            var stacks = new List<char>[numStacks];

            for (int stackNum = 0; stackNum < numStacks; stackNum++)
            {
                stacks[stackNum] = new List<char>();
            }

            foreach (string stateStr in initialState)
            {
                for (int stackNum = 0; stackNum < numStacks; stackNum++)
                {
                    char c = stateStr[(stackNum * 4) + 1];

                    if (c != ' ')
                        stacks[stackNum].Add(c);
                }
            }

            foreach (string move in moves)
            {
                var match = Regex.Match(move, "move (.*) from (.*) to (.*)");

                if (!match.Success)
                {
                    throw new Exception();
                }

                int toMove = int.Parse(match.Groups[1].Value);

                var crates = stacks[int.Parse(match.Groups[2].Value) - 1].GetRange(0, toMove);

                stacks[int.Parse(match.Groups[2].Value) - 1].RemoveRange(0, toMove);

                stacks[int.Parse(match.Groups[3].Value) - 1].InsertRange(0, crates);
            }

            string tops = "";

            foreach (var stack in stacks)
            {
                tops += stack[0];
            }

            return base.Compute();
        }
    }
}
