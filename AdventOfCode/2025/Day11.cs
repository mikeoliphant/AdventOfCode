
namespace AdventOfCode._2025
{
    using PathCache = (string Node, bool haveDAC, bool haveFFT);

    internal class Day11 : Day
    {
        Graph<string> graph = new();
        Dictionary<string, long> numPathsToOut = new();
        Dictionary<PathCache, long> numPathsWithDACFFT = new();

        void ReadData()
        {
            foreach (string line in File.ReadLines(DataFile))
            {
                string[] fromTo = line.Split(':');

                foreach (string to in fromTo[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    graph.Connect(fromTo[0], to);
                }
            }
        }

        long GetNumPathsToOut(string start, HashSet<string> visited)
        {
            if (start == "out")
            {
                return 1;
            }

            long totPaths = 0;

            HashSet<string> newVisited = new(visited);
            newVisited.Add(start);

            foreach (string to in graph.GetConnectionsFrom(start))
            {
                if (numPathsToOut.ContainsKey(to))
                {
                    totPaths += numPathsToOut[to];
                }
                else
                {
                    long paths = GetNumPathsToOut(to, newVisited);

                    numPathsToOut[to] = paths;

                    totPaths += paths;
                }
            }

            return totPaths;
        }


        public override long Compute()
        {
            ReadData();

            return GetNumPathsToOut("you", new HashSet<string>());
        }

        long GetNumPathsWithDACFFT(string start, HashSet<string> visited)
        {
            if (start == "out")
            {
                return (visited.Contains("dac") && visited.Contains("fft")) ? 1 : 0;
            }

            var cache = new PathCache(start, visited.Contains("dac"), visited.Contains("fft"));

            if (numPathsWithDACFFT.ContainsKey(cache))
                return numPathsWithDACFFT[cache];

            long totPaths = 0;

            HashSet<string> newVisited = new(visited);
            newVisited.Add(start);

            foreach (string to in graph.GetConnectionsFrom(start))
            {
                totPaths += GetNumPathsWithDACFFT(to, newVisited);
            }

            numPathsWithDACFFT[cache] = totPaths;

            return totPaths;
        }

        public override long Compute2()
        {
            ReadData();

            return GetNumPathsWithDACFFT("svr", new HashSet<string>());
        }
    }
}
