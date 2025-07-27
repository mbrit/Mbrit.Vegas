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
        public void DrawArrow(Color color, PointF start, PointF end)
        {
        }

        public void DrawLine(Pen pen, float lastProfitX, float lastProfitY, float profitX, float profitY)
        {
        }

        public void DrawRectangle(Pen pen, RectangleF rect)
        {
        }

        public void DrawString(string label, Font font, Brush brush, float x, float v, StringFormat format)
        {
        }

        public void FillEllipse(Brush brush, float v1, float v2, int size1, int size2)
        {
        }

        public void FillPolygon(Brush aquamarine, PointF[] pointFs)
        {
        }

        public void DrawPoint(Pen black, float x, float y)
        {
        }
    }
}
