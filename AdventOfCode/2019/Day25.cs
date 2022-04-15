namespace AdventOfCode._2019
{
    //
    //  IDs for items (obtained using hackery)
    //
    //4601 - giant electromagnet(can't move)
    //4605 - molton lava (die)
    //4609 - coin
    //4613 - easter egg
    //4617 - monolith
    //4621 - photons (die)
    //4625 - jam
    //4629 - mug
    //4633 - shell
    //4637 - space heater
    //4641 - fuel cell
    //4645 - infinite loop (die)
    //4649 - escape pod (die)

    internal class Day25
    {
        IntcodeComputer computer = new IntcodeComputer();

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day25.txt").ToLongs(',').ToArray();

            computer.SetProgram(program);
        }

        string ReadLine()
        {
            string line = "";

            char c;

            do
            {
                c = (char)(computer.GetNextOutput());

                line += c;
            }
            while (c != 10);

            return line;
        }

        void WriteLine(string line)
        {
            foreach (char c in line)
            {
                computer.AddInput(c);
            }

            computer.AddInput('\n');
        }

        void CompareMemory(Dictionary<long, long> mem1, Dictionary<long, long> mem2)
        {
            foreach (var loc in mem1)
            {
                long val1 = loc.Value;
                long val2 = 0;
                mem2.TryGetValue(loc.Key, out val2);

                if (val1 != val2)
                {
                    Console.WriteLine("Loc: " + loc.Key + " " + val1 + " " + val2);
                }
            }

            foreach (var loc in mem2)
            {
                if (!mem1.ContainsKey(loc.Key))
                {
                    Console.WriteLine("Loc: " + loc.Key + " 0 " + loc.Value);
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            List<int> validItems = new List<int>();

            for (int i = 4609; i <= 4641; i += 4)
            {
                if (i != 4621)  // Skip "photons"
                    validItems.Add(i);
            }

            Dictionary<long, long> lastMemory = new Dictionary<long, long>();

            do
            {
                string line;

                do
                {
                    line = ReadLine();
                    Console.WriteLine(line);
                }
                while (!line.StartsWith("Command?"));

                var snapShot = computer.SnapshotMemory();

                bool handled = false;
                string cmd;

                do
                {
                    handled = false;

                    cmd = Console.ReadLine();

                    if (cmd.StartsWith("set"))  // Set the value of a memory location
                    {
                        string[] words = cmd.Split(' ');

                        computer.SetMemory(long.Parse(words[1]), long.Parse(words[2]));

                        handled = true;
                    }
                    else if (cmd.StartsWith("dump"))    // dump the memory changes from the last command
                    {
                        CompareMemory(lastMemory, snapShot);

                        handled = true;
                    }
                    else if (cmd.StartsWith("hack"))    // try going north with all combinations of non-lethal items
                    {
                        for (int mask = 0; mask < 256; mask++)
                        {
                            for (int itemPos = 0; itemPos < 8; itemPos++)
                            {
                                if ((mask & (1 << itemPos)) != 0)
                                {
                                    computer.SetMemory(validItems[itemPos], -1);    // Put item in inventory
                                }
                                else
                                {
                                    computer.SetMemory(validItems[itemPos], 0);     // Keep item out of inventory
                                }
                            }

                            WriteLine("north");

                            do
                            {
                                line = ReadLine();
                                Console.WriteLine(line);
                            }
                            while (!line.StartsWith("Command?"));
                        }
                    }
                }
                while (handled);

                lastMemory = snapShot;

                WriteLine(cmd);
            }
            while (true);


            return 0;
        }
    }
}
