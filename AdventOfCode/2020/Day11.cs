namespace AdventOfCode._2020
{
    public class Day11
    {
        int width;
        int height;

        char[,] seats;

        void ReadInput()
        {
            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day11.txt").ToArray();

            width = lines[0].Length;
            height = lines.Length;

            seats = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    seats[x, y] = lines[y][x];
                }
            }
        }

        char GetSeat(int x, int y)
        {
            if ((x < 0) || (x >= width))
                return '\0';

            if ((y < 0) || (y >= height))
                return '\0';

            return seats[x, y];
        }

        char[,] CycleSeats()
        {
            char[,] newSeats = seats.Clone() as char[,];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbors = 0;

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if ((dx != 0) || (dy != 0))
                            {
                                if (GetSeat(x + dx, y + dy) == '#')
                                    neighbors++;
                            }
                        }
                    }

                    char seat = GetSeat(x, y);

                    if ((seat == '#') && (neighbors > 3))
                        newSeats[x, y] = 'L';
                    else if ((seat == 'L') && (neighbors == 0))
                        newSeats[x, y] = '#';
                }
            }

            return newSeats;
        }

        char Project(int x, int y, int dx, int dy)
        {
            do
            {
                x += dx;
                y += dy;

                char c = GetSeat(x, y);

                if (c != '.')
                    return c;

            }
            while (true);
        }

        char[,] CycleSeats2()
        {
            char[,] newSeats = seats.Clone() as char[,];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbors = 0;

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if ((dx != 0) || (dy != 0))
                            {
                                if (Project(x, y, dx, dy) == '#')
                                    neighbors++;
                            }
                        }
                    }

                    char seat = GetSeat(x, y);

                    if ((seat == '#') && (neighbors > 4))
                        newSeats[x, y] = 'L';
                    else if ((seat == 'L') && (neighbors == 0))
                        newSeats[x, y] = '#';
                }
            }

            return newSeats;
        }

        public long Compute()
        {
            ReadInput();

            bool stable = true;

            do
            {
                stable = true;

                char[,] newSeats = CycleSeats2();

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (seats[x, y] != newSeats[x, y])
                        {
                            stable = false;

                            break;
                        }
                    }
                }

                seats = newSeats;

                //for (int y = 0; y < height; y++)
                //{
                //    for (int x = 0; x < width; x++)
                //    {
                //        Console.Write(seats[x, y]);
                //    }

                //    Console.WriteLine();
                //}

                //Console.WriteLine();
            }
            while (!stable);

            int occupiedCount = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (seats[x, y] == '#')
                        occupiedCount++;
                }
            }

            return occupiedCount;
        }
    }
}
