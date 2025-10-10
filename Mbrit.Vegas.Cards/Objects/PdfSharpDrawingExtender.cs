using BootFX.Common;
using Mbrit.Vegas.Lens.Gdi;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSD = PdfSharpCore.Drawing;
using PSP = PdfSharpCore.Pdf;

namespace Mbrit.Vegas.Cards
{
    internal static class PdfSharpDrawingExtender
    {
        internal static PSD.XPen ToPdfXPen(this XPen pen)
        {
            var thePen = new PSD.XPen(pen.Color.ToPdfXColor(), pen.Width);

            if (pen.DashStyle == XDashStyle.Solid)
            {
                // no-op...
            }
            else if (pen.DashStyle == XDashStyle.Dash)
                thePen.DashStyle = PSD.XDashStyle.Dash;
            else if (pen.DashStyle == XDashStyle.Dot)
                thePen.DashStyle = PSD.XDashStyle.Dot;
            else
                throw new NotSupportedException($"Cannot handle '{pen.DashStyle}'.");

            return thePen;
        }

        internal static PSD.XBrush ToPdfXBrush(this XBrush brush) => new PSD.XSolidBrush(brush.Color.ToPdfXColor());

        internal static PSD.XFont ToPdfXFont(this XFont font)
        {
            var style = PSD.XFontStyle.Regular;
            if (font.Bold)
                style |= PSD.XFontStyle.Bold;

            return new PSD.XFont(font.FontFamily.Name, font.Size * 1.3f, style);
        }

        internal static PSD.XRect ToPdfXRect(this XRectangleF rect) => new PSD.XRect(rect.X, rect.Y, rect.Width, rect.Height);

        internal static PSP.PdfRectangle ToPdfRectangle(this XRectangleF rect) => new PSP.PdfRectangle(new PSD.XPoint(rect.X, rect.Y), new PSD.XSize(rect.Width, rect.Height));

        internal static PSD.XStringFormat ToPdfXStringFormat(this XStringFormat format)
        {
            var newFormat = new PSD.XStringFormat();

            if (format != null)
            {
                if (format.Alignment == XStringAlignment.Center)
                    newFormat.Alignment = PSD.XStringAlignment.Center;
                else if (format.Alignment == XStringAlignment.Near)
                    newFormat.Alignment = PSD.XStringAlignment.Near;
                else if (format.Alignment == XStringAlignment.Far)
                    newFormat.Alignment = PSD.XStringAlignment.Far;
                else
                    throw new NotSupportedException($"Cannot handle '{newFormat}'.");

                if (format.LineAlignment == XStringAlignment.Center)
                    newFormat.LineAlignment = PSD.XLineAlignment.Center;
                else if (format.LineAlignment == XStringAlignment.Near)
                    newFormat.LineAlignment = PSD.XLineAlignment.Near;
                else if (format.LineAlignment == XStringAlignment.Far)
                    newFormat.LineAlignment = PSD.XLineAlignment.Far;
                else
                    throw new NotSupportedException($"Cannot handle '{newFormat}'.");
            }

            return newFormat;
        }

        internal static PSD.XColor ToPdfXColor(this XColor color) => PSD.XColor.FromArgb(color.R, color.G, color.B);

        internal static PSD.XImage ToPdfXImage(this Image image)
        {
            return PSD.XImage.FromStream(() =>
            {
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            });
        }
    }
}
