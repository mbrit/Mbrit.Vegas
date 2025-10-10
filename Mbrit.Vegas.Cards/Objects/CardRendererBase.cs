using Mbrit.Vegas.Cards.Objects;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using Mbrit.Vegas.Utility;

namespace Mbrit.Vegas.Cards
{
    internal abstract class CardRendererBase : ICardRenderer
    {
        protected float Scale { get; }

        internal const float InchesToMm = 25.4f;
        protected const int MaxHands = 25;

        protected CardRendererBase(float scale)
        {
            this.Scale = scale;
        }

        public void Render(IGraphics g)
        {
            using (var style = new CardStyle(this.Scale))
            {
                //var widthMm = 3.5f * InchesToMm;
                //var heightMm = 2.5f * InchesToMm;

                //var width = this.ScaleF(widthMm);
                //var height = this.ScaleF(heightMm);

                // outer...
                //var rect = new XRectangleF(0, 0, width, height);
                var rect = g.ClientRect;

                g.FillRectangle(XBrushes.White, rect);
                //g.DrawRoundedRectangle(XPens.Black, rect, this.ScaleF((decimal)((1f / 8f) * 25.4f)));

                this.DoRender(rect, style, g);
            }
        }

        protected abstract void DoRender(XRectangleF rect, CardStyle style, IGraphics g);

        protected float ScaleF(float value) => ((float)value * 3.2f) * this.Scale;

        protected static string GetSuitSymbol(Suit suit)
        {
            if (suit == Suit.Diamond)
                return "♦";
            else if (suit == Suit.Heart)
                return "♥";
            else if (suit == Suit.Spade)
                return "♠";
            else if (suit == Suit.Club)
                return "♣";
            else
                throw new NotImplementedException("This operation has not been implemented.");
        }

        protected void RenderLabel(XRectangleF rect, string label, CardStyle style, IGraphics g)
        {
            g.FillRectangle(style.Row1Brush, rect);
            g.DrawString(label, style.BodyFont, style.DarkLabelBrush, rect, new XStringFormat()
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XStringAlignment.Center
            });
        }
    }
}