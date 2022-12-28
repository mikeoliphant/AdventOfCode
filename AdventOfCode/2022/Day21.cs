using OpenTK.Graphics.ES11;

namespace AdventOfCode._2022
{
    internal class Day21 : Day
    {
        class Monkey
        {
            public string Name { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
            public string Op { get; set; }

            public static Dictionary<string, Monkey> Monkeys = new Dictionary<string, Monkey>();

            public static Monkey GetMonkey(string name)
            {
                if (!Monkeys.ContainsKey(name))
                {
                    Monkeys[name] = new Monkey { Name = name };
                }

                return Monkeys[name];
            }

            public long GetNumber()
            {
                switch (Op)
                {
                    case "+":
                        return GetMonkey(Arg1).GetNumber() + GetMonkey(Arg2).GetNumber();
                    case "-":
                        return GetMonkey(Arg1).GetNumber() - GetMonkey(Arg2).GetNumber();
                    case "*":
                        return GetMonkey(Arg1).GetNumber() * GetMonkey(Arg2).GetNumber();
                    case "/":
                        return GetMonkey(Arg1).GetNumber() / GetMonkey(Arg2).GetNumber();

                    default:
                        return int.Parse(Op);
                }
            }

            public string GetNumberString()
            {
                switch (Op)
                {
                    case "+":
                        return "(" + GetMonkey(Arg1).GetNumberString() + " + " + GetMonkey(Arg2).GetNumberString() + ")";
                    case "-":
                        return "(" + GetMonkey(Arg1).GetNumberString() + " - " + GetMonkey(Arg2).GetNumberString() + ")";
                    case "*":
                        return "(" + GetMonkey(Arg1).GetNumberString() + " * " + GetMonkey(Arg2).GetNumberString() + ")";
                    case "/":
                        return "(" + GetMonkey(Arg1).GetNumberString() + " / " + GetMonkey(Arg2).GetNumberString() + ")";

                    default:
                        return Op;
                }
            }
        }

        void ReadInput(string file)
        {
            foreach (string line in File.ReadLines(file))
            {
                var match = Regex.Match(line, "(.*): (.*) (.*) (.*)");

                if (match.Success)
                {
                    Monkey monkey = Monkey.GetMonkey(match.Groups[1].Value);

                    monkey.Arg1 = match.Groups[2].Value;
                    monkey.Op = match.Groups[3].Value;
                    monkey.Arg2 = match.Groups[4].Value;
                }
                else
                {
                    match = Regex.Match(line, "(.*): (.*)");

                    if (!match.Success)
                        throw new Exception();

                    Monkey.GetMonkey(match.Groups[1].Value).Op = match.Groups[2].Value;
                }
            }
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            return Monkey.GetMonkey("root").GetNumber();
        }

        public override long Compute2()
        {
            ReadInput(DataFile);

            Monkey.GetMonkey("humn").Op = "X";

            Monkey root = Monkey.GetMonkey("root");

            Console.WriteLine(Monkey.GetMonkey(root.Arg1).GetNumberString() + "   =   " + Monkey.GetMonkey(root.Arg2).GetNumberString());

            // Paste resulting equation into an equation solver...

            return 0;
        }
    }
}
