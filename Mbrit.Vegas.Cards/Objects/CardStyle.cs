using Mbrit.Vegas.Lens.Gdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards.Objects
{
    internal class CardStyle : IDisposable
    {
        internal XFont BodyFont { get; }
        internal XFont HandFont { get; }
        internal XFont RankFont { get; }
        internal XFont SuitFont { get; }

        internal XBrush BodyBrush { get; }
        internal XBrush LabelBrush { get; }
        internal XBrush DarkLabelBrush { get; }
        internal XPen GridPen { get; }
        internal XPen LightGridPen { get; }

        internal XBrush RedRankBrush { get; }
        internal XBrush BlackRankBrush { get; }

        internal XBrush Row1Brush { get; }
        internal XBrush Row2Brush { get; }

        internal CardStyle(float scale)
        {
            const string fontName = "Bahnschrift";

            this.BodyFont = new XFont(fontName, 10 * scale);
            this.HandFont = new XFont(fontName, 6 * scale);
            this.RankFont = new XFont("Oswald", 17 * scale)
            {
                Bold = true
            };
            this.SuitFont = new XFont("Noto Sans Symbols 2", 19 * scale)
            {
                Bold = true
            };

            this.BodyBrush = new XBrush(XColor.Black);
            this.LabelBrush = new XBrush(XColor.FromArgb(0xc0, 0xc4, 0xc8));
            this.DarkLabelBrush = new XBrush(XColor.FromArgb(0x90, 0xa0, 0xb0));
            this.GridPen = new XPen(XColor.FromArgb(0x60, 0x70, 0x80));
            this.LightGridPen = new XPen(XColor.FromArgb(0x80, 0x90, 0xa0))
            {
                DashStyle = XDashStyle.Dot
            };

            this.RedRankBrush = new XBrush(XColor.FromArgb(200, 16, 46));
            this.BlackRankBrush = new XBrush(XColor.Black);

            this.Row1Brush = new XBrush(XColor.FromArgb(0xd0, 0xd4, 0xd8));
            this.Row2Brush = new XBrush(XColor.White);
        }

        public void Dispose()
        {
            this.BodyFont.Dispose();
            this.HandFont.Dispose();
            this.RankFont.Dispose();
            this.SuitFont.Dispose();

            this.BodyBrush.Dispose();
            this.LabelBrush.Dispose();
            this.DarkLabelBrush.Dispose();
            this.GridPen.Dispose();
            this.LightGridPen.Dispose();

            this.RedRankBrush.Dispose();
            this.BlackRankBrush.Dispose();

            this.Row1Brush.Dispose();
            this.Row2Brush.Dispose();
        }
    }
}
