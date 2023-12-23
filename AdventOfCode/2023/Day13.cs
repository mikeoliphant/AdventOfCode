using static OpenTK.Graphics.OpenGL.GL;

namespace AdventOfCode._2023
{
    internal class Day13 : Day
    {
        bool Reflects(Grid<char> grid, int x)
        {
            for (int r1 = x + 1, r2 = x; ((r1 < grid.Width) && (r2 >= 0)); r1++, r2--)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[r1, y] != grid[r2, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        IEnumerable<int> FindVerticallReflection(Grid<char> grid)
        {
            for (int x = 0; x < grid.Width - 1; x++)
            {
                if (Reflects(grid, x))
                {
                    yield return x;
                }
            }
        }

        IEnumerable<int> GetReflectValue(Grid<char> grid)
        {
            foreach (int reflectX in FindVerticallReflection(grid))
            {
               yield return reflectX + 1;
            }

            Grid<char> rotate = new Grid<char>(grid.Height, grid.Width);

            foreach (var cell in grid.GetAll())
            {
                rotate[cell.Y, cell.X] = grid[cell];
            }

            foreach (int reflectY in FindVerticallReflection(rotate))
            {
                yield return (reflectY + 1) * 100;
            }
        }

        public override long Compute()
        {
            long sum = 0;

            foreach (string gridStr in File.ReadAllText(DataFile).SplitParagraphs())
            {
                Grid<char> grid = new();

                grid.CreateDataFromRows(gridStr.SplitLines());

                //grid.PrintToConsole();

                sum += GetReflectValue(grid).First();
            }

            return sum;
        }

        public override long Compute2()
        {
            long sum = 0;

            foreach (string gridStr in File.ReadAllText(DataFile).SplitParagraphs())
            {
                Grid<char> grid = new();

                grid.CreateDataFromRows(gridStr.SplitLines());

                int unsmudgedValue = GetReflectValue(grid).First();

                bool haveReflect = false;

                foreach (var smudge in grid.GetAll())
                {
                    var bak = grid[smudge];

                    if (grid[smudge] == '.')
                        grid[smudge] = '#';
                    else
                        grid[smudge] = '.';

                    foreach (int smudgedValue in GetReflectValue(grid))
                    {
                        if (smudgedValue != unsmudgedValue)
                        {
                            sum += smudgedValue;

                            haveReflect = true;

                            break;
                        }
                    }

                    grid[smudge] = bak;

                    if (haveReflect)
                        break;
                }

                if (!haveReflect)
                {
                    throw new InvalidDataException();
                }
            }

            return sum;
        }
    }
}