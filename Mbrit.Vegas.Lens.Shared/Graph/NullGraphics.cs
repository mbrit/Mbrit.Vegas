using BootFX.Common;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens
{
    internal class NullGraphics : IGraphics
    {
        public XRectangleF ClientRect => new XRectangleF(0, 0, 0, 0);

        public void DrawArrow(XColor color, XPointF start, XPointF end)
        {
        }

        public void DrawLine(XPen pen, float lastProfitX, float lastProfitY, float profitX, float profitY)
        {
        }

        public void DrawLine(object black, float v1, float y1, float v2, float y2)
        {
        }

        public void DrawPoint(XPen black, float x, float y)
        {
        }

        public void DrawRectangle(XPen pen, XRectangleF rect)
        {
        }

        public void FillRectangle(XBrush brush, XRectangleF rect)
        {
        }

        public void DrawString(string label, XFont font, XBrush brush, float x, float v, XStringFormat format = null)
        {
        }

        public void FillEllipse(XBrush brush, float v1, float v2, int size1, int size2)
        {
        }

        public void FillPolygon(XBrush aquamarine, XPointF[] pointFs)
        {
        }

        public XSizeF MeasureString(string buf, XFont font) => new XSizeF(0, 0);

        public IDisposable RotateAround(float x, float y, float angle) => new NullDisposer();

        public void DrawStringTight(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
        }

        public void DrawString(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
        }
    }
}
