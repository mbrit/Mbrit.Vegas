using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XPoint
    {
        public int X { get; }
        public int Y { get; }

        public static XPointF Empty { get; } = new XPointF(0, 0);

        public XPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public XPoint Move(int x, int y) => new XPoint(this.X + x, this.Y + y);

        public XPointF ToPointF() => new XPointF(this.X, this.Y);
    }
}
