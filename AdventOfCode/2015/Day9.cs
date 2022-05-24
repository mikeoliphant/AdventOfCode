namespace AdventOfCode._2015
{
    internal class Day9 : Day
    {
        Dictionary<int, List<(int City, int Dist)>> routes = new Dictionary<int, List<(int City, int Dist)>>();
        List<string> cities = new List<string>();

        int GetCity(string city)
        {
            int index = cities.IndexOf(city);

            if (index == -1)
            {
                cities.Add(city);

                return cities.Count - 1;
            }

            return index;
        }

        void AddRoute(string from, string to, int distance)
        {
            int fromID = GetCity(from);
            int toID = GetCity(to);

            if (!routes.ContainsKey(fromID))
            {
                routes[fromID] = new List<(int City, int Dist)>();
            }

            routes[fromID].Add((toID, distance));
        }

        IEnumerable<KeyValuePair<(int, int), float>> GetNeighbors((int City, int CityMask) state)
        {
            foreach (var toCity in routes[state.City])
            {
                if ((state.CityMask & (1 << toCity.City)) == 0)
                {
                    yield return new KeyValuePair<(int, int), float>((toCity.City, state.CityMask | (1 << toCity.City)), toCity.Dist);
                }
            }
        }

        IEnumerable<KeyValuePair<(int, int), float>> GetNeighborsMax((int City, int CityMask) state)
        {
            foreach (var toCity in routes[state.City])
            {
                if ((state.CityMask & (1 << toCity.City)) == 0)
                {
                    yield return new KeyValuePair<(int, int), float>((toCity.City, state.CityMask | (1 << toCity.City)), -toCity.Dist);
                }
            }
        }

        public override long Compute()
        {
            foreach (string path in File.ReadLines(DataFile))
            {
                var match = Regex.Match(path, "(.*) to (.*) = (.*)");

                if (match.Success)
                {
                    int dist = int.Parse(match.Groups[3].Value);

                    AddRoute(match.Groups[1].Value, match.Groups[2].Value, dist);
                    AddRoute(match.Groups[2].Value, match.Groups[1].Value, dist);
                }
                else throw new InvalidOperationException();
            }

            int allCityMask = (1 << cities.Count) - 1;

            int minCost = int.MaxValue;

            DijkstraSearch<(int, int)> search = new DijkstraSearch<(int, int)>(GetNeighborsMax);

            for (int startCity = 0; startCity < cities.Count; startCity++)
            {
                for (int endCity = startCity + 1; endCity < cities.Count; endCity++)
                {
                    List<(int, int)> path;
                    float cost;

                    if (search.GetShortestPath((startCity, (1 << startCity)), (endCity, allCityMask), out path, out cost))
                    {
                        if (cost < minCost)
                        {
                            minCost = (int)cost;
                        }
                    }
                }
            }

            return -minCost;
        }
    }
}
