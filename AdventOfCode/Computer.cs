namespace AdventOfCode
{
    public class ComputerInstruction
    {
        public IComputer Computer { get; private set; }
        public string InstructionString { get; private set; }

        public ComputerInstruction(string instructionString, IComputer computer)
        {
            this.InstructionString = instructionString;
            this.Computer = computer;
        }

        public override string ToString()
        {
            return InstructionString;
        }

        public virtual void Execute()
        {
            Computer.InstructionPointer++;
        }
    }

    public interface IComputer
    {
        int InstructionPointer { get; set; }

        void SetRegister(string register, long val);
        long GetRegister(string register);
    }

    public class Computer<T> : IComputer where T : ComputerInstruction
    {
        public List<T> Instructions { get; set; } = new List<T>();
        public int InstructionPointer { get; set; }
        public Dictionary<string, long> Registers { get; private set; } = new Dictionary<string, long>();
        public string LastWrittenRegister { get; private set; }
        public T LastInstruction { get; private set; }
        public int LastInstructionPos { get; private set; }

        public Computer()
        {
        }

        public void SetProgram(IEnumerable<string> instructionStrings)
        {
            foreach (string instructionString in instructionStrings)
            {
                Instructions.Add(Activator.CreateInstance(typeof(T), instructionString, this) as T);
            }

            InstructionPointer = 0;
        }

        public void SetRegister(string register, long val)
        {
            Registers[register] = val;

            LastWrittenRegister = register;
        }

        public long GetRegister(string register)
        {
            if (!Registers.ContainsKey(register))
                return 0;

            return Registers[register];
        }

        public void RunDebug()
        {
            InstructionPointer = 0;

            string waitForRegister = null;
            int waitForInstruction = -1;
            int run = 0;

            do
            {
                LastWrittenRegister = null;

                if (!RunInstruction())
                    break;

                if (run > 0)
                {
                    run--;
                    continue;
                }

                if (waitForRegister != null)
                {
                    if (LastWrittenRegister != waitForRegister)
                    {
                        continue;
                    }

                    waitForRegister = null;
                }

                if (waitForInstruction != -1)
                {
                    if (InstructionPointer != waitForInstruction)
                    {
                        continue;
                    }

                    waitForInstruction = -1;
                }

                do
                {
                    Console.WriteLine();
                    Console.WriteLine(LastInstructionPos + ": " + LastInstruction.ToString());
                    Console.WriteLine();

                    foreach (var register in Registers)
                    {
                        Console.Write("[" + register.Key + "] " + register.Value);

                        if (LastWrittenRegister == register.Key)
                            Console.Write(" < ");

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.Write("> ");

                    string cmd = Console.ReadLine();

                    if (cmd.StartsWith("run"))
                    {
                        run = int.Parse(cmd.Substring(4));

                        break;
                    }
                    else if (cmd.StartsWith("waitreg"))
                    {
                        waitForRegister = cmd.Substring(8).Trim();

                        if (!Registers.ContainsKey(waitForRegister))
                        {
                            Console.WriteLine("Bad register");

                            waitForRegister = null;
                        }
                        break;
                    }
                    else if (cmd.StartsWith("waitinst"))
                    {
                        waitForInstruction = int.Parse(cmd.Substring(9));

                        if ((waitForInstruction < 0) | (waitForInstruction > (Instructions.Count - 1)))
                        {
                            Console.WriteLine("Bad instruction");

                            waitForInstruction = -1;
                        }
                        break;
                    }
                    else if (cmd.StartsWith("setreg"))
                    {
                        string[] args = cmd.Substring(7).Split(' ').ToArray();

                        Registers[args[0]] = long.Parse(args[1]);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cmd))
                        {
                            Console.WriteLine("Bad command");
                        }

                        break;
                    }
                }
                while (true);
            }
            while (true);
        }

        public bool RunInstruction()
        {
            if (InstructionPointer >= Instructions.Count)
                return false;

            LastInstruction = Instructions[InstructionPointer];
            LastInstructionPos = InstructionPointer;

            Instructions[InstructionPointer].Execute();

            return true;
        }
    }
}
