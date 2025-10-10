using Mbrit.Vegas.Lens.Gdi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Graph
{
    public static class GraphicsExtender
    {
        public static void DrawLine(this Graphics g, XPen pen, float x1, float y1, float x2, float y2)
        {
            using (var thePen = pen.ToPen())
                g.DrawLine(thePen, x1, y1, x2, y2);
        }

        public static void DrawRectangle(this Graphics g, XPen pen, XRectangleF rect)
        {
            using (var thePen = pen.ToPen())
                g.DrawRectangle(thePen, rect.ToRectangleF());
        }

        public static void FillRectangle(this Graphics g, XBrush brush, XRectangleF rect)
        {
            using (var theBrush = brush.ToBrush())
                g.FillRectangle(theBrush, rect.ToRectangleF());
        }

        public static void DrawString(this Graphics g, string label, XFont font, XBrush brush, float x, float y, XStringFormat format = null)
        {
            using (var theFont = font.ToFont())
            {
                using (var theBrush = brush.ToBrush())
                {
                    g.DrawString(label, theFont, theBrush, x, y, format.ToStringFormat());
                }
            }
        }

        public static void DrawString(this Graphics g, string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format = null)
        {
            using (var theFont = font.ToFont())
            {
                using (var theBrush = brush.ToBrush())
                {
                    g.DrawString(label, theFont, theBrush, rect.ToRectangleF(), format.ToStringFormat());
                }
            }
        }

        public static void FillEllipse(this Graphics g, XBrush brush, float x, float y, int widgth, int height)
        {
            using(var theBrush = brush.ToBrush())
                g.FillEllipse(theBrush, x, y, widgth, height);
        }

        public static void FillPolygon(this Graphics g, XBrush brush, XPointF[] points)
        {
            using (var theBrush = brush.ToBrush())
                g.FillPolygon(theBrush, points.Select(v => v.ToPointF()).ToArray());
        }

        private static Color ToColor(this XColor color) => Color.FromArgb(color.A, color.R, color.G, color.B);

        internal static Pen ToPen(this XPen pen)
        {
            var thePen = new Pen(pen.Color.ToColor(), pen.Width);

            if (pen.DashStyle == XDashStyle.Solid)
            {
                // no-op...
            }
            else if (pen.DashStyle == XDashStyle.Dash)
                thePen.DashStyle = DashStyle.Dash;
            else if (pen.DashStyle == XDashStyle.Dot)
                thePen.DashStyle = DashStyle.Dot;
            else
                throw new NotSupportedException($"Cannot handle '{pen.DashStyle}'.");

            return thePen;
        }

        internal static Brush ToBrush(this XBrush brush) => new SolidBrush(brush.Color.ToColor());

        internal static Font ToFont(this XFont font)
        {
            var style = FontStyle.Regular;
            if (font.Bold)
                style |= FontStyle.Bold;

            var theFont = new Font(font.FontFamily.Name, font.Size, style);
            return theFont;
        }

        private static PointF ToPointF(this XPointF point) => new PointF(point.X, point.Y);

        internal static RectangleF ToRectangleF(this XRectangleF rect) => new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);

        internal static Rectangle ToRectangle(this XRectangleF rect) => new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        private static StringFormat ToStringFormat(this XStringFormat format)
        {
            var theFormat = new StringFormat();

            if (format != null)
            {
                if (format.Alignment == XStringAlignment.Near)
                    theFormat.Alignment = StringAlignment.Near;
                else if (format.Alignment == XStringAlignment.Center)
                    theFormat.Alignment = StringAlignment.Center;
                else if (format.Alignment == XStringAlignment.Far)
                    theFormat.Alignment = StringAlignment.Far;
                else
                    throw new NotSupportedException($"Cannot handle '{format.Alignment}'.");

                if (format.LineAlignment == XStringAlignment.Center)
                    theFormat.LineAlignment = StringAlignment.Center;
                else if (format.LineAlignment == XStringAlignment.Near)
                    theFormat.LineAlignment = StringAlignment.Near;
                else if (format.LineAlignment == XStringAlignment.Far)
                    theFormat.LineAlignment = StringAlignment.Far;
                else
                    throw new NotSupportedException($"Cannot handle '{theFormat}'.");
            }

            return theFormat;
        }
    }
}
