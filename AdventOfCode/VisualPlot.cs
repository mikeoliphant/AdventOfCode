using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;

namespace AdventOfCode
{
    public class PlotDrawable
    {
        public virtual void Draw(SKCanvas canvas)
        {

        }
    }

    public class PlotPoints : PlotDrawable
    {
        public SKPoint[] Points { get; set; }
        public SKColor Color { get; set; }

        public override void Draw(SKCanvas canvas)
        {
            foreach (SKPoint point in Points)
                canvas.DrawPoint(point, Color);
        }
    }

    public class PlotLines : PlotDrawable
    {
        public (SKPoint Start, SKPoint End)[] Lines { get; set; }
        public SKPaint Paint { get; set;  }

        public override void Draw(SKCanvas canvas)
        {
            foreach (var line in Lines)
                canvas.DrawLine(line.Start, line.End, Paint);
        }
    }

    public class PlotRects : PlotDrawable
    {
        public SKRect[] Rects { get; set; }
        public SKPaint Paint { get; set; }

        public override void Draw(SKCanvas canvas)
        {
            foreach (var rect in Rects)
                canvas.DrawRect(rect, Paint);
        }
    }

    public class PlotDisplay
    {
        Form form;
        SKControl control;
        List<PlotDrawable> drawables = new();

        public SKMatrix Matrix { get; set; } = SKMatrix.Identity;

        public PlotDisplay(int width, int height)
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

        public void SetDisplayRegion(Vector2 min, Vector2 max)
        {
            float xScale = (float)control.Size.Width / (max.X - min.X);
            float yScale = (float)control.Size.Height / (max.Y - min.Y);

            Matrix = SKMatrix.Concat(SKMatrix.CreateScale(xScale, yScale), SKMatrix.CreateTranslation(-min.X, -min.Y));
        }

        public void AddDrawable(PlotDrawable drawable)
        {
            drawables.Add(drawable);
        }

        public void AddPoints(IEnumerable<Vector2> points, SKColor color)
        {
            AddDrawable(new PlotPoints()
            {
                Points = points.Select(p => new SKPoint(p.X, p.Y)).ToArray(),
                Color = color
            });
        }

        public void AddLines(IEnumerable<(Vector2 Start, Vector2 End)> lines, SKPaint paint)
        {
            AddDrawable(new PlotLines()
            {
                Lines = lines.Select(l => (new SKPoint(l.Start.X, l.Start.Y), new SKPoint(l.End.X, l.End.Y))).ToArray(),
                Paint = paint
            });
        }

        public void AddRects(IEnumerable<(Vector2 Min, Vector2 Max)> rects, SKPaint paint)
        {
            AddDrawable(new PlotRects()
            {
                Rects = rects.Select(r => (new SKRect(r.Min.X, r.Min.Y, r.Max.X, r.Max.Y))).ToArray(),
                Paint = paint
            });
        }

        private void Control_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            canvas.SetMatrix(Matrix);

            canvas.Clear(SKColors.White);

            foreach (PlotDrawable drawable in drawables)
            {
                drawable.Draw(canvas);
            }
        }
    }
}