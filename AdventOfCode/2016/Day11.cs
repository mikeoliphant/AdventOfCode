namespace AdventOfCode._2016
{
    internal class Day11
    {
        int numTypes = 0;
        string[] typeNames = null;
        long startState = 0;

        int GetFloorGenerators(long state, int floor)
        {
            return (int)BitUtil.GetLongStorage(state, floor * numTypes * 2, numTypes);
        }

        int GetFloorChips(long state, int floor)
        {
            return (int)BitUtil.GetLongStorage(state, numTypes + (floor * numTypes * 2), numTypes);
        }

        long SetFloorGenerators(long state, int floor, int generators)
        {
            return BitUtil.SetLongStorage(state, generators, floor * numTypes * 2, numTypes);
        }

        long SetFloorChips(long state, int floor, int chips)
        {
            return BitUtil.SetLongStorage(state, chips, numTypes + (floor * numTypes * 2), numTypes);
        }

        int GetElevatorFloor(long state)
        {
            return (int)BitUtil.GetLongStorage(state, numTypes * 2 * 4, 2);
        }

        long SetElevatorFloor(long state, int floor)
        {
            return BitUtil.SetLongStorage(state, floor, numTypes * 2 * 4, 2);
        }

        void PrintStateToConsole(long state)
        {
            for (int floor = 0; floor < 4; floor++)
            {
                Console.Write("Floor: " + floor);

                Console.Write("  Generators: ");

                int generators = GetFloorGenerators(state, floor);

                for (int pos = 0; pos < numTypes; pos++)
                {
                    if ((generators & (1 << pos)) != 0)
                    {
                        Console.Write(typeNames[pos] + " ");
                    }
                }

                Console.Write("  Chips: ");

                int chips = GetFloorChips(state, floor);

                for (int pos = 0; pos < numTypes; pos++)
                {
                    if ((chips & (1 << pos)) != 0)
                    {
                        Console.Write(typeNames[pos] + " ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Elevator is on floor: " + GetElevatorFloor(state));

            Console.WriteLine();
        }

        void ReadInput()
        {
            string[] floors = File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day11Part2.txt").ToArray();

            Dictionary<string, int> generatorFloors = new Dictionary<string, int>();
            Dictionary<string, int> chipFloors = new Dictionary<string, int>();

            for (int floor = 0; floor < floors.Length; floor++)
            {
                string[] split = floors[floor].Replace(".", "").Replace(",", "").Split(' ');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "generator")
                    {
                        generatorFloors[split[i - 1]] = floor;
                    }
                    else if (split[i] == "microchip")
                    {
                        string[] chip = split[i - 1].Split('-');

                        chipFloors[chip[0]] = floor;
                    }
                }
            }

            numTypes = generatorFloors.Keys.Count();
            typeNames = generatorFloors.Keys.ToArray();

            int[] floorGenerators = new int[4];
            int[] floorChips = new int[4];

            foreach (var generatorFloor in generatorFloors)
            {
                floorGenerators[generatorFloor.Value] |= 1 << Array.IndexOf(typeNames, generatorFloor.Key);
            }

            foreach (var chipFloor in chipFloors)
            {
                floorChips[chipFloor.Value] |= 1 << Array.IndexOf(typeNames, chipFloor.Key);
            }

            for (int floor = 0; floor < 4; floor++)
            {
                startState = SetFloorGenerators(startState, floor, floorGenerators[floor]);
                startState = SetFloorChips(startState, floor, floorChips[floor]);
            }

            startState = SetElevatorFloor(startState, 0);
        }

        IEnumerable<long> GetFloorTransitions(long state, int fromFloor, int toFloor)
        {
            int fromFloorGenerators = GetFloorGenerators(state, fromFloor);
            int fromFloorChips = GetFloorChips(state, fromFloor);
            int toFloorGenerators = GetFloorGenerators(state, toFloor);
            int toFloorChips = GetFloorChips(state, toFloor);

            state = SetElevatorFloor(state, toFloor);

            for (int generator = 0; generator < numTypes; generator++)
            {
                int generatorMask = 1 << generator;

                if ((fromFloorGenerators & generatorMask) != 0)
                {
                    // Just take one generator
                    yield return SetFloorGenerators(SetFloorGenerators(state, toFloor, toFloorGenerators | generatorMask), fromFloor, fromFloorGenerators & ~generatorMask);

                    for (int generator2 = generator + 1; generator2 < numTypes; generator2++)
                    {
                        int generatorMask2 = generatorMask | (1 << generator2);

                        if ((fromFloorGenerators & generatorMask2) == generatorMask2)
                        {
                            // Take two generators
                            yield return SetFloorGenerators(SetFloorGenerators(state, toFloor, toFloorGenerators | generatorMask2), fromFloor, fromFloorGenerators & ~generatorMask2);
                        }
                    }

                    // Take generator and its chip
                    if ((fromFloorChips & generatorMask) == generatorMask)
                    {
                        yield return SetFloorChips(SetFloorChips(SetFloorGenerators(SetFloorGenerators(state, toFloor, toFloorGenerators | generatorMask), fromFloor, fromFloorGenerators & ~generatorMask), toFloor, toFloorChips | generatorMask), fromFloor, fromFloorChips & ~generatorMask);
                    }
                }
            }

            for (int chip = 0; chip < numTypes; chip++)
            {
                int chipMask = 1 << chip;

                if ((fromFloorChips & chipMask) != 0)
                {
                    // Take one chip
                    yield return SetFloorChips(SetFloorChips(state, toFloor, toFloorChips | chipMask), fromFloor, fromFloorChips & ~chipMask);

                    for (int chip2 = chip + 1; chip2 < numTypes; chip2++)
                    {
                        int chipMask2 = chipMask | (1 << chip2);

                        if ((fromFloorChips & chipMask2) == chipMask2)
                        {
                            // Take two chips
                            yield return SetFloorChips(SetFloorChips(state, toFloor, toFloorChips | chipMask2), fromFloor, fromFloorChips & ~chipMask2);
                        }
                    }
                }
            }
        }

        bool IsStable(long state, int floor)
        {
            int floorGenerators = GetFloorGenerators(state, floor);

            if (floorGenerators == 0)
                return true;

            int floorChips = GetFloorChips(state, floor);

            for (int chip = 0; chip < numTypes; chip++)
            {
                int chipMask = 1 << chip;

                if ((floorChips & chipMask) != 0)
                {
                    if ((floorGenerators & chipMask) == 0)
                        return false;
                }
            }

            return true;
        }

        IEnumerable<long> GetPossibleTransitions(long state)
        {
            int elevatorFloor = GetElevatorFloor(state);

            if (elevatorFloor > 0)
            {
                foreach (long newState in GetFloorTransitions(state, elevatorFloor, elevatorFloor - 1))
                {
                    if (IsStable(newState, elevatorFloor) && IsStable(newState, elevatorFloor - 1))
                    {
                        yield return newState;
                    }
                }
            }

            if (elevatorFloor < 3)
            {
                foreach (long newState in GetFloorTransitions(state, elevatorFloor, elevatorFloor + 1))
                {
                    if (IsStable(newState, elevatorFloor) && IsStable(newState, elevatorFloor + 1))
                    {
                        yield return newState;
                    }
                }
            }
        }

        IEnumerable<KeyValuePair<long, float>> GetNeighbors(long state)
        {
            foreach (long neighbor in GetPossibleTransitions(state))
            {
                yield return new KeyValuePair<long, float>(neighbor, 1);
            }
        }
        
        public long Compute()
        {
            ReadInput();

            //PrintStateToConsole(startState);

            //foreach (long state in GetPossibleTransitions(startState))
            //{
            //    PrintStateToConsole(state);
            //}

            int mask = 0;

            for (int type = 0; type < numTypes; type++)
            {
                mask |= (1 << type);
            }

            long endState = 0;
            endState = SetElevatorFloor(endState, 3);
            endState = SetFloorChips(endState, 3, mask);
            endState = SetFloorGenerators(endState, 3, mask);

            //PrintStateToConsole(endState);

            DijkstraSearch<long> search = new DijkstraSearch<long>(GetNeighbors);

            List<long> path;
            float cost;

            if (search.GetShortestPath(startState, endState, out path, out cost))
            {
                //foreach (long state in path)
                //{
                //    PrintStateToConsole(state);
                //}

                return (long)cost;
            }

            throw new InvalidOperationException();
        }
    }
}
