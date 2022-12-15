namespace AdventOfCode._2022
{
    internal class Day11 : Day
    {
        public List<Monkey> Monkeys = new List<Monkey>();

        public class Monkey
        {
            public List<long> Items { get; set; }
            public char Operation { get; set; }
            public int OperationAmount { get; set;  }
            public int DivisionTest { get; set;  }
            public int TrueMonkey { get; set; }
            public int FalseMonkey { get; set; }
            public long Inspections { get; set; }
        }

        public void ReadInput()
        {
            foreach (var monkeyStr in File.ReadAllText(DataFile).SplitParagraphs())
            {
                var match = Regex.Match(monkeyStr, "Starting items: (.*) Operation: new = old (.*) Test: divisible by (.*) If true: throw to monkey (.*) If false: throw to monkey (.*)", RegexOptions.Singleline);
                
                if (!match.Success)
                {
                    throw new Exception();
                }

                Monkey monkey = new Monkey();
                monkey.Items = match.Groups[1].Value.ToLongs(',').ToList();
                monkey.Operation = match.Groups[2].Value[0];

                if (!(match.Groups[2].Value[2] == 'o'))
                    monkey.OperationAmount = int.Parse(match.Groups[2].Value.Substring(2));
    
                monkey.DivisionTest = int.Parse(match.Groups[3].Value);
                monkey.TrueMonkey = int.Parse(match.Groups[4].Value);
                monkey.FalseMonkey = int.Parse(match.Groups[5].Value);

                Monkeys.Add(monkey);
            }
        }

        public override long Compute()
        {
            ReadInput();

            // Exploit modulo properties of least-common-multiple of sets prime numbers
            long primeCap = Monkeys.Select(m => m.DivisionTest).Aggregate(1, (x, y) => x * y);

            for (int round = 0; round < 10000; round++)
            {
                foreach (Monkey monkey in Monkeys)
                {
                    foreach (long item in monkey.Items)
                    {
                        monkey.Inspections++;

                        long newItem = 0;

                        if (monkey.Operation == '+')
                        {
                            if (monkey.OperationAmount == 0)
                            {
                                newItem = item + item;
                            }
                            else
                            {
                                newItem = item + monkey.OperationAmount;
                            }
                        }
                        else if (monkey.Operation == '*')
                        {
                            if (monkey.OperationAmount == 0)
                            {
                                newItem = item * item;
                            }
                            else
                            {
                                newItem = item * monkey.OperationAmount;
                            }
                        }
                        else
                            throw new Exception();

                        //newItem /= 3;

                        newItem %= primeCap;

                        if ((newItem % monkey.DivisionTest) == 0)
                        {
                            Monkeys[monkey.TrueMonkey].Items.Add(newItem);
                        }
                        else
                        {
                            Monkeys[monkey.FalseMonkey].Items.Add(newItem);
                        }
                    }

                    monkey.Items.Clear();
                }
            }

            var top2 = Monkeys.OrderByDescending(m => m.Inspections).Take(2).ToArray();

            return top2[0].Inspections * top2[1].Inspections;
        }
    }
}
