using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XBrush : IDisposable
    {
        public XColor Color { get; }

        public XBrush(XColor color)
        {
            this.Color = color;
        }

        public void Dispose()
        {
            // no-op...
        }
    }
}
