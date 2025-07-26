using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Graph
{
    internal interface IGraphics
    {
        void DrawLine(Pen pen, float lastProfitX, float lastProfitY, float profitX, float profitY);
        void DrawRectangle(Pen pen, RectangleF rect);
        void DrawString(string label, Font font, Brush brush, float x, float v, StringFormat format = null);
        void FillEllipse(Brush brush, float v1, float v2, int size1, int size2);
        void FillPolygon(Brush aquamarine, PointF[] pointFs);
        void DrawArrow(Color color, PointF start, PointF end);
    }
}
