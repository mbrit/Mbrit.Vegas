using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using PSD = PdfSharpCore.Drawing;
using PSP = PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mbrit.Vegas.Lens;
using PdfSharpCore.Pdf;
using System.Runtime.CompilerServices;

namespace Mbrit.Vegas.Cards
{
    public sealed class PdfGraphics : IGraphics, IDisposable
    {
        private PSP.PdfDocument Document { get; set; }
        private PSP.PdfPage Page { get; set; }
        private PSD.XGraphics Graphics { get; set; }

        public XRectangleF ClientRect { get; }
        public XRectangleF TrimRect { get; }
        public XRectangleF BleedRect { get; }

        private float PageWidthMm { get; }
        private float PageHeightMm { get; }
        private float TrimsMm { get; }

        private bool IsDisposed { get; set; }

        //private const float MmToPointsRatio = 72f / 25.4f;

        private PdfGraphics(PSP.PdfDocument doc, float pageWidthMm, float pageHeightMm, float trimsMm)
        {
            this.Document = doc;

            this.PageWidthMm = pageWidthMm + (2 * trimsMm);
            this.PageHeightMm = pageHeightMm + (2 * trimsMm);

            this.ClientRect = new XRectangleF(MmToPoints(trimsMm * 2), MmToPoints(trimsMm * 2), MmToPoints(pageWidthMm), MmToPoints(pageHeightMm));

            this.TrimRect = this.ClientRect;

            this.BleedRect = new XRectangleF(MmToPoints(trimsMm * 1), MmToPoints(trimsMm * 1), 
                MmToPoints(pageWidthMm + (2 * trimsMm)), MmToPoints(pageHeightMm + (2 * trimsMm)));

            this.TrimsMm = trimsMm;
        }

        // Convenience factory to open a page sized in millimetres.
        public static PdfGraphics Create(float pageWidthMm, float pageHeightMm, float trimsMm, out PSP.PdfDocument doc)
        {
            doc = new PSP.PdfDocument();
            return new PdfGraphics(doc, pageWidthMm, pageHeightMm, trimsMm);
        }

        private static PdfPage CreatePage(float pageWidthMm, float pageHeightMm, float trimsMm, PdfDocument doc)
        {
            var page = doc.AddPage();
            page.Width = PSD.XUnit.FromMillimeter(pageWidthMm + (trimsMm * 2));
            page.Height = PSD.XUnit.FromMillimeter(pageHeightMm + (trimsMm * 2));
            return page;
        }

        private static float MmToPoints(float mm) => (float)PSD.XUnit.FromMillimeter((float)mm);

        public void Dispose()
        {
            if (IsDisposed) 
                return;

            Graphics.Dispose();
            IsDisposed = true;
        }

        // ================= IGraphics =================

        public void DrawLine(XPen pen, float x1, float y1, float x2, float y2)
            => Graphics.DrawLine(pen.ToPdfXPen(), x1, y1, x2, y2);

        public void DrawRectangle(XPen pen, XRectangleF rect)
            => Graphics.DrawRectangle(pen.ToPdfXPen(), new PSD.XRect(rect.X, rect.Y, rect.Width, rect.Height));

        public void FillRectangle(XBrush brush, XRectangleF rect)
            => Graphics.DrawRectangle(brush.ToPdfXBrush(), new PSD.XRect(rect.X, rect.Y, rect.Width, rect.Height));

        public void FillEllipse(XBrush brush, float x, float y, int width, int height)
            => Graphics.DrawEllipse(brush.ToPdfXBrush(), x, y, width, height);

        public void FillPolygon(XBrush brush, XPointF[] pointFs)
        {
            var pts = pointFs?.Select(p => new PSD.XPoint(p.X, p.Y)).ToArray() ?? Array.Empty<PSD.XPoint>();
            // Fill polygon; Winding is typical for UI shapes
            Graphics.DrawPolygon(brush.ToPdfXBrush(), pts, PSD.XFillMode.Winding);
        }

        public void DrawString(string label, XFont font, XBrush brush, float x, float y, XStringFormat format)
        {
            // PDFsharp expects an XPoint plus XStringFormat
            Graphics.DrawString(label, font.ToPdfXFont(), brush.ToPdfXBrush(), new PSD.XPoint(x, y), format.ToPdfXStringFormat());
        }

        public void DrawString(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
            var r = new PSD.XRect(rect.X, rect.Y, rect.Width, rect.Height);
            Graphics.DrawString(label, font.ToPdfXFont(), brush.ToPdfXBrush(), rect.ToPdfXRect(), format.ToPdfXStringFormat());
        }

