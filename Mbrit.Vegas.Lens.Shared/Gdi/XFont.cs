using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Lens.Gdi
{
    public record XFontFamily(string Name)
    {
    }

    public class XFont : IDisposable
    {
        public XFontFamily FontFamily { get; }
        public float Size { get; }
        public bool Bold { get; set; }

        public XFont(string fontName, float size)
        {
            this.FontFamily = new XFontFamily(fontName);
            this.Size = size;
        }

        public void Dispose()
        {
            // no-op...
        }
    }
}
