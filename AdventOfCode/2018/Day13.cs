namespace AdventOfCode._2018
{
    internal class Day13
    {
        class Cart
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Dir { get; set; }
            public int LSR { get; set; }
            public bool Crashed { get; set; }

            public override string ToString()
            {
                return X + "," + Y + "," + Dir;
            }
        }

        Grid<char> grid;
        List<Cart> carts = new List<Cart>();

        void ReadInput()
        {
            grid = new Grid<char>().CreateDataFromRows(File.ReadLines(@"C:\Code\AdventOfCode\Input\2018\Day13.txt"));

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    switch (grid[x, y])
                    {
                        case '^':
                            grid[x, y] = '|';

                            carts.Add(new Cart { X = x, Y = y, Dir = 0 });
                            break;

                        case 'v':
                            grid[x, y] = '|';

                            carts.Add(new Cart { X = x, Y = y, Dir = 2 });
                            break;

                        case '>':
                            grid[x, y] = '-';

                            carts.Add(new Cart { X = x, Y = y, Dir = 1 });
                            break;

                        case '<':
                            grid[x, y] = '-';

                            carts.Add(new Cart { X = x, Y = y, Dir = 3 });
                            break;
                    }
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            int numTicks = 0;

            do
            {
                foreach (Cart cart in carts.Where(c => c.Crashed).ToArray())
                    carts.Remove(cart);

                if (carts.Count == 1)
                    break;

                carts.Sort((a, b) => ((a.Y * grid.Width) + a.X).CompareTo((b.Y * grid.Width) + b.X));

                foreach (Cart cart in carts)
                {
                    if (cart.Crashed)
                        continue;

                    char currentTrack = grid[cart.X, cart.Y];

                    switch (currentTrack)
                    {
                        case '/':
                            switch (cart.Dir)
                            {
                                case 0:
                                    cart.Dir = 1;
                                    break;
                                case 1:
                                    cart.Dir = 0;
                                    break;
                                case 2:
                                    cart.Dir = 3;
                                    break;
                                case 3:
                                    cart.Dir = 2;
                                    break;
                            }
                            break;

                        case '\\':
                            switch (cart.Dir)
                            {
                                case 0:
                                    cart.Dir = 3;
                                    break;
                                case 1:
                                    cart.Dir = 2;
                                    break;
                                case 2:
                                    cart.Dir = 1;
                                    break;
                                case 3:
                                    cart.Dir = 0;
                                    break;
                            }
                            break;

                        case '+':
                            switch (cart.LSR)
                            {
                                case 0:
                                    cart.Dir--;
                                    if (cart.Dir == -1)
                                        cart.Dir = 3;
                                    break;
                                case 1:
                                    break;
                                case 2:
                                    cart.Dir++;
                                    if (cart.Dir == 4)
                                        cart.Dir = 0;
                                    break;
                            }

                            cart.LSR = (cart.LSR + 1) % 3;

                            break;
                    }

                    int dx = 0;
                    int dy = 0;

                    switch (cart.Dir)
                    {
                        case 0:
                            dy = -1;
                            break;
                        case 1:
                            dx = 1;
                            break;
                        case 2:
                            dy = 1;
                            break;
                        case 3:
                            dx = -1;
                            break;
                    }

                    cart.X += dx;
                    cart.Y += dy;

                    foreach (Cart cart2 in carts)
                    {
                        if (cart.Crashed)
                            continue;

                        if (cart == cart2)
                            continue;

                        if ((cart.X == cart2.X) && (cart.Y == cart2.Y))
                        {
                            cart.Crashed = true;
                            cart2.Crashed = true;
                        }
                    }
                }

                numTicks++;
            }
            while (true);

            return 0;
        }
    }
}
