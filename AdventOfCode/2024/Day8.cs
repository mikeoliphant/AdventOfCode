namespace AdventOfCode._2024
{
    internal class Day8 : Day
    {
        Grid<char> grid = null;
        List<char> antennas = null;
        Dictionary<char, List<(int X, int Y)>> antennaLocations = new();

        void ReadData()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            antennas = grid.GetAllValues().Distinct().Where(a => a != '.').ToList();

            foreach (char antenna in antennas)
            {
                antennaLocations[antenna] = grid.FindValue(antenna).ToList();
            }
        }

        public override long Compute()
        {
            ReadData();

            foreach (char antenna in antennas)
            {
                var locations = antennaLocations[antenna];

                for (int a1 = 0; a1 < locations.Count; a1++)
                {
                    for (int a2 = a1 + 1; a2 < locations.Count; a2++)
                    {
                        var p1 = new IntVec2(locations[a1]);
                        var p2 = new IntVec2(locations[a2]);

                        var ap1 = p1 + (p1 - p2);
                        var ap2 = p2 + (p2 - p1);

                        if (grid.IsValid(ap1.X, ap1.Y))
                        {
                            grid[ap1.X, ap1.Y] = '#';
                        }

                        if (grid.IsValid(ap2.X, ap2.Y))
                        {
                            grid[ap2.X, ap2.Y] = '#';
                        }
                    }
                }
            }


            grid.PrintToConsole();

            long num = grid.FindValue('#').Count();

            return num;
        }

        void AddAntinodes(IntVec2 pos, IntVec2 delta)
        {
            do
            {
                grid[pos.X, pos.Y] = '#';
;
                pos += delta;
            }
            while (grid.IsValid(pos.X, pos.Y));
        }

        public override long Compute2()
        {
            ReadData();

            foreach (char antenna in antennas)
            {
                var locations = antennaLocations[antenna];

                for (int a1 = 0; a1 < locations.Count; a1++)
                {
                    for (int a2 = a1 + 1; a2 < locations.Count; a2++)
                    {
                        var p1 = new IntVec2(locations[a1]);
                        var p2 = new IntVec2(locations[a2]);

                        AddAntinodes(p1, (p1 - p2));
                        AddAntinodes(p2, (p2 - p1));
                    }
                }
            }


            grid.PrintToConsole();

            long num = grid.FindValue('#').Count();

            return num;
        }
    }
}
