namespace AdventOfCode._2016
{
    internal class Day2
    {

        public long Compute()
        {
            Grid<char> keyPad = new Grid<char>().CreateDataFromRows(new string[] { "123", "456", "789" });

            LongVec2 pos = new LongVec2(1, 1);

            string code = "";

            foreach (string instruction in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day2.txt"))
            {
                foreach (char dir in instruction)
                {
                    switch (dir)
                    {
                        case 'U':
                            if (pos.Y > 0)
                                pos.Y--;
                            break;

                        case 'D':
                            if (pos.Y < 2)
                                pos.Y++;
                            break;

                        case 'L':
                            if (pos.X > 0)
                                pos.X--;
                            break;

                        case 'R':
                            if (pos.X < 2)
                                pos.X++;
                            break;
                    }
                }

                code += keyPad[(int)pos.X, (int)pos.Y];
            }

            return 0;
        }

        public long Compute2()
        {
            Grid<char> keyPad = new Grid<char>().CreateDataFromRows(new string[] { "  1  ", " 234 ", "56789", " ABC ", "  D  "});

            LongVec2 pos = new LongVec2(1, 1);

            string code = "";

            foreach (string instruction in File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day2.txt"))
            {
                foreach (char dir in instruction)
                {
                    LongVec2 lastPos = pos;

                    switch (dir)
                    {
                        case 'U':
                            if (pos.Y > 0)
                                pos.Y--;
                            break;

                        case 'D':
                            if (pos.Y < 4)
                                pos.Y++;
                            break;

                        case 'L':
                            if (pos.X > 0)
                                pos.X--;
                            break;

                        case 'R':
                            if (pos.X < 4)
                                pos.X++;
                            break;
                    }

                    if (keyPad[(int)pos.X, (int)pos.Y] == ' ')
                        pos = lastPos;
                }

                code += keyPad[(int)pos.X, (int)pos.Y];
            }

            return 0;
        }
    }
}
