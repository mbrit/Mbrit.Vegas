using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public struct XPointF
    {
        public float X { get; }
        public float Y { get; }

        public static XPointF Empty { get; } = new XPointF(0, 0);

        public XPointF(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public XPointF Move(float x, float y) => new XPointF(this.X + x, this.Y + y);   
    }
}
