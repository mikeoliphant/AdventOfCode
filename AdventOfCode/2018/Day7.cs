namespace AdventOfCode._2018
{
    internal class Day7
    {
        Dictionary<char, List<char>> prerequisites = new Dictionary<char, List<char>>();

        void ReadInput()
        {
            foreach (string prereqStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day7.txt"))
            {
                string[] words = prereqStr.Split(' ');

                char pre = words[1][0];
                char step = words[7][0];

                if (!prerequisites.ContainsKey(pre))
                {
                    prerequisites[pre] = new List<char>();
                }

                if (!prerequisites.ContainsKey(step))
                {
                    prerequisites[step] = new List<char>();
                }

                prerequisites[step].Add(pre);
            }
        }

        public string GetStepOrder()
        {
            string stepOrder = "";

            int remaining = 0;

            do
            {
                remaining = 0;

                char step = prerequisites.Where(p => (p.Value.Count == 0)).Select(p => p.Key).OrderBy(p => p).FirstOrDefault();

                if (step == 0)
                    break;

                prerequisites.Remove(step);

                stepOrder += step;

                foreach (List<char> pre in prerequisites.Values)
                {
                    pre.Remove(step);

                    remaining += pre.Count;
                }
            }
            while (true);

            return stepOrder;
        }

        public long Compute()
        {
            ReadInput();

            int numWorkers = 5;
            int baseCost = 61;

            (char Step, int Secs)[] workers = new (char Step, int Secs)[numWorkers];

            int secsSoFar = 0;

            int numSteps = prerequisites.Count;

            do
            {
                for (int w = 0; w < numWorkers; w++)
                {
                    if (workers[w].Step == 0)
                    {
                        workers[w].Step = prerequisites.Where(p => (p.Value.Count == 0)).Select(p => p.Key).OrderBy(p => p).FirstOrDefault();

                        prerequisites.Remove(workers[w].Step);
                    }
                }

                for (int w = 0; w < numWorkers; w++)
                {
                    if (workers[w].Step != 0)
                    {
                        workers[w].Secs++;

                        if (workers[w].Secs == (baseCost + (workers[w].Step - 'A')))
                        {
                            numSteps--;

                            foreach (List<char> pre in prerequisites.Values)
                            {
                                pre.Remove(workers[w].Step);
                            }

                            workers[w] = ('\0', 0);
                        }
                    }
                }

                secsSoFar++;
            }
            while (numSteps > 0);

            return secsSoFar;
        }
    }
}
