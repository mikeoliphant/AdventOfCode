using System.Diagnostics;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace AdventOfCode._2022
{
    internal class Day16 : Day
    {       
        class Valve
        {
            public static List<Valve> Valves { get; private set; } = new List<Valve>();

            public string Name { get; set; }
            public int ID { get; set; }
            public int FlowRate { get; set; }
            public List<Valve> ConnectedValves { get; set; } = new List<Valve>();

            public static Valve GetValve(string name)
            {
                var valve = Valves.FirstOrDefault(v => v.Name == name);

                if (valve != null)
                    return valve;

                Valve newValve = new Valve();

                newValve.Name = name;
                newValve.ID = Valves.Count;

                Valves.Add(newValve);

                return newValve;
            }

            public override string ToString()
            {
                return Name + " (" + FlowRate + ") -> " + String.Join(", ", ConnectedValves.Select(v => v.Name));
            }
        }

        void ReadInput(string dataFile)
        {
            foreach (string line in File.ReadLines(dataFile))
            {
                var match = Regex.Match(line, "Valve (.*) has flow rate=(.*); tunnel.* lead.* to valve.? (.*)");

                if (!match.Success)
                    throw new Exception();

                Valve valve = Valve.GetValve(match.Groups[1].Value);

                valve.FlowRate = int.Parse(match.Groups[2].Value);

                string[] connectedTunnels = match.Groups[3].Value.Split(',');

                foreach (string connected in connectedTunnels)
                {
                    valve.ConnectedValves.Add(Valve.GetValve(connected.Trim()));
                }
            }
        }

        int GetFlow(ulong valves)
        {
            int flow = 0;

            foreach (Valve valve in Valve.Valves)
            {
                if ((valves & (1ul << valve.ID)) != 0)
                    flow += valve.FlowRate;
            }

            return flow;
        }

        string GetOpenValves(ulong valves)
        {
            string valveStr = "";

            foreach (Valve valve in Valve.Valves)
            {
                if ((valves & (1ul << valve.ID)) != 0)
                    valveStr += valve.Name + " ";
            }

            return valveStr;
        }

        IEnumerable<KeyValuePair<(ulong Valves, int Pos, int Time), float>> GetNeighbors((ulong Valves, int Pos, int Time) state)
        {
            Valve currentValve = Valve.Valves[state.Pos];

            // Get flow for current state
            int flow = 10000 - GetFlow(state.Valves);

            if ((currentValve.FlowRate != 0) && ((state.Valves & (1ul << currentValve.ID)) == 0)) // Stay put and open valve
            {
                yield return new KeyValuePair<(ulong Valves, int Pos, int Time), float>((state.Valves | (1ul << currentValve.ID), state.Pos, state.Time + 1), flow); 
            }

            foreach (Valve neighbor in currentValve.ConnectedValves)    // Move without opening valve
            {
                yield return new KeyValuePair<(ulong Valves, int Pos, int Time), float>((state.Valves, neighbor.ID, state.Time + 1), flow);   
            }
        }

        int PrintPathToConsole(List<(ulong Valves, int Pos, int Time)> path)
        {
            int totFlow = 0;

            foreach (var state in path)
            {
                int flow = GetFlow(state.Valves);
                totFlow += flow;

                Console.WriteLine("Minute " + state.Time);
                Console.WriteLine("At: " + Valve.Valves[state.Pos].Name);
                Console.WriteLine("Valves: " + GetOpenValves(state.Valves));
                Console.WriteLine("Flow: " + flow + " (" + totFlow + ")");
                Console.WriteLine();
            }

            return totFlow;
        }


        public override long Compute()
        {
            ReadInput(DataFile);

            DijkstraSearch<(ulong Valves, int Pos, int Time)> search = new DijkstraSearch<(ulong Valves, int Pos, int Time)>(GetNeighbors);

            var result = search.GetShortestPath((0, Valve.GetValve("AA").ID, 1),
                delegate ((ulong Valves, int Pos, int Time) state)
                {
                    return (state.Time == 30);
                });

            if (result.Path == null)
                throw new Exception();

            return PrintPathToConsole(result.Path);
        }

        //IEnumerable<(ulong Valves, int Pos, int Flow)> GetTransitions((ulong Valves, int Pos, int Time) state)
        //{
        //    if (state.Pos >= 1000)  // We are moving to the next valve
        //    {
        //        yield return (state.Valves, state.Pos - 1000, 0);
        //    }
        //    else if ((Valve.Valves[state.Pos].FlowRate > 0) && ((state.Valves & (1ul << state.Pos)) == 0))  // Open current valve
        //    {
        //        yield return (state.Valves | (1ul << state.Pos), state.Pos, Valve.Valves[state.Pos].FlowRate * (MaxTime - state.Time));
        //    }
        //    else
        //    {
        //        bool haveTransition = false;

        //        foreach (var connected in distances[state.Pos])
        //        {
        //            if ((state.Valves & (1ul << connected.ID)) == 0)
        //            {
        //                if (connected.Dist < (MaxTime - state.Time))
        //                {
        //                    yield return (state.Valves, connected.ID + (1000 * (connected.Dist - 1)), 0);
        //                }

        //                haveTransition = true;
        //            }
        //        }

        //        // Stay put if we have nowhere we need to go
        //        if (!haveTransition)
        //        {
        //            yield return (state.Valves, state.Pos, 0);
        //        }
        //    }
        //}

        //IEnumerable<KeyValuePair<State2, float>> GetNeighbors2(State2 state)
        //{
        //    // Get flow for current state
        //    //int flow = 10000 - GetFlow(state.Valves);

        //    foreach (var trans1 in GetTransitions((state.Valves, state.Pos, state.Time)))
        //    {
        //        foreach (var trans2 in GetTransitions((trans1.Valves, state.Pos2, state.Time)))
        //        {
        //            //if (trans2.Pos != trans1.Pos)
        //                yield return new KeyValuePair<State2, float>(new State2(trans2.Valves, Math.Min(trans1.Pos, trans2.Pos), Math.Max(trans1.Pos, trans2.Pos), state.Time + 1), 100000 - (trans1.Flow + trans2.Flow));  // Do Min() Max() to avoid duplicates
        //            //yield return new KeyValuePair<State2, float>(new State2(trans2.Valves, trans1.Pos, trans2.Pos, state.Time + 1), flow);
        //        }
        //    }
        //}

        //int PrintPathToConsole2(List<State2> path)
        //{
        //    int totFlow = 0;

        //    foreach (var state in path)
        //    {
        //        int flow = GetFlow(state.Valves);
        //        totFlow += flow;

        //        Console.WriteLine("Minute " + state.Time);
        //        Console.WriteLine("At: " + Valve.Valves[state.Pos % 1000].Name);
        //        Console.WriteLine("Elephant at: " + Valve.Valves[state.Pos2 % 1000].Name);
        //        Console.WriteLine("Valves: " + GetOpenValves(state.Valves));
        //        Console.WriteLine("Flow: " + flow + " (" + totFlow + ")");
        //        Console.WriteLine();
        //    }

        //    return totFlow;
        //}

        public int MaxTime;
        Dictionary<int, List<(int ID, int Dist)>> distances = new Dictionary<int, List<(int ID, int Dist)>>();

        void ComputeDistances()
        {
            DijkstraSearch<int> search = new DijkstraSearch<int>(delegate (int ID) { return Valve.Valves[ID].ConnectedValves.Select(v => v.ID); });

            foreach (Valve valve in Valve.Valves)
            {
                if ((valve.Name == "AA") || (valve.FlowRate > 0)) // Only consider valves we want to turn on
                {
                    distances[valve.ID] = new List<(int ID, int Dist)>();

                    foreach (Valve other in Valve.Valves)
                    {
                        if ((other != valve) && (other.FlowRate > 0))
                        {
                            var result = search.GetShortestPath(valve.ID, other.ID);

                            if (result.Path != null)
                            {
                                distances[valve.ID].Add((other.ID, (int)result.Cost));
                            }
                        }
                    }

                    distances[valve.ID].Sort((a, b) => b.Dist.CompareTo(a.Dist));
                }
            }
        }   

        record struct State2(ulong Valves, int Pos, int Pos2, int Time);

        Dictionary<ulong, int> flowDict = new Dictionary<ulong, int>();

        IEnumerable<ulong> GetPossibilities(ulong mask, ulong startState, int pos, int currentFlow, int flowRate, int time)
        {
            bool haveConnection = false;

            foreach (var connected in distances[pos])
            {
                if ((((startState | mask) & (1ul << connected.ID)) == 0) && (connected.Dist < time))
                {
                    foreach (ulong state in GetPossibilities(mask, startState | (1ul << connected.ID), connected.ID, currentFlow + ((connected.Dist + 1) * flowRate), flowRate + Valve.Valves[connected.ID].FlowRate, time - connected.Dist - 1))
                    {
                        haveConnection = true;

                        yield return state;
                    }
                }
            }

            int flowLeft = currentFlow + (flowRate * time);

            if (!flowDict.ContainsKey(startState))
            {
                flowDict[startState] = flowLeft;
            }
            else
            {
                flowDict[startState] = Math.Max(flowDict[startState], flowLeft);
            }

            yield return startState;
        }

        IEnumerable<(ulong, ulong)> GetOtherPossibilities(IEnumerable<ulong> firstPossibilities, int pos, int time)
        {
            foreach (ulong first in firstPossibilities)
            {
                foreach (ulong second in GetPossibilities(first, 0, pos, 0, 0, time))
                {
                    yield return (Math.Min(first, second), Math.Max(first, second));
                }
            }
        }

        public override long Compute2()
        {
            MaxTime = 26;

            ReadInput(DataFile);

            ComputeDistances();

            var first = GetPossibilities(0, 0, Valve.GetValve("AA").ID, 0, 0, MaxTime).Distinct();

            var second = GetOtherPossibilities(first, Valve.GetValve("AA").ID, MaxTime).Distinct();

            int maxFlow = 0;
            (ulong, ulong) maxPair = (0, 0);

            foreach (var pair in second)
            {
                int flow = flowDict[pair.Item1] + flowDict[pair.Item2];

                if (flow > maxFlow)
                {
                    maxFlow = flow;
                    maxPair = pair;
                }
            }

            Console.WriteLine(GetOpenValves(maxPair.Item1) + " -- " + GetOpenValves(maxPair.Item2));

            return maxFlow;
        }
    }
}
