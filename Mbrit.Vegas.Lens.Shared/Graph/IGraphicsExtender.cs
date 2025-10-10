using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens
{
    public static class IGraphicsExtender
    {
        public static void MarkRectangle(this IGraphics g, XRectangleF rect)
        {
            using (var pen = XPens.Magenta)
            {
                g.DrawRectangle(pen, rect);
                g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Bottom);
                g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Top);
            }
        }

        public static void DrawLine(this IGraphics g, XPen pen, XPointF start, XPointF end) =>
            g.DrawLine(pen, start.X, start.Y, end.X, end.Y);

        public static void DrawArrow(this IGraphics g, XColor color, XPointF start, XPointF end)
        {
            using (var pen = new XPen(color, 3))
            {
                using (var brush = new XBrush(color))
                {
                    g.DrawLine(pen, start, end);

                    const float size = 6f;

                    var poly = new List<XPointF>();
                    poly.Add(end);

                    if (end.Y > start.Y)
                    {
                        poly.Add(new XPointF(end.X - size, end.Y - size));
                        poly.Add(new XPointF(end.X + size, end.Y - size));
                    }
                    else
                    {
                        poly.Add(new XPointF(end.X - size, end.Y + size));
                        poly.Add(new XPointF(end.X + size, end.Y + size));
                    }

                    g.FillPolygon(brush, poly.ToArray());
                }
            }
        }

        public static void DrawPoint(this IGraphics g, XPen pen, float x, float y, float size = 5)
        {
            g.DrawLine(pen, x - size, y - size, x + size, y + size);
            g.DrawLine(pen, x - size, y + size, x + size, y - size);
        }

        public static void DrawString(this IGraphics g, string label, XFont font, XBrush brush, XRectangleF rect) =>
            g.DrawString(label, font, brush, rect, new XStringFormat());

        public static void DrawString(this IGraphics g, string label, XFont font, XBrush brush, float x, float y) =>
            g.DrawString(label, font, brush, x, y, new XStringFormat());

        public static void DrawCross(this IGraphics g, XPen pen, float x, float y, float size = 5)
        {
            g.DrawLine(pen, x - size, y - size, x + size, y + size);
            g.DrawLine(pen, x - size, y + size, x + size, y - size);
        }

        public static void DrawBox(this IGraphics g, XPen pen, XRectangleF rect)
        {
            g.DrawRectangle(pen, rect);
            g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Bottom);
            g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Top);
        }
    }
}
