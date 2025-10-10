using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public struct XRectangle
    {
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }

        public XRectangle(int left, int top, int width, int height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
        }

        public int X => this.Left;

        public int Y => this.Top;

        public int Right => this.Left + this.Width;

        public int Bottom => this.Top + this.Height;

        public int MidWidth => this.Left + (this.Width / 2);

        public int MidHeight => this.Top + (this.Height / 2);

        public XPoint TopLeft => new XPoint(this.Left, this.Top);

        public XRectangle Inflate(int adjust) => new XRectangle(this.Left + adjust, this.Top + adjust,
            this.Width - (2 * adjust), this.Height - (2 * adjust));

        public bool Contains(XPoint point) => this.Left <= point.X && this.Right >= point.X &&
            this.Top <= point.Y && this.Bottom >= point.Y;
    }
}