        // "Tight" text rendering hint – draws from TopLeft and doesn't add extra padding.
        // If you want true tight metrics, you can pre-measure and align manually.
        public void DrawStringTight(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
            var theFormat = format.ToPdfXStringFormat();
            Graphics.DrawString(label, font.ToPdfXFont(), brush.ToPdfXBrush(), rect.ToPdfXRect(), theFormat);
        }

        public IDisposable RotateAround(float x, float y, float angleDegrees)
        {
            var state = Graphics.Save();
            Graphics.RotateAtTransform(angleDegrees, new PSD.XPoint(x, y));
            return new RestoreOnDispose(Graphics, state);
        }

        private sealed class RestoreOnDispose : IDisposable
        {
            private readonly PSD.XGraphics _gfx;
            private readonly PSD.XGraphicsState _state;
            private bool _done;
            public RestoreOnDispose(PSD.XGraphics gfx, PSD.XGraphicsState state) { _gfx = gfx; _state = state; }
            public void Dispose() { if (_done) return; _gfx.Restore(_state); _done = true; }
        }

        public XSizeF MeasureString(string buf, XFont font)
        {
            var theFont = font.ToPdfXFont();
            var size = this.Graphics.MeasureString(buf, theFont);
            return new XSizeF((float)size.Width, (float)size.Height);
        }

        internal void AddTrimMarksToPage()
        {
            //var trimBox = this.ClientRect;
            //this.DrawBox(XPens.Green, this.TrimRect);
            //this.DrawBox(XPens.Blue, this.BleedRect);

            var pen = XPens.Black.ToPdfXPen();

            this.Graphics.DrawLine(pen, 0, this.TrimRect.Top, this.BleedRect.Left, this.TrimRect.Top);
            this.Graphics.DrawLine(pen, this.BleedRect.Right, this.TrimRect.Top, this.Page.Width, this.TrimRect.Top);

            this.Graphics.DrawLine(pen, 0, this.TrimRect.Bottom, this.BleedRect.Left, this.TrimRect.Bottom);
            this.Graphics.DrawLine(pen, this.BleedRect.Right, this.TrimRect.Bottom, this.Page.Width, this.TrimRect.Bottom);

            this.Graphics.DrawLine(pen, this.TrimRect.Left, 0, this.TrimRect.Left, this.BleedRect.Top);
            this.Graphics.DrawLine(pen, this.TrimRect.Left, this.BleedRect.Bottom, this.TrimRect.Left, this.Page.Height);

            this.Graphics.DrawLine(pen, this.TrimRect.Right, 0, this.TrimRect.Right, this.BleedRect.Top);
            this.Graphics.DrawLine(pen, this.TrimRect.Right, this.BleedRect.Bottom, this.TrimRect.Right, this.Page.Height);

            this.Page.TrimBox = this.TrimRect.ToPdfRectangle();
            this.Page.BleedBox = this.BleedRect.ToPdfRectangle();
            //this.Page.CropBox = this.Page.BleedBox;
        }

        internal PdfPage AddPage()
        {
            this.Page = CreatePage(this.PageWidthMm, this.PageHeightMm, this.TrimsMm, this.Document);

            if (this.Graphics != null)
                this.Graphics.Dispose();

            this.Graphics = PSD.XGraphics.FromPdfPage(this.Page);

            return this.Page;
        }

        internal void DrawImageInBox(PSD.XImage image, XRectangleF bleedRect)
        {
            XRectangleF targetRect;

            if(image.PointWidth > image.PointHeight)
            {
                var ratio = image.PointWidth / image.PointHeight;
                var width = (float)(this.BleedRect.Height * ratio);
                targetRect = new XRectangleF(this.BleedRect.MidWidth - (width / 2f), this.BleedRect.MidHeight - (this.BleedRect.Height / 2),
                    width, this.BleedRect.Height);
            }
            else
                throw new NotImplementedException("This operation has not been implemented.");

            this.Graphics.DrawImage(image, targetRect.ToPdfXRect());
        }

        internal void DrawImageInBox2(PSD.XImage image, XRectangleF bleedRect)
        {
            XRectangleF targetRect;

            if (image.PointWidth > image.PointHeight)
            {
                var ratio = (float)(image.PointWidth / image.PointHeight);
                var width = this.ClientRect.Width;
                var height = width / ratio;
                targetRect = new XRectangleF(this.BleedRect.MidWidth - (width / 2f), this.BleedRect.MidHeight - (height / 2),
                    width, height);
            }
            else
                throw new NotImplementedException("This operation has not been implemented.");

            this.Graphics.DrawImage(image, targetRect.ToPdfXRect());
        }
    }
}