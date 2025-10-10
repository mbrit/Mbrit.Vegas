using Mbrit.Vegas.Cards.Objects;
using Mbrit.Vegas.Lens;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;

namespace Mbrit.Vegas.Cards
{
    internal class NotepadRenderer : CardRendererBase
    {
        internal NotepadRenderer(float scale)
            : base(scale) 
        {

        }

        protected override void DoRender(XRectangleF rect, CardStyle style, IGraphics g)
        {
            var xMargin = this.ScaleF(6);
            var yMargin = this.ScaleF(12);

            var inner = new XRectangleF(rect.Left + xMargin, rect.Top + yMargin, rect.Width - (2 * xMargin), rect.Bottom - (2 * yMargin));
            g.DrawRectangle(style.GridPen, inner);

            const int hands = 25;
            var per = inner.Height / (hands + 1);

            var rows = new List<XRectangleF>();
            var y = inner.Top;
            for (var index = 0; index < hands + 1; index++)
            {
                var row = new XRectangleF(inner.Left, y, inner.Width, per);
                rows.Add(row);



                y += per;
            }
        }
    }
}