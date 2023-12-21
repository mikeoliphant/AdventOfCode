namespace AdventOfCode._2023
{
    internal class Day10 : Day
    {
        Grid<char> grid = new();
        Grid<char> pathGrid;

        Dictionary<char, Grid<char>> pipeGrids = new();

        void Init()
        {
            grid.CreateDataFromRows(File.ReadLines(DataFile));

            pathGrid = new Grid<char>(grid.Width * 3, grid.Height * 3);
            pathGrid.Fill('.');

            foreach (string path in Directory.GetFiles(Path.Combine(DataFileDir, "Day" + DayNumber)))
            {
                char c = (char)int.Parse(Path.GetFileNameWithoutExtension(path));

                pipeGrids[c] = new();
                pipeGrids[c].CreateDataFromRows(File.ReadLines(path));
            }
        }

        IEnumerable<(int X, int Y)> GetNeighbors((int X, int Y) cell)
        {
            char c = grid[cell];

            switch (c)
            {
                case '|':
                    yield return (cell.X, cell.Y - 1);
                    yield return (cell.X, cell.Y + 1);
                    break;
                case '-':
                    yield return (cell.X + 1, cell.Y);
                    yield return (cell.X - 1, cell.Y);
                    break;
                case 'L':
                    yield return (cell.X, cell.Y - 1);
                    yield return (cell.X + 1, cell.Y);
                    break;
                case 'J':
                    yield return (cell.X - 1, cell.Y);
                    yield return (cell.X, cell.Y - 1);
                    break;
                case '7':
                    yield return (cell.X - 1, cell.Y);
                    yield return (cell.X, cell.Y + 1);
                    break;
                case 'F':
                    yield return (cell.X + 1, cell.Y);
                    yield return (cell.X, cell.Y + 1);
                    break;
                case 'S':
                    yield return (cell.X, cell.Y - 1);
                    yield return (cell.X, cell.Y + 1);
                    yield return (cell.X + 1, cell.Y);
                    yield return (cell.X - 1, cell.Y);
                    break;
            }
        }

        public override long Compute()
        {
            Init();

            //grid.PrintToConsole();

            Grid<char> clearedGrid = new Grid<char>(grid.Width, grid.Height);
            clearedGrid.Fill('.');

            var currentCell = grid.FindValue('S').First();

            var last = currentCell;

            int steps = 0;

            do
            {
                Grid<char>.Copy(pipeGrids[grid[currentCell]], pathGrid, currentCell.X * 3, currentCell.Y * 3);
                clearedGrid[currentCell] = grid[currentCell];

                foreach (var cell in GetNeighbors(currentCell).Where(c => grid.IsValid(c)))
                {
                    if (cell == last)
                        continue;

                    var cellNeighbors = GetNeighbors(cell).Where(c => grid.IsValid(c));

                    if (cellNeighbors.Contains(currentCell))
                    {
                        last = currentCell;

                        currentCell = cell;

                        steps++;

                        break;
                    }
                }
            }
            while (grid[currentCell] != 'S');

            grid = clearedGrid;

            return steps / 2;
        }

        public override long Compute2()
        {
            Compute();

            //pathGrid.PrintToConsole();

            DijkstraSearch<(int X, int Y)> search = new(delegate ((int X, int Y) cell) { return Grid.AllNeighbors(cell.X, cell.Y, includeDiagonal: true).Where(c => pathGrid.GetValue(c) != '#'); });

            int numInternal = 0;

            var blah = grid.FindValue('.');

            foreach (var cell in grid.FindValue('.'))
            {
                var result = search.GetShortestPath((cell.X * 3, cell.Y * 3), delegate ((int X, int Y) c) { return !pathGrid.IsValid(c); });

                if (result.Path == null)
                {
                    grid[cell] = '*';

                    numInternal++;
                }
            }

            //grid.PrintToConsole();

            return numInternal;
        }
    }
}
