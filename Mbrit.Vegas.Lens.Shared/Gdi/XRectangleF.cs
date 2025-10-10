using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public struct XRectangleF
    {
        public float Left { get; }
        public float Top { get; }
        public float Width { get; }
        public float Height { get; }

        public XRectangleF(float left, float top, float width, float height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
        }

        public float X => this.Left;

        public float Y => this.Top;

        public float Right => this.Left + this.Width;

        public float Bottom => this.Top + this.Height;

        public float MidWidth => this.Left + (this.Width / 2);

        public float MidHeight => this.Top + (this.Height / 2);

        public XPointF TopLeft => new XPointF(this.Left, this.Top);

        public XRectangleF Inflate(float adjust) => this.Inflate(adjust, adjust);

        public XRectangleF Inflate(float xAdjust, float yAdjust) => new XRectangleF(this.Left + xAdjust, this.Top + yAdjust,
            this.Width - (2 * xAdjust), this.Height - (2 * yAdjust));

        public bool Contains(XPointF point) => this.Left <= point.X && this.Right >= point.X && 
            this.Top <= point.Y && this.Bottom >= point.Y;

        public XRectangle ToXRectangle() => new XRectangle((int)this.Left, (int)this.Top, (int)this.Width, (int)this.Height);
    }
}
