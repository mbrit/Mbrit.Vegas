using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public struct XSizeF
    {
        public float Width { get; }
        public float Height { get; }

        public XSizeF(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
