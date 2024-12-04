using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Forms;

namespace AdventOfCode
{
    public class GridDisplay<T>
    {
        Form form;
        SKControl control;
        GridBase<T> grid;

        public GridDisplay(GridBase<T> grid, int width, int height)
        {
            this.grid = grid;

            form = new Form();
            form.AutoSize = true;
            form.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            control = new SKControl();
            control.Size = new Size(width, height);

            control.PaintSurface += Control_PaintSurface;

            form.Controls.Add(control);

            new Thread(new ThreadStart(delegate
            {
                Application.Run(form);
            })).Start();
        }

        private void Control_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.White);

            foreach (var pos in grid.GetAll())
            {
                if (!grid[pos].Equals(grid.DefaultValue))
                {
                    canvas.DrawPoint(pos.X, pos.Y, SKColors.Black);
                }
            }
        }
    }

    public class VisualSparseGrid<T> : SparseGrid<T>
    {
        public Dictionary<T, SKColor> Colors { get; set;  }

        Form form;
        SKControl control;

        public VisualSparseGrid(int width, int height)
        {
            form = new Form();
            form.AutoSize = true;
            form.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            control = new SKControl();
            control.Size = new Size(width, height);

            control.PaintSurface += Control_PaintSurface;

            form.Controls.Add(control);

            new Thread(new ThreadStart(delegate
            {
                Application.Run(form);
            })).Start();
        }

        private void Control_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (!wait)
                return;

            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.White);

            foreach (var pos in GetAll())
            {
                T value = data[pos];

                if ((Colors != null) && Colors.ContainsKey(value))
                {
                    canvas.DrawPoint(pos.Y, pos.X, Colors[data[pos]]);
                }
                else
                {
                    canvas.DrawPoint(pos.Y, pos.X, SKColors.Black);
                }
            }

            wait = false;
        }

        bool wait = false;

        public void ReDraw()
        {
            control.Invalidate();

            wait = true;

            while (wait)
            {

            }
        }
    }
}
