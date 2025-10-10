using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XPens
    {
        public static XPen Black => new XPen(XColor.Black, 1);

        public static XPen White => new XPen(XColor.White, 1);

        public static XPen Magenta => new XPen(XColor.Magenta, 1);

        public static XPen Blue => new XPen(XColor.Blue, 1);

        public static XPen Green => new XPen(XColor.Green, 1);

        public static XPen Red => new XPen(XColor.Red, 1);

        public static XPen Gray => new XPen(XColor.Gray, 1);
    }
}
