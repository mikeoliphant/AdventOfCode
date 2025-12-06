
namespace AdventOfCode._2025
{
    internal class Day6 : Day
    {
        public override long Compute()
        {
            List<List<long>> numbers = new();

            var data = File.ReadLines(DataFile);

            foreach (string line in data.SkipLast(1))
            {
                List<long> row = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToLongs().ToList();

                numbers.Add(row);
            }

            List<string> ops = data.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            long tot = 0;

            for (int i = 0; i < ops.Count; i++)
            {
                long result = (ops[i] == "+") ? 0 : 1;

                foreach (var row in numbers)
                {
                    if (ops[i] == "+")
                    {
                        result += row[i];
                    }
                    else
                    {
                        result *= row[i];
                    }
                }

                tot += result;
            }

            return tot;
        }

        public override long Compute2()
        {
            Grid<char> grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            Stack<long> numStack = new();

            long tot = 0;

            for (int col = grid.Width - 1; col >= 0; col--)
            {
                var colData = grid.GetColValues(col);

                var str = new string(colData.SkipLast(1).ToArray()).Trim();

                if (string.IsNullOrEmpty(str))
                    continue;

                long value = long.Parse(str);

                char op = colData.Last();

                numStack.Push(value);

                if (op != ' ')
                {
                    long result = (op == '+') ? 0 : 1;

                    while (numStack.Count > 0)
                    {
                        if (op == '+')
                        {
                            result += numStack.Pop();
                        }
                        else
                        {
                            result *= numStack.Pop();
                        }
                    }

                    tot += result;
                }
            }

            return tot;
        }
    }
}
