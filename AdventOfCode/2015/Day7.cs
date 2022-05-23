namespace AdventOfCode._2015
{
    internal class Day7 : Day
    {
        static Dictionary<string, Wire> wires = new Dictionary<string, Wire>();

        class Wire
        {
            public string Name { get; set; }
            public List<Wire> Feeds { get; set; } = new List<Wire>();
            public string Op { get; set; }
            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
            public long? Value { get; set; }

            public IEnumerable<string> PullsFrom()
            {
                if ((Arg1 != null) && char.IsLetter(Arg1[0]))
                {
                    yield return Arg1;
                }

                if ((Arg2 != null) && char.IsLetter(Arg2[0]))
                {
                    yield return Arg2;
                }
            }

            public bool TryComputeValue()
            {
                long val1;
                long val2;
                
                switch (Op)
                {
                    case null:
                        if (TryGetValue(Arg1, out val1))
                        {
                            Value = val1;

                            return true;
                        }
                        break;

                    case "NOT":
                        if (TryGetValue(Arg1, out val1))
                        {
                            Value = ~val1;

                            return true;
                        }
                        break;

                    case "AND":
                        if (TryGetValue(Arg1, out val1) && TryGetValue(Arg2, out val2))
                        {
                            Value = val1 & val2;

                            return true;
                        }
                        break;

                    case "OR":
                        if (TryGetValue(Arg1, out val1) && TryGetValue(Arg2, out val2))
                        {
                            Value = val1 | val2;

                            return true;
                        }
                        break;

                    case "LSHIFT":
                        if (TryGetValue(Arg1, out val1) && TryGetValue(Arg2, out val2))
                        {
                            Value = val1 << (int)val2;

                            return true;
                        }
                        break;

                    case "RSHIFT":
                        if (TryGetValue(Arg1, out val1) && TryGetValue(Arg2, out val2))
                        {
                            Value = val1 >> (int)val2;

                            return true;
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                return false;
            }

            public override string ToString()
            {
                return Name + ": " + Op + " " + Arg1 + " " + Arg2;
            }
        }

        static bool TryGetValue(string arg, out long value)
        {
            value = 0;

            if (char.IsDigit(arg[0]))
            {
                value = long.Parse(arg);

                return true;
            }

            Wire wire = GetWire(arg);

            if (!wire.Value.HasValue)
                return false;

            value = wire.Value.Value;

            return true;
        }

        static Wire GetWire(string name)
        {
            if (!wires.ContainsKey(name))
            {
                wires[name] = new Wire { Name = name };
            }

            return wires[name];
        }

        public override long Compute()
        {
            HashSet<Wire> ready = new HashSet<Wire>();

            foreach (string connection in File.ReadLines(DataFileTest))
            {
                string[] leftRight = connection.Split(" -> ");

                Wire destWire = GetWire(leftRight[1]);

                string[] op = leftRight[0].Split(' ');

                if (op.Length == 1)
                {
                    destWire.Arg1 = op[0];
                }
                else if (op.Length == 2)
                {
                    destWire.Op = op[0];
                    destWire.Arg1 = op[1];
                }
                else if (op.Length == 3)
                {
                    destWire.Op = op[1];

                    destWire.Arg1 = op[0];
                    destWire.Arg2 = op[2];
                }
                else throw new InvalidOperationException();

                if (destWire.TryComputeValue())
                {
                    ready.Add(destWire);
                }

                foreach (string pull in destWire.PullsFrom())
                {
                    GetWire(pull).Feeds.Add(destWire);
                }
            }

            HashSet<Wire> newready = new HashSet<Wire>();

            while (ready.Count > 0)
            {
                foreach (Wire wire in ready)
                {
                    if (wire.TryComputeValue())
                    {
                        foreach (Wire feed in wire.Feeds)
                        {
                            newready.Add(feed);
                        }
                    }
                }

                var tmp = ready;
                ready = newready;
                newready = tmp;

                newready.Clear();
            }

            return GetWire("a").Value.Value;
        }
    }
}
