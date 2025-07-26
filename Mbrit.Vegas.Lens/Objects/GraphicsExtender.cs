using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens
{
    internal static class GraphicsExtender
    {
        internal static void DrawX(this Graphics g, float x, float y, float size = 2.5f)
        {
            var pen = Pens.Black;
            g.DrawLine(pen, x - size, y - size, x + size, y + size);
            g.DrawLine(pen, x - size, y + size, x + size, y - size);
        }
    }
}
