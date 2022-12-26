using Microsoft.VisualBasic.Logging;
using OpenTK.Graphics.OpenGL;

namespace AdventOfCode._2022
{
    internal class Day17 : Day
    {
        List<Grid<char>> shapes = new List<Grid<char>>();
        SparseGrid<char> chamber = new SparseGrid<char> { DefaultValue = '.' };

        void ReadShapes()
        {
            foreach (string shape in File.ReadAllText(Path.Combine(DataFileDir, "Day" + DayNumber + "Shapes.txt")).SplitParagraphs())
            {
                var shapeGrid = new Grid<char>().CreateDataFromRows(shape.SplitLines());
                shapeGrid.DefaultValue = '.';

                shapes.Add(shapeGrid);
            }
        }

        bool ExecuteTurn(char command, Grid<char> shape, ref LongVec2 shapeOffset)
        {
            int dx = 0;

            var bounds = chamber.GetBounds();

            switch (command)
            {
                case '>':
                    dx = 1;
                    break;
                case '<':
                    dx = -1;
                    break;
                default:
                    throw new Exception();                   
            }

            foreach (var pos in shape.GetAll())
            {
                if (shape[pos] == '#')
                {
                    int x = pos.X + (int)shapeOffset.X + dx;

                    if ((x < bounds.MinX) || (x > bounds.MaxX))
                    {
                        dx = 0;

                        break;
                    }

                    if (chamber.GetValue((int)(pos.X + shapeOffset.X + dx), (int)(pos.Y + shapeOffset.Y)) == '#')
                    {
                        dx = 0;

                        break;
                    }
                }
            }

            shapeOffset.X += dx;

            foreach (var pos in shape.GetAll())
            {
                if (shape[pos] == '#')
                {
                    if (chamber.GetValue((int)(pos.X + shapeOffset.X), (int)(pos.Y + shapeOffset.Y + 1)) == '#')
                    {
                        return true;
                    }
                }
            }

            shapeOffset.Y++;

            return false;
        }

        long GetHeight(string wind, long numRocks)
        {
            for (int i = 0; i < 7; i++)
                chamber[i, 0] = '#';

            int windPos = 0;

            int rocks = 0;
            int shapePos = 0;
            Grid<char> currentShape = null;
            LongVec2 shapeOffset = LongVec2.Zero;

            long lastRocks = 0;
            long windTot = 0;
            long lastHeight = 0;

            do
            {
                if (currentShape == null)
                {
                    currentShape = shapes[shapePos];

                    var bounds = chamber.GetBounds();

                    shapeOffset = new LongVec2(bounds.MinX + 2, bounds.MinY - currentShape.Height - 3);
                }

                //var copy = new SparseGrid<char>(chamber);
                //Grid<char>.Copy(currentShape, copy, (int)shapeOffset.X, (int)shapeOffset.Y);

                //copy.PrintToConsole();

                if (ExecuteTurn(wind[windPos], currentShape, ref shapeOffset))
                {
                    Grid<char>.Copy(currentShape, chamber, (int)shapeOffset.X, (int)shapeOffset.Y);
                    currentShape = null;

                    shapePos = (shapePos + 1) % shapes.Count;

                    //chamber.PrintToConsole();

                    rocks++;
                }

                windTot++;
                windPos = (windPos + 1) % wind.Length;

                if ((windTot % (wind.Length * 1)) == 0)
                {
                    var bounds = chamber.GetBounds();

                    int height = bounds.MaxY - bounds.MinY;

                    Console.WriteLine("Rocks: " + (rocks - lastRocks) + "  Height: " + (height - lastHeight));

                    lastRocks = rocks;
                    lastHeight = height;
                }
            }
            while (rocks < numRocks);

            var finalBounds = chamber.GetBounds();

            return finalBounds.MaxY - finalBounds.MinY;
        }

        public override long Compute()
        {
            ReadShapes();

            string wind = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
            //string wind = File.ReadAllText(DataFile).Trim();


            long height = GetHeight(wind, 2022);

            chamber.PrintToConsole();

            return height;
        }

        public override long Compute2()
        {
            ReadShapes();

            //string wind = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
            string wind = File.ReadAllText(DataFile).Trim();


            // The rock pattern cyles over a multiple of the wind length - 5x for example, 1x for my input           

            //GetHeight(wind, 50000);

            // Calculations for the sample:

            //long remainder = (1000000000000 - 36) % 35;

            //long height = GetHeight(wind, 71 + remainder);

            //long numCycles = (1000000000000 - (71 + remainder)) / 35;

            //height += numCycles * 53;


            long remainder = (1000000000000 - 1723) % 1725;

            long height = GetHeight(wind, 1723 + 1725 + remainder);

            long numCycles = (1000000000000 - (1723 + 1725 + remainder)) / 1725;

            height += numCycles * 2709;

            return height;
        }
    }
}
