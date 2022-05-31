namespace AdventOfCode._2015
{
    internal class Day16 : Day
    {
        Dictionary<string, int> identified = new Dictionary<string, int>();

        Dictionary<string, int>[] allSues = new Dictionary<string, int>[500];

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2015\Day16Data.txt"))
            {
                string[] split = line.Split(": ");

                identified[split[0]] = int.Parse(split[1]);
            }

            foreach (string line in File.ReadLines(DataFile))
            {
                string[] split = line.Split(": ", 2);

                int sueNumber = int.Parse(split[0].Split(" ")[1]) - 1;

                string[] props = split[1].Split(", ");

                allSues[sueNumber] = new Dictionary<string, int>();

                foreach (string prop in props)
                {
                    string[] propSplit = prop.Split(": ");

                    allSues[sueNumber][propSplit[0]] = int.Parse(propSplit[1]);
                }
            }
        }

        public override long Compute()
        {
            ReadInput();

            for (int sueNum = 0; sueNum < allSues.Length; sueNum++)
            {
                var sue = allSues[sueNum];

                bool match = true;

                foreach (var prop in identified)
                {
                    if (sue.ContainsKey(prop.Key) && (sue[prop.Key] != prop.Value))
                    {
                        match = false;

                        break;
                    }
                }

                if (match)
                    return sueNum + 1;
            }

            throw new InvalidOperationException();
        }

        bool Match(Dictionary<string, int> sue, string key, int value)
        {
            if ((key == "cats") || (key == "trees"))
            {
                return sue[key] > value;
            }
            
            if ((key == "pomeranians") || (key == "goldfish"))
            {
                return sue[key] < value;
            }

            return sue[key] == value;
        }

        public override long Compute2()
        {
            ReadInput();

            for (int sueNum = 0; sueNum < allSues.Length; sueNum++)
            {
                var sue = allSues[sueNum];

                bool match = true;

                foreach (var prop in identified)
                {
                    if (sue.ContainsKey(prop.Key) && !Match(sue, prop.Key, prop.Value))
                    {
                        match = false;

                        break;
                    }
                }

                if (match)
                    return sueNum + 1;
            }

            throw new InvalidOperationException();
        }
    }
}
