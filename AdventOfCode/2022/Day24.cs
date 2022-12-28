using static System.Windows.Forms.AxHost;

namespace AdventOfCode._2022
{
    internal class Day24 : Day
    {
        int width;
        int height;

        BitArray[] left;
        BitArray[] right;
        BitArray[] up;
        BitArray[] down;

        void ReadInput(string file)
        {
            Grid<char> grid = new Grid<char>().CreateDataFromRows(File.ReadLines(file));

            width = grid.Width - 2;
            height = grid.Height - 2;

            left = new BitArray[height];
            right = new BitArray[height];
            up = new BitArray[width];
            down = new BitArray[width];

            for (int row = 0; row < left.Length; row++)
            {
                left[row] = new BitArray(width);
                right[row] = new BitArray(width);
            }

            for (int col = 0; col < up.Length; col++)
            {
                up[col] = new BitArray(height);
                down[col] = new BitArray(height);
            }

            foreach (var pos in grid.GetAll())
            {
                char c = grid[pos];

                switch (c)
                {
                    case '<':
                        left[pos.Y - 1].Set(pos.X - 1, true);
                        break;
                    case '>':
                        right[pos.Y - 1].Set(pos.X - 1, true);
                        break;
                    case '^':
                        up[pos.X - 1].Set(pos.Y - 1, true);
                        break;
                    case 'v':
                        down[pos.X - 1].Set(pos.Y - 1, true);
                        break;
                }
            }
        }
    
        char GetBlizzard(int col, int row, int time)
        {
            if (row == -1)
                return (col == 0) ? '.' : 'x'; // For start position

            if (row == height)
                return (col == (width - 1)) ? '.' : 'x';    // For end position

            int rowUpShift = ModHelper.PosMod(row + time, height);
            int rowDownShift = ModHelper.PosMod(row - time, height);

            int colLeftShift = ModHelper.PosMod(col + time, width);
            int colRightShift = ModHelper.PosMod(col - time, width);

            int count = 0;

            if (left[row][colLeftShift])
                count++;
            if (right[row][colRightShift])
                count++;
            if (up[col][rowUpShift])
                count++;
            if (down[col][rowDownShift])
                count++;

            if (count == 0)
            {
                return '.';
            }
            else if (count == 1)
            {
                if (left[row][colLeftShift])
                    return '<';
                else if (right[row][colRightShift])
                    return '>';
                else if (up[col][rowUpShift])
                    return '^';
                else if (down[col][rowDownShift])
                    return 'v';
            }

            return (char)('0' + count);
        }

        void PrintBlizzards(int time)
        {
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Console.Write(GetBlizzard(col, row, time));
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            Func<(int Col, int Row), IEnumerable<(int Col, int Row)>> allNeighborsAndSelf = delegate ((int Col, int Row) pos) { return Grid.AllNeighbors(pos).Append(pos); };

            DijkstraSearch<(int Col, int Row, int Time)> search = new DijkstraSearch<(int Col, int Row, int Time)>(delegate ((int Col, int Row, int Time) state)
            {
                return allNeighborsAndSelf((state.Col, state.Row)).Where(pos => (pos.Col >= 0) && (pos.Col < width) && (pos.Row >= -1) && (pos.Row <= height) && (GetBlizzard(pos.Col, pos.Row, state.Time + 1) == '.')).Select(pos => (pos.Col, pos.Row, state.Time + 1));
            });

            var result = search.GetShortestPath((0, -1, 0), delegate ((int Col, int Row, int Time) state) { return (state.Col == (width - 1)) && (state.Row == height); });

            int time = (int)result.Cost;    // Solution for part 1

            result = search.GetShortestPath((width - 1, height, time), delegate ((int Col, int Row, int Time) state) { return (state.Col == 0) && (state.Row == -1); });

            time += (int)result.Cost;

            result = search.GetShortestPath((0, -1, time), delegate ((int Col, int Row, int Time) state) { return (state.Col == (width - 1)) && (state.Row == height); });

            time += (int)result.Cost;

            return (long)time;
        }
    }
}
