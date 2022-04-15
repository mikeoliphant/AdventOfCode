namespace AdventOfCode._2020
{
    public class Day13
    {
        public long Compute()
        {
            int departureTimestamp;
            int[] busIDs;

            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2020\Day13.txt"))
            {
                departureTimestamp = int.Parse(reader.ReadLine());
                busIDs = (from idStr in reader.ReadLine().Split(',') where idStr != "x" select int.Parse(idStr)).ToArray();
            }

            int[] busTimes = new int[busIDs.Length];

            for (int i = 0; i < busIDs.Length; i++)
            {
                if (busIDs[i] > 0)
                {
                    busTimes[i] = (int)Math.Ceiling((double)departureTimestamp / (double)busIDs[i]) * busIDs[i];
                }
            }

            return (busTimes.Min() - departureTimestamp) * busIDs[Array.IndexOf(busTimes, busTimes.Min())];
        }

        public long Compute2()
        {
            int departureTimestamp;
            List<int> busIDs = new List<int>();
            List<int> busOffsets = new List<int>();

            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2020\Day13.txt"))
            {
                departureTimestamp = int.Parse(reader.ReadLine());

                int offset = 0;

                foreach (string busIDStr in reader.ReadLine().Split(','))
                {
                    if (busIDStr != "x")
                    {
                        int id = int.Parse(busIDStr);

                        busIDs.Add(id);
                        busOffsets.Add(offset % id);
                    }

                    offset++;
                }
            }

            long timestamp = 0;

            bool haveMatch = true;

            long skip = busIDs[0];
            int numBusses = 2;

            do
            {
                do
                {
                    timestamp += skip;

                    if (timestamp == 1068781)
                    {
                    }
                    else if (timestamp > 1068781)
                    {
                    }

                    haveMatch = true;

                    for (int bus = 0; bus < numBusses; bus++)
                    {
                        if ((((long)Math.Ceiling((double)timestamp / (double)busIDs[bus]) * busIDs[bus]) - timestamp) != busOffsets[bus])
                        {
                            haveMatch = false;

                            break;
                        }
                    }
                }
                while (!haveMatch);

                skip *= busIDs[numBusses - 1];

                numBusses++;
            }
            while (numBusses < (busIDs.Count + 1));

            return timestamp;
        }
    }
}
