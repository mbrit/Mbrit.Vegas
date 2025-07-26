

namespace Mbrit.Vegas.Lens.Graph
{
    internal class GdiGraphics : IGraphics
    {
        private Graphics Graphics { get; }

        public GdiGraphics(Graphics g)
        {
            this.Graphics = g;
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2) =>
            this.Graphics.DrawLine(pen, x1, y1, x2, y2);

        public void DrawRectangle(Pen pen, RectangleF rect) =>
            this.Graphics.DrawRectangle(pen, rect);

        public void DrawString(string label, Font font, Brush brush, float x, float y, StringFormat format) =>
            this.Graphics.DrawString(label, font, brush, x, y, format);

        public void FillEllipse(Brush brush, float v1, float v2, int size1, int size2) =>
            this.Graphics.FillEllipse(brush, v1, v2, size1, size2);

        public void FillPolygon(Brush brush, PointF[] points) =>
            this.Graphics.FillPolygon(brush, points);

        public void DrawArrow(Color color, PointF start, PointF end)
        {
            using (var pen = new Pen(color, 3))
            {
                using (var brush = new SolidBrush(color))
                {
                    this.Graphics.DrawLine(pen, start, end);

                    const float size = 6f;

                    var poly = new List<PointF>();
                    poly.Add(end);

                    if (end.Y > start.Y)
                    {
                        poly.Add(new PointF(end.X - size, end.Y - size));
                        poly.Add(new PointF(end.X + size, end.Y - size));
                    }
                    else
                    {
                        poly.Add(new PointF(end.X - size, end.Y + size));
                        poly.Add(new PointF(end.X + size, end.Y + size));
                    }

                    this.Graphics.FillPolygon(brush, poly.ToArray());
                }
            }
        }
    }
}