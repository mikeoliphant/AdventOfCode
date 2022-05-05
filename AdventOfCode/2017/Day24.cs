namespace AdventOfCode._2017
{
    internal class Day24
    {
        List<(int P1, int P2)> components = new List<(int P1, int P2)>();

        void ReadInput()
        {
            foreach (string portStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day24.txt"))
            {
                string[] ports = portStr.Split("/");

                components.Add((int.Parse(ports[0]), int.Parse(ports[1])));
            }
        }

        int GetMaxStrength(int port, List<(int P1, int P2)> available)
        {
            int maxStrength = 0;

            foreach (var nextPort in available.Where(a => (a.P1 == port) || (a.P2 == port)))
            {
                var newAvailable = available.ToList();
                newAvailable.Remove(nextPort);

                int nextStrength = nextPort.P1 + nextPort.P2 + GetMaxStrength((nextPort.P1 == port) ? nextPort.P2 : nextPort.P1, newAvailable);

                if (nextStrength > maxStrength)
                {
                    maxStrength = nextStrength;
                }
            }

            return maxStrength;
        }

        int GetMaxLengthStrength(int port, List<(int P1, int P2)> available, int lengthBonus)
        {
            int maxStrength = 0;

            foreach (var nextPort in available.Where(a => (a.P1 == port) || (a.P2 == port)))
            {
                var newAvailable = available.ToList();
                newAvailable.Remove(nextPort);

                int nextStrength = nextPort.P1 + nextPort.P2 + GetMaxLengthStrength((nextPort.P1 == port) ? nextPort.P2 : nextPort.P1, newAvailable, lengthBonus + 10000);

                if (nextStrength > maxStrength)
                {
                    maxStrength = nextStrength;
                }
            }

            return maxStrength + lengthBonus;
        }

        public long Compute()
        {
            ReadInput();

            return GetMaxStrength(0, components);
        }

        public long Compute2()
        {
            ReadInput();

            return GetMaxLengthStrength(0, components, 0) % 10000;
        }
    }
}
