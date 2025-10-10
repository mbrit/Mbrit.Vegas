using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public class XBrushes
    {
        public static XBrush Black => new XBrush(XColor.Black);

        public static XBrush White => new XBrush(XColor.White);

        public static XBrush Gray => new XBrush(XColor.Gray);

        public static XBrush Green => new XBrush(XColor.Green);

        public static XBrush Blue => new XBrush(XColor.Blue);

        public static XBrush Red => new XBrush(XColor.Red);
    }
}
