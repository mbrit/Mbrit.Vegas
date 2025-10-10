using Mbrit.Vegas.Lens.Gdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Graph
{
    public interface IGraphics
    {
        XRectangleF ClientRect { get; }

        void DrawLine(XPen pen, float x1, float y1, float x2, float y2);
        void DrawRectangle(XPen pen, XRectangleF rect);
        void DrawString(string label, XFont font, XBrush brush, float x, float y, XStringFormat format);
        void DrawString(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format);
        void DrawStringTight(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format);
        void FillEllipse(XBrush brush, float v1, float v2, int size1, int size2);
        void FillPolygon(XBrush brush, XPointF[] pointFs);
        void FillRectangle(XBrush brush, XRectangleF rect);

        XSizeF MeasureString(string label, XFont font);

        IDisposable RotateAround(float x, float y, float angle);
    }
}
