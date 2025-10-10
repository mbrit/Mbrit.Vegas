using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XPen : IDisposable
    {
        public XColor Color { get; }
        public int Width { get; }
        public XDashStyle DashStyle { get; set; }

        public XPen(XColor color, int width = 1)
        {
            this.Color = color;
            this.Width = width;
        }

        public void Dispose()
        {
            // no-op...
        }
    }
}
