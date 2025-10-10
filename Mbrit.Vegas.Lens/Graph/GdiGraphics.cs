using BootFX.Common;
using Mbrit.Vegas.Lens.Gdi;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;

namespace Mbrit.Vegas.Lens.Graph
{
    internal class GdiGraphics : IGraphics
    {
        internal Graphics Graphics { get; }

        public XRectangleF ClientRect { get; }

        internal GdiGraphics(Graphics g)
        {
            this.Graphics = g;
            this.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            this.ClientRect = new XRectangleF(0, 0, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);
        }

        public void DrawLine(XPen pen, float x1, float y1, float x2, float y2) =>
            this.Graphics.DrawLine(pen, x1, y1, x2, y2);

        public void DrawRectangle(XPen pen, XRectangleF rect) =>
            this.Graphics.DrawRectangle(pen, rect);

        public void FillRectangle(XBrush brush, XRectangleF rect) =>
            this.Graphics.FillRectangle(brush, rect);

        public void DrawString(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format) =>
            this.Graphics.DrawString(label, font, brush, rect, format);

        public void DrawString(string label, XFont font, XBrush brush, float x, float y, XStringFormat format) =>
            this.Graphics.DrawString(label, font, brush, x, y, format);

        public void DrawStringTight(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            var sf = new StringFormat(StringFormat.GenericTypographic)
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (var theFont = font.ToFont())
            {
                // Convert point size to world units (pixels) properly
                float emSizePx = theFont.SizeInPoints * this.Graphics.DpiY / 72f;

                // Build glyph outlines centered at (0,0)
                path.AddString(label, theFont.FontFamily, (int)theFont.Style, emSizePx, new PointF(0, 0), sf);
            }

            using(var theBrush = brush.ToBrush())
                this.Graphics.FillPath(theBrush, path);
        }

        public void FillEllipse(XBrush brush, float v1, float v2, int size1, int size2) =>
            this.Graphics.FillEllipse(brush, v1, v2, size1, size2);

        public void FillPolygon(XBrush brush, XPointF[] points) =>
            this.Graphics.FillPolygon(brush, points);

        public XSizeF MeasureString(string buf, XFont font)
        {
            using (var theFont = font.ToFont())
            {
                var size = this.Graphics.MeasureString(buf, theFont);
                return new XSizeF(size.Width, size.Height);
            }
        }

        public IDisposable RotateAround(float x, float y, float angle)
        {
            var state = this.Graphics.Save();

            this.Graphics.TranslateTransform(x, y);
            this.Graphics.RotateTransform(angle);

            return new NullDisposer(() =>
            {
                this.Graphics.Restore(state);
            });
        }

        private GraphicsPath GetRoundedRectPath(RectangleF rect, float radiusPx)
        {
            var path = new GraphicsPath();
            float r = Math.Min(radiusPx, Math.Min(rect.Width, rect.Height) / 2f);
            float d = 2f * r;

            // Top-left
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            // Top-right
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            // Bottom-right
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            // Bottom-left
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }

        internal void DrawRoundedRectangle(XPen pen, XRectangleF rect, float radius)
        {
            var radiusPx = radius;
            using (var path = this.GetRoundedRectPath(rect.ToRectangleF(), radiusPx))
            {
                using (var thePen = pen.ToPen())
                    this.Graphics.DrawPath(thePen, path);
            }
        }
    }
}