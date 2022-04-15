namespace AdventOfCode._2018
{
    internal class Day4
    {
        List<(DateTime Time, string Record)> records = new List<(DateTime T, string Record)>();
        Dictionary<int, Dictionary<int, int>> guardSleep = new Dictionary<int, Dictionary<int, int>>();

        public void ReadInput()
        {
            foreach (string record in File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day4.txt"))
            {
                string[] split = record.Split("]", 2);

                records.Add((DateTime.Parse(split[0].Substring(1)), split[1].Trim()));

                records.Sort((a, b) => a.Time.CompareTo(b.Time));
            }

            DateTime lastTime = DateTime.MinValue;
            int currentGuardID = -1;
            bool isAsleep = false;

            foreach (var record in records)
            {
                if (record.Record.StartsWith("Guard"))
                {
                    string[] words = record.Record.Split(' ');

                    currentGuardID = int.Parse(words[1].Substring(1));

                    isAsleep = false;
                }
                else if (record.Record.StartsWith("wakes"))
                {
                    if ((currentGuardID != -1) && isAsleep)
                    {
                        if (!guardSleep.ContainsKey(currentGuardID))
                            guardSleep[currentGuardID] = new Dictionary<int, int>();

                        do
                        {
                            int min = lastTime.Minute;

                            if (!guardSleep[currentGuardID].ContainsKey(min))
                            {
                                guardSleep[currentGuardID][min] = 1;
                            }
                            else
                            {
                                guardSleep[currentGuardID][min]++;
                            }

                            lastTime = lastTime.AddMinutes(1);
                        }
                        while (lastTime < record.Time);
                    }
                }
                else
                {
                    isAsleep = true;
                    lastTime = record.Time;
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            int maxMinutes = guardSleep.Max(g => g.Value.Sum(m => m.Value));
            int guardID = guardSleep.First(g => g.Value.Sum(m => m.Value) == maxMinutes).Key;
            int maxMinute = guardSleep[guardID].Max(m => m.Value);
            
            return guardID * guardSleep[guardID].First(m => m.Value == maxMinute).Key;
        }

        public long Compute2()
        {
            ReadInput();

            int maxMinutes = guardSleep.Max(g => g.Value.Max(m => m.Value));
            int guardID = guardSleep.First(g => g.Value.Any(m => m.Value == maxMinutes)).Key;
            int minute = guardSleep[guardID].First(m => m.Value == maxMinutes).Key;

            return guardID * minute;
        }
    }
}
