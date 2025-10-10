using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XColor
    {
        public int A { get; }
        public int R { get; }
        public int G { get; }
        public int B { get; }

        private XColor(int a, int r, int g, int b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public static XColor FromArgb(int r, int g, int b) => FromArgb(255, r, g, b);

        public static XColor FromArgb(int a, int r, int g, int b) => new XColor(a, r, g, b);

        public static XColor Black => FromArgb(0, 0, 0);

        public static XColor White => FromArgb(255, 255, 255);

        public static XColor Gray => FromArgb(0x80, 0x80, 0x80);

        public static XColor Red => FromArgb(192, 0, 0);

        public static XColor Green => FromArgb(0, 192, 0);

        public static XColor Blue => FromArgb(0, 0, 192);

        public static XColor Magenta => FromArgb(255, 0, 255);

        public static XColor Orange => FromArgb(255, 153, 99);
    }
}
