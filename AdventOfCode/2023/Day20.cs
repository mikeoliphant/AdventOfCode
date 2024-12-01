namespace AdventOfCode._2023
{
    internal class Day20 : Day
    {
        static Dictionary<string, Module> modules = new();
        static long numLow = 0;
        static long numHigh = 0;
        static long press = 0;

        static Dictionary<string, long> gqPulses = new();
        static Dictionary<string, long> gqPulseDeltas = new();

        Broadcast broadcast = new Broadcast();

        static Queue<(Module Module, bool Pulse)> pulseQueue = new();

        class Module
        {
            public string Name;
            public string[] OutputModules { get; set;  }

            List<(Module Module, bool High)> toPulse = new();

            public virtual void PulseFrom(string from, bool high)
            {
                if (high)
                    numHigh++;
                else
                    numLow++;

                //Console.WriteLine(from + "->" + Name + ": " + (high ? "high" : "low"));
            }

            public void DoOutput(bool high)
            {
                toPulse.Clear();

                foreach (string output in OutputModules)
                {
                    modules[output].PulseFrom(Name, high);
                }
            }
        }

        class Broadcast : Module
        {
            public override void PulseFrom(string from, bool high)
            {
                base.PulseFrom(from, high);

                pulseQueue.Enqueue((this, high));
            }
        }

        class FlipFlop : Module
        {
            bool on = false;

            public override void PulseFrom(string from, bool high)
            {
                base.PulseFrom(from, high);

                if (!high)
                {
                    pulseQueue.Enqueue((this, !on));

                    on = !on;
                }
            }
        }

        class Conjunction : Module
        {
            Dictionary<string, bool> memory = new();

            public void SetMemory(string from, bool high)
            {
                memory[from] = high;
            }

            public override void PulseFrom(string from, bool high)
            {
                base.PulseFrom(from, high);

                memory[from] = high;

                bool allHigh = true;

                foreach (bool value in memory.Values)
                {
                    if (!value)
                    {
                        allHigh = false;
                    }
                }

                if (Name == "gq" && high)
                {
                    if (gqPulses.ContainsKey(from))
                    {
                        long delta = press - gqPulses[from];

                        gqPulseDeltas[from] = delta;
                    }

                    gqPulses[from] = press;
                }

                pulseQueue.Enqueue((this, !allHigh));
            }
        }

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] modVals = line.Split(" -> ");

                string[] outputs = modVals[1].Split(", ");

                if (modVals[0] == "broadcaster")
                {
                    broadcast.OutputModules = outputs;
                }
                else
                {
                    string name = modVals[0].Substring(1);

                    if (modVals[0][0] == '%')
                    {
                        modules[name] = new FlipFlop()
                        {
                            Name = name,
                            OutputModules = outputs
                        };
                    }
                    else
                    {
                        modules[name] = new Conjunction()
                        {
                            Name = name,
                            OutputModules = outputs
                        };
                    }
                }
            }

            // Add "rx" module not in file
            modules["rx"] = new Module { Name = "rx", OutputModules = new string[0] };

            // Initialize memory

            foreach (Module module in modules.Values)
            {
                foreach (string output in module.OutputModules)
                {
                    Conjunction conj = modules[output] as Conjunction;

                    if (conj != null)
                    {
                        conj.SetMemory(module.Name, false);
                    }
                }
            }
        }

        public override long Compute()
        {
            ReadData();

            for (int i = 0; i < 1000; i++)
            {
                broadcast.PulseFrom("button", false);

                while (pulseQueue.Count > 0)
                {
                    var toPulse = pulseQueue.Dequeue();

                    toPulse.Module.DoOutput(toPulse.Pulse);
                }
            }

            long tot = numLow * numHigh;

            return tot;
        }


        public override long Compute2()
        {
            ReadData();

            do
            {
                press++;

                broadcast.PulseFrom("button", false);

                while (pulseQueue.Count > 0)
                {
                    var toPulse = pulseQueue.Dequeue();

                    toPulse.Module.DoOutput(toPulse.Pulse);
                }
            }
            while (gqPulseDeltas.Count < 4);    // Find the number of press cycles for each module feeding into gq (which pulses rx)

            long lcm = 1;

            // Total number of presses is the least common multiple of the four press cycles
            foreach (long delta in gqPulseDeltas.Values)
            {
                lcm = FactorHelper.LeastCommonMultiple(lcm, delta);
            }

            return lcm;
        }
    }
}
