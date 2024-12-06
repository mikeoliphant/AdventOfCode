using SkiaSharp;

namespace AdventOfCode._2023
{
    internal class Day18 : Day
    {
        void FloodFill(GridBase<char> grid, (int X, int Y) pos)
        {
            Stack<(int X, int Y)> cellStack = new();

            cellStack.Push(pos);

            while (cellStack.Count > 0)
            {
                var cell = cellStack.Pop();

                if (grid.GetValue(cell) == '.')
                {
                    grid[cell] = '#';

                    foreach (var neighbor in Grid.AllNeighbors(cell.X, cell.Y, includeDiagonal: false))
                    {
                        cellStack.Push(neighbor);
                    }
                }
            }
        }

        IEnumerable<(int DX, int DY, int Dist)> GetInstructions(string dataFile)
        {
            foreach (string instruction in File.ReadLines(dataFile))
            {
                string[] split = instruction.Split(' ');

                int dx = 0;
                int dy = 0;

                switch (split[0][0])
                {
                    case 'U':
                        dy = -1;
                        break;
                    case 'D':
                        dy = 1;
                        break;
                    case 'L':
                        dx = -1;
                        break;
                    case 'R':
                        dx = 1;
                        break;
                }

                yield return (dx, dy, int.Parse(split[1]));
            }
        }

        public override long Compute()
        {
            SparseGrid<char> grid = new();
            grid.DefaultValue = '.';

            (int X, int Y) cell = (0, 0);

            grid[cell] = '#';

            var instructions = GetInstructions(DataFile);

            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Dist; i++)
                {
                    cell = (cell.X + instruction.DX, cell.Y + instruction.DY);

                    grid[cell] = '#';
                }
            }

            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Dist; i++)
                {
                    cell = (cell.X + instruction.DX, cell.Y + instruction.DY);

                    var flood = (cell.X - instruction.DY, cell.Y + instruction.DX);

                    FloodFill(grid, flood);
                }
            }

            Grid<char> dugGrid = grid.ToGrid();

            GridDisplay display = new GridDisplay(dugGrid, dugGrid.Width, dugGrid.Height);

            long count = dugGrid.FindValue('#').LongCount();

            return count;
        }


        IEnumerable<(int DX, int DY, int Dist)> GetInstructions2(string dataFile)
        {
            foreach (string instruction in File.ReadLines(dataFile))
            {
                string[] split = instruction.Split(' ');

                int dx = 0;
                int dy = 0;

                switch (split[2][7])
                {
                    case '3':
                        dy = -1;
                        break;
                    case '1':
                        dy = 1;
                        break;
                    case '2':
                        dx = -1;
                        break;
                    case '0':
                        dx = 1;
                        break;
                }

                yield return (dx, dy, int.Parse(split[2].Substring(2, 5), System.Globalization.NumberStyles.HexNumber));
            }
        }

        List<LineHV> lines = new();

        public override long Compute2()
        {
            SparseGrid<char> grid = new();
            grid.DefaultValue = '.';

            var instructions = GetInstructions2(DataFile);

            LongVec2 cell = new LongVec2(0, 0);

            long dugCount = 0;

            foreach (var instruction in instructions)
            {
                LongVec2 next = cell + (new LongVec2(instruction.DX, instruction.DY) * instruction.Dist);

                lines.Add(new LineHV(cell, next));

                cell = next;
            }

            PlotDisplay plot = new PlotDisplay(1024, 800);

            float min = lines.Select(l => l.Range.Min).Min();
            float max = lines.Select(l => l.Range.Max).Max();

            float delta = max - min;

            min -= (delta * 0.1f);
            max += (delta * 0.1f);

            plot.SetDisplayRegion(new Vector2(min, min), new Vector2(max, max));

            var drawLines = lines.Select(l => l.IsVertical ?
                (new Vector2(l.AxisValue, l.Range.Min), new Vector2(l.AxisValue, l.Range.Max)) :
                (new Vector2(l.Range.Min, l.AxisValue), new Vector2(l.Range.Max, l.AxisValue))).ToList();

            plot.AddLines(drawLines, new SKPaint { Color = SKColors.Black });

            List<long> yVals = new();

            foreach (var line in lines)
            {
                if (line.IsVertical)
                {
                    yVals.Add(line.Range.Min);
                    yVals.Add(line.Range.Max);
                }
            }

            yVals = yVals.Distinct().Order().ToList();

            var horizLines = yVals.Select(y => (new Vector2(min, y), new Vector2(max, y)));

            //plot.AddLines(horizLines, new SKPaint { Color = new SKColor(255, 0, 0, 128) });

            long? lastY = null;

            List<(Vector2, Vector2)> rects = new();

            foreach (long y in yVals)
            {
                if (lastY != null)
                {
                    int crossCount = 0;

                    long lastCross = 0;

                    foreach (var cross in lines.Where(l => (l.IsVertical && (l.Range.Min <= lastY) && l.Range.Max >= y)).OrderBy(l => l.AxisValue))
                    {
                        crossCount++;

                        if ((crossCount % 2) == 0)
                        {
                            // Every other cross

                            dugCount += (cross.AxisValue - lastCross + 1) * (y - lastY.Value + 1);

                            rects.Add((new Vector2(lastCross, lastY.Value), new Vector2(cross.AxisValue, y)));
                        }

                        lastCross = cross.AxisValue;
                    }
                }

                lastY = y;
            }

            plot.AddRects(rects, new SKPaint { Color = new SKColor(255, 255, 0, 128) });

            return dugCount;
        }
    }
}
