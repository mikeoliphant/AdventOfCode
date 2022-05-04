namespace AdventOfCode._2017
{
    internal class Day18
    {
        class AudioComputer
        {
            public AudioComputer OtherComputer { get; set; }
            public long NumSends { get; private set; }
            public long LastFreq { get; private set; }

            Dictionary<char, long> registers = new Dictionary<char, long>();
            Queue<long> audioQueue = new Queue<long>();
            string[] commands = null;
            int commandPos = 0;

            public AudioComputer(string[] commands)
            {
                this.commands = commands;

                NumSends = 0;
            }

            public long GetVal(string var)
            {
                if (char.IsLetter(var[0]))
                {
                    if (!registers.ContainsKey(var[0]))
                        return 0;

                    return registers[var[0]];
                }

                return long.Parse(var);
            }

            public void SetVal(string var, long val)
            {
                registers[var[0]] = val;
            }

            public void Enqueue(long freq)
            {
                audioQueue.Enqueue(freq);
            }

            public bool RunInstruction()
            {
                string[] split = commands[commandPos].Split(' ');

                switch (split[0])
                {
                    case "set":
                        SetVal(split[1], GetVal(split[2]));
                        break;

                    case "add":
                        SetVal(split[1], GetVal(split[1]) + GetVal(split[2]));
                        break;

                    case "mul":
                        SetVal(split[1], GetVal(split[1]) * GetVal(split[2]));
                        break;

                    case "mod":
                        SetVal(split[1], GetVal(split[1]) % GetVal(split[2]));
                        break;

                    case "snd":
                        if (OtherComputer == null)
                        {
                            LastFreq = GetVal(split[1]);
                        }
                        else
                        {
                            NumSends++;
                            OtherComputer.Enqueue(GetVal(split[1]));
                        }
                        break;

                    case "jgz":
                        if (GetVal(split[1]) > 0)
                        {
                            commandPos += (int)GetVal(split[2]);

                            return true;
                        }
                        break;

                    case "rcv":
                        if (OtherComputer == null)
                        {
                            if (GetVal(split[1]) != 0)
                                return false;
                        }
                        else
                        {
                            if (audioQueue.Count == 0)
                                return false;

                            SetVal(split[1], audioQueue.Dequeue());
                        }

                        break;

                    default:
                        throw new InvalidOperationException();
                }

                commandPos++;

                return true;
            }
        }


        public long Compute()
        {
            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day18.txt").ToArray();

            AudioComputer computer = new AudioComputer(commands);

            while (computer.RunInstruction());

            return computer.LastFreq;
        }

        public long Compute2()
        {
            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day18.txt").ToArray();

            AudioComputer computer1 = new AudioComputer(commands);
            AudioComputer computer2 = new AudioComputer(commands);

            computer1.OtherComputer = computer2;
            computer2.OtherComputer = computer1;

            computer1.SetVal("p", 0);
            computer1.SetVal("p", 1);

            bool wait1 = false;
            bool wait2 = false;

            do
            {
                wait1 = computer1.RunInstruction();
                wait2 = computer2.RunInstruction();
            }
            while (wait1 || wait2);

            return computer1.NumSends;
        }
    }
}
