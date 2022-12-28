using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace AdventOfCode._2022
{
    internal class Day22 : Day
    {
        SparseGrid<char> grid;
        IEnumerable<string> commands;

        (int MinX, int MinY, int MaxX, int MaxY) bounds;

        void ReadInput(string file)
        {
            var sections = File.ReadAllText(file).SplitParagraphs();

            grid = new SparseGrid<char>().CreateDataFromRows(sections[0].SplitLines());

            grid.DefaultValue = ' ';

            foreach (var pos in grid.FindValue(' '))
            {
                grid.RemoveValue(pos);
            }

            var match = Regex.Matches(sections[1].Trim(), @"\d+|R|L");

            commands = match.Select(m => m.Value);
        }

        int GetMinX(int y)
        {
            for (int x = bounds.MinX; x <= bounds.MaxX; x++)
            {
                if (grid.IsValid((x, y)))
                    return x;
            }

            throw new Exception();
        }

        int GetMaxX(int y)
        {
            for (int x = bounds.MaxX; x >= bounds.MinX; x--)
            {
                if (grid.IsValid((x, y)))
                    return x;
            }

            throw new Exception();
        }

        int GetMinY(int x)
        {
            for (int y = bounds.MinY; y <= bounds.MaxY; y++)
            {
                if (grid.IsValid((x, y)))
                    return y;
            }

            throw new Exception();
        }

        int GetMaxY(int x)
        {
            for (int y = bounds.MaxY; y >= bounds.MinY; y--)
            {
                if (grid.IsValid((x, y)))
                    return y;
            }

            throw new Exception();
        }

        long WalkPath(LongVec2 pos, int facing, Func<(LongVec2 Pos, int Facing), (LongVec2 Pos, int Facing)> getNeighbor)
        {
            foreach (string command in commands)
            {
                switch (command)
                {
                    case "R":
                        facing = LongVec2.TurnFacing(facing, 1);
                        break;
                    case "L":
                        facing = LongVec2.TurnFacing(facing, -1);
                        break;
                    default:
                        int num = int.Parse(command);

                        while (num > 0)
                        {
                            LongVec2 neighbor = pos;

                            neighbor.AddFacing(facing, 1);

                            char val;

                            if (grid.TryGetValue((int)neighbor.X, (int)neighbor.Y, out val))
                            {
                                if (val == '#')
                                {
                                    break;
                                }

                                pos = neighbor;
                            }
                            else
                            {
                                var next = getNeighbor((pos, facing));

                                if (grid.TryGetValue((int)next.Pos.X, (int)next.Pos.Y, out val))
                                {
                                    if (val == '#')
                                    {
                                        break;
                                    }
                                }

                                pos = next.Pos;
                                facing = next.Facing;
                            }

                            num--;
                        }

                        break;
                }
            }

            return ((pos.Y + 1) * 1000) + ((pos.X + 1) * 4) + LongVec2.TurnFacing(facing, -1);
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            grid.GetBounds();

            bounds = grid.GetBounds();

            //grid.PrintToConsole();

            LongVec2 pos = new LongVec2(grid.GetAll().OrderBy(p => p.Y).ThenBy(p => p.X).First());

            int facing = 1;

            return WalkPath(pos, facing, delegate ((LongVec2 Pos, int Facing) state)
            {
                pos.AddFacing(facing, 1);

                switch (state.Facing)
                {
                    case 0:
                        state.Pos.Y = GetMaxY((int)state.Pos.X);
                        break;
                    case 1:
                        state.Pos.X = GetMinX((int)state.Pos.Y);
                        break;
                    case 2:
                        state.Pos.Y = GetMinY((int)state.Pos.X);
                        break;
                    case 3:
                        state.Pos.X = GetMaxX((int)state.Pos.Y);
                        break;
                }

                return state;
            });
        }

        void RenderFace(char number, int col, int row, int size)
        {
            foreach (var pos in Grid.GetRectangle(new Rectangle(col * size, row * size, size, size)))
            {
                faceGrid[pos] = number;
            }
        }

        SparseGrid<char> faceGrid = new SparseGrid<char>();

        public override long Compute2()
        {
            ReadInput(DataFileTest);

            grid.GetBounds();

            bounds = grid.GetBounds();

            //grid.PrintToConsole();

            faceGrid.DefaultValue = ' ';

            RenderFace('1', 2, 0, 4);
            RenderFace('2', 0, 1, 4);
            RenderFace('3', 1, 1, 4);
            RenderFace('4', 2, 1, 4);
            RenderFace('5', 2, 2, 4);
            RenderFace('6', 3, 2, 4);

            //faceGrid.PrintToConsole();

            LongVec2 pos = new LongVec2(grid.GetAll().OrderBy(p => p.Y).ThenBy(p => p.X).First());

            int facing = 1;

            return WalkPath(pos, facing, delegate ((LongVec2 Pos, int Facing) state)
            {
                switch (faceGrid[(int)state.Pos.X, (int)state.Pos.Y])
                {
                    case '1':
                        switch (facing)
                        {
                            case 0:

                                break;
                        }
                        break;
                    case '2':
                        break;
                    case '3':
                        break;
                    case '4':
                        break;
                    case '5':
                        break;
                    case '6':
                        break;
                }

                return state;
            });
        }
    }
}
