using System.Diagnostics.Eventing.Reader;

namespace AdventOfCode._2024
{
    internal class Day24 : Day
    {
        static Dictionary<string, List<Gate>> connections = new();
        static List<Gate> gates = new();
        static Dictionary<string, bool> wireValues = new();
        static List<(string, bool)> initialWireValues = new();

        static List<Gate> GetConnections(string wire)
        {
            if (!connections.ContainsKey(wire))
            {
                connections[wire] = new List<Gate>();
            }

            return connections[wire];
        }

        static void SetWire(string wire, bool value)
        {
            wireValues[wire] = value;

            if (connections.ContainsKey(wire))
            {
                foreach (Gate gate in connections[wire])
                {
                    gate.AddInput(value);
                }
            }
        }

        class Gate
        {
            public enum EOpCode
            {
                AND,
                OR,
                XOR
            }

            public string OutputWire { get; set; }
            public EOpCode OpCode { get; set; }
            protected bool[] inputs = new bool[2];
            int haveInputs = 0;

            public void AddInput(bool input)
            {
                inputs[haveInputs++] = input;

                if (haveInputs == 2)
                {
                    GenerateOutput();
                }
            }

            public void Reset()
            {
                haveInputs = 0;
            }

            protected virtual void GenerateOutput()
            {
                bool output = false;

                switch (OpCode)
                {
                    case EOpCode.AND:
                        output = inputs[0] && inputs[1];
                        break;
                    case EOpCode.OR:
                        output = inputs[0] || inputs[1];
                        break;
                    case EOpCode.XOR:
                        output = inputs[0] ^ inputs[1];
                        break;
                }

                SetWire(OutputWire, output);
            }
        }

        void ReadData()
        {
            string[] split = File.ReadAllText(DataFile).SplitParagraphs();

            foreach (string line in split[1].SplitLines())
            {
                var match = Regex.Match(line, @"(.*) (.*) (.*) -> (.*)");

                string input1 = match.Groups[1].Value;
                string input2 = match.Groups[3].Value;
                string op = match.Groups[2].Value;
                string output = match.Groups[4].Value;

                Gate gate = new Gate()
                {
                    OpCode = Enum.Parse<Gate.EOpCode>(op),
                    OutputWire = output
                };

                gates.Add(gate);

                GetConnections(input1).Add(gate);
                GetConnections(input2).Add(gate);
            }

            foreach (string line in split[0].SplitLines())
            {
                string[] wireVal = line.Split(": ");

                initialWireValues.Add((wireVal[0], int.Parse(wireVal[1]) == 1));
            }
        }

        public override long Compute()
        {
            ReadData();

            foreach (var val in initialWireValues)
            {
                SetWire(val.Item1, val.Item2);
            }

            var bits = new string(wireValues.Where(w => w.Key.StartsWith('z')).OrderByDescending(w => w.Key).Select(w => w.Value ? '1' : '0').ToArray());

            long value = Convert.ToInt64(bits, 2);

            return value;
        }

        void Reset()
        {
            wireValues.Clear();

            foreach (Gate gate in gates)
            {
                gate.Reset();
            }
        }

        long Run(long num1, long num2)
        {
            Reset();

            for (int x = 44; x >= 0; x--)
            {
                SetWire("x" + x.ToString("00"), (num1 & (1 << x)) != 0); 
            }

            for (int y = 44; y >= 0; y--)
            {
                SetWire("y" + y.ToString("00"), (num1 & (1 << y)) != 0);
            }

            var bits = new string(wireValues.Where(w => w.Key.StartsWith('z')).OrderByDescending(w => w.Key).Select(w => w.Value ? '1' : '0').ToArray());

            return Convert.ToInt64(bits, 2);
        }

        long Factorial(long value)
        {
            if (value == 1)
                return 1;

            return value * Factorial(value - 1);
        }

        public long NChooseR(int n, int r)
        {
            return FactorialDivision(n, n - r) / Factorial(r);
        }

        private long FactorialDivision(long topFactorial, long divisorFactorial)
        {
            long result = 1;

            for (long i = topFactorial; i > divisorFactorial; i--)
                result *= i;

            return result;
        }

        IEnumerable<string> Trace(string startWire)
        {
            yield return startWire;

            foreach (Gate gate in GetConnections(startWire))
            {
                foreach (string wire in Trace(gate.OutputWire))
                {
                    yield return wire;
                }
            }
        }

        Random random = new Random();

        int TestBits()
        {
            long correctMask = (1L << 46) - 1;

            int num = Convert.ToString(correctMask, 2).Where(c => c == '1').Count();

            for (int i = 0; i < 100; i++)
            {
                long num1 = (long)(random.NextDouble() * (1L << 45));
                long num2 = (long)(random.NextDouble() * (1L << 45));

                long calculated = Run(num1, num2);

                Console.WriteLine(Convert.ToString(calculated, 2).PadLeft(46, '0'));
                Console.WriteLine(Convert.ToString(num1 + num2, 2).PadLeft(46, '0'));

                long wrong = (num1 + num2) & calculated;

                Console.WriteLine(Convert.ToString(wrong, 2).PadLeft(46, '0'));
                Console.WriteLine();

                correctMask &= ~wrong;

                if (correctMask == 0)
                    return 0;
            }

            return Convert.ToString(correctMask, 2).Where(c => c == '1').Count();
        }

        void Swap2(Gate gate1, Gate gate2)
        {
            string tmp = gate1.OutputWire;
            gate1.OutputWire = gate2.OutputWire;
            gate2.OutputWire = tmp;
        }

        public override long Compute2()
        {
            ReadData();

            Dictionary<string, int> counts = new();

            for (int i = 0; i < 45; i++)
            {
                var trace = Trace("x" + i.ToString("00"));

                foreach (string wire in trace)
                {
                    if (!counts.ContainsKey(wire))
                    {
                        counts[wire] = 1;
                    }
                    else
                    {
                        counts[wire]++;
                    }
                }
            }

            var gatesByWire = gates.ToDictionary(g => g.OutputWire, g => g);

            var sorted = counts.OrderByDescending(c => c.Value).Select(c => (c.Key, c.Value)).ToList();

            Swap2(gatesByWire["fps"], gatesByWire["mvw"]);
            Swap2(gatesByWire["jqp"], gatesByWire["jfb"]);

            var path = Trace("x00").ToList();

            int bits = TestBits();

            for (int g1 = 0; g1 < sorted.Count; g1++)
            {
                for (int g2 = g1 + 1; g2 < sorted.Count; g2++)
                {
                    Swap2(gatesByWire[sorted[g1].Key], gatesByWire[sorted[g2].Key]);

                    if (TestBits() > 0)
                    {

                    }

                    Swap2(gatesByWire[sorted[g1].Key], gatesByWire[sorted[g2].Key]);
                }
            }

            return 0;
        }
    }
}

