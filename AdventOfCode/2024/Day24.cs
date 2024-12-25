using System.Configuration;
using System.Diagnostics.Eventing.Reader;

namespace AdventOfCode._2024
{
    internal class Day24 : Day
    {
        static Dictionary<string, List<Gate>> connections = new();
        static List<Gate> gates = new();
        static Dictionary<string, bool> wireValues = new();
        static List<(string, bool)> initialWireValues = new();
        static Dictionary<string, Gate> gatesByWire;


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

                if ((haveInputs == 2) || (OpCode == EOpCode.OR))
                {
                    GenerateOutput();
                }
            }

            public void Reset()
            {
                haveInputs = 0;
            }

            protected void GenerateOutput()
            {
                bool output = false;

                switch (OpCode)
                {
                    case EOpCode.AND:
                        output = inputs[0] && inputs[1];
                        break;
                    case EOpCode.OR:
                        if ((haveInputs == 2) && inputs[0]) // Already fired
                            return;
                        if ((haveInputs == 1) && !inputs[0])    // Need a second
                            return;
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

            gatesByWire = gates.ToDictionary(g => g.OutputWire, g => g);
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

        long Run(long num1, long num2, int numBits)
        {
            Reset();

            for (int x = 0; x < numBits; x++)
            {
                SetWire("x" + x.ToString("00"), (num1 & (1L << x)) != 0); 
            }

            for (int y = 0; y < numBits; y++)
            {
                SetWire("y" + y.ToString("00"), (num2 & (1L << y)) != 0);
            }

            long output = 0;

            for (int z = 0; z < numBits; z++)
            {
                string wire = "z" + z.ToString("00");

                if (wireValues[wire])
                    output += (1L << z);
            }

            //var bits = new string(wireValues.Where(w => w.Key.StartsWith('z')).OrderByDescending(w => w.Key).Select(w => w.Value ? '1' : '0').ToArray());

            //return Convert.ToInt64(bits, 2);

            return output;
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

        int TestBits(int bitsToTest)
        {
            long correctMask = (1L << bitsToTest) - 1;

            for (int i = 0; i < 100; i++)
            {
                long num1 = (long)(random.NextDouble() * (1L << bitsToTest));
                long num2 = (long)(random.NextDouble() * (1L << bitsToTest));

                long calculated = Run(num1, num2, bitsToTest);

                long sum = num1 + num2;

                for (int bit = 0; bit < bitsToTest; bit++)
                {
                    if ((calculated & (1L << bit)) != (sum & (1L << bit)))
                    {
                        correctMask &= ~(1L << bit);
                    }
                }

                if (correctMask == 0)
                    return 0;
            }

            return Convert.ToString(correctMask, 2).Where(c => c == '1').Count();
        }

        void Swap2(string wire1, string wire2)
        {
            Gate gate1 = gatesByWire[wire1];
            Gate gate2 = gatesByWire[wire2];

            string tmp = gate1.OutputWire;
            gate1.OutputWire = gate2.OutputWire;
            gate2.OutputWire = tmp;
        }

        public override long Compute2()
        {
            ReadData();

            HashSet<string> goodWires = new();

            List<(string, string)> swapped = new();

            for (int bits = 1; bits < 46; bits++)
            {
                int correct = TestBits(bits);

                HashSet<string> usedWires = wireValues.Keys.Where(w => !w.StartsWith('x') && !w.StartsWith('y')).Where(w => !goodWires.Contains(w)).ToHashSet();

                if (correct == bits)
                {
                }
                else
                {
                    var wires = usedWires.ToList();
                    //var wires = gates.Select(g => g.OutputWire).Where(w => !goodWires.Contains(w)).ToList();
                    (string, string)? toSwap = null;

                    for (int w1 = 0; w1 < wires.Count; w1++)
                    {
                        for (int w2 = w1 + 1; w2 < wires.Count; w2++)
                        {
                            string wire1 = wires[w1];
                            string wire2 = wires[w2];

                            if (wire1 != wire2)
                            {
                                Swap2(wire1, wire2);

                                try
                                {
                                    correct = TestBits(bits);

                                    if (correct == bits)
                                    {
                                        if (toSwap != null)
                                        {

                                        }

                                        toSwap = (wire1, wire2);
                                    }
                                }
                                catch { }

                                Swap2(wire1, wire2);
                            }
                        }
                    }

                    if (toSwap == null)
                        throw new Exception();

                    Swap2(toSwap.Value.Item1, toSwap.Value.Item2);
                    swapped.Add(toSwap.Value);
               }

                foreach (string good in usedWires)
                    goodWires.Add(good);
            }

            var allswapped = new List<string>();

            foreach (var swap in swapped)
            {
                allswapped.Add(swap.Item1);
                allswapped.Add(swap.Item2);
            }

            allswapped.Sort();

            string result = string.Join(',', allswapped);

            return 0;
        }
    }
}

