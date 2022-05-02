namespace AdventOfCode._2017
{
    internal class Day8
    {
        Dictionary<string, int> registers = new Dictionary<string, int>();

        int GetRegister(string name)
        {
            if (!registers.ContainsKey(name))
                return 0;

            return registers[name];
        }

        void SetRegister(string name, int value)
        {
            registers[name] = value;
        }

        public long Compute()
        {
            int highest = int.MinValue;

            foreach (string instruction in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day8.txt"))
            {
                string[] split = instruction.Split(" if ");

                string[] cond = split[1].Split(' ');

                int condValue = int.Parse(cond[2]);

                bool condResult = false;

                switch (cond[1])
                {
                    case "<":
                        condResult = GetRegister(cond[0]) < condValue; 
                        break;

                    case ">":
                        condResult = GetRegister(cond[0]) > condValue;
                        break;

                    case "<=":
                        condResult = GetRegister(cond[0]) <= condValue;
                        break;

                    case ">=":
                        condResult = GetRegister(cond[0]) >= condValue;
                        break;

                    case "==":
                        condResult = GetRegister(cond[0]) == condValue;
                        break;

                    case "!=":
                        condResult = GetRegister(cond[0]) != condValue;
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                if (condResult)
                {
                    string[] incDec = split[0].Split(' ');

                    int incDecValue = int.Parse(incDec[2]);

                    if (incDec[1] == "inc")
                    {
                        SetRegister(incDec[0], GetRegister(incDec[0]) + incDecValue);
                    }
                    else
                    {
                        SetRegister(incDec[0], GetRegister(incDec[0]) - incDecValue);
                    }

                    highest = Math.Max(GetRegister(incDec[0]), highest);
                }
            }

            return highest; // registers.Values.Max();
        }
    }
}
