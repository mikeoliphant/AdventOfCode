namespace AdventOfCode._2017
{   
    internal class Day13
    {
        Dictionary<int, int> scannerRange = new Dictionary<int, int>();

        void ReadInput()
        {
            foreach (string scanner in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day13.txt"))
            {
                string[] split = scanner.Split(": ");

                scannerRange[int.Parse(split[0])] = int.Parse(split[1]);
            }
        }

        public bool Severity(int delay, out int severity, bool breakOnCaught)
        {
            bool caught = false;

            severity = 0;

            foreach (var scanner in scannerRange)
            {
                if (((delay + scanner.Key) % (scanner.Value + scanner.Value - 2)) == 0)
                {
                    severity += (scanner.Key * scanner.Value);

                    caught = true;

                    if (breakOnCaught)
                        return true;
                }
            }

            return caught;
        }

        public long Compute()
        {
            ReadInput();

            int severity;

            Severity(0, out severity, breakOnCaught: false);

            return severity;
        }

        public long Compute2()
        {
            int delay = 0;
            int severity;

            ReadInput();

            do
            {
                delay++;
            }
            while (Severity(delay, out severity, breakOnCaught: true));

            return delay;
        }
    }
}
