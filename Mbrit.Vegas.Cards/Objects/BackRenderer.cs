using Mbrit.Vegas.Cards.Objects;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using Mbrit.Vegas.Simulator;
using System.Configuration;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks.Sources;
using System.Windows.Forms.VisualStyles;

namespace Mbrit.Vegas.Cards
{
    internal class BackRenderer : CardRendererBase
    {
        internal BackRenderer(float scale)
            : base(scale)
        {
        }

        protected override void DoRender(XRectangleF outer, CardStyle style, IGraphics g)
        {
            var edge = this.ScaleF(2.25f);
            var rect = outer.Inflate(edge);

            const int numBands = 5;
            var bandHeight = rect.Height / numBands;

            var y = rect.Top;

            var bands = new List<XRectangleF>();
            for(var index = 0; index < numBands; index++)
            {
                bands.Add(new XRectangleF(rect.Left, y, rect.Width, bandHeight));
                y += bandHeight;
            }

            var barWidth = bandHeight / 6;

            var pattern = new XRectangleF(rect.Left, bands[0].Bottom, rect.Width, bands[4].Top - bands[0].Bottom);

            // top band, bottom band...
            this.DrawThickBox(XBrushes.Black, new XRectangleF(rect.Left, rect.Top, rect.Width, bandHeight), barWidth, g);
            this.DrawThickBox(XBrushes.Black, new XRectangleF(rect.Left, rect.Bottom - bandHeight, rect.Width, bandHeight), barWidth, g);

            // diamond...
            var diamondSize = (bandHeight * (numBands - 2)) + (2 * barWidth);
            var diamond = new XRectangleF(rect.MidWidth - (diamondSize / 2), rect.MidHeight - (diamondSize / 2), diamondSize, diamondSize);

            // lines from diamond...
            var toLeft = diamond.Left - rect.Left;
            for (var index = 0; index < 5; index++)
            {
                this.DrawThickLine(XBrushes.Black, diamond.Left, diamond.MidHeight, diamond.Left - toLeft, diamond.MidHeight + toLeft, barWidth, g,
                    0 - (barWidth * (index * 2)), 0 - (barWidth * (index * 2)));
            }
            for (var index = 0; index < 2; index++)
            {
                this.DrawThickLine(XBrushes.Black, rect.Left, diamond.MidHeight - (diamond.Left - rect.Left), diamond.Left, diamond.MidHeight, barWidth, g,
                    (barWidth * (index * 2)), 0 - barWidth * (index * 2));
            }

            var offset = barWidth * 2;
            for (var index = 0; index < 2; index++)
            {
                this.DrawThickLine(XBrushes.Black, diamond.Left - (pattern.Top - (diamond.MidHeight - (2 * offset))), pattern.Top,
                    diamond.Left, diamond.MidHeight - (2 * offset), barWidth, g,
                    0 - (barWidth * (index * 2)), 0 - barWidth * (index * 2));
            }

            // left-side verticals...
            for (var index = -3; index < 4; index++)
            {
                var vIndex = index;
                if (vIndex < 0)
                    vIndex = 0 - vIndex;

                this.DrawThickLine(XBrushes.Black, diamond.Right, pattern.Top, diamond.Right, diamond.MidHeight - (barWidth * (vIndex * 2)), barWidth, g,
                    0 - (barWidth * (index * 2)), 0);
            }

            // right side...
            var toRight = rect.Right - diamond.Right;
            for (var index = 0; index < 5; index++)
            {
                this.DrawThickLine(XBrushes.Black, diamond.Right, diamond.MidHeight, diamond.Right + toRight, diamond.MidHeight - toRight, barWidth, g, 
                    barWidth * (index * 2), barWidth * (index * 2));
            }
            for (var index = 0; index < 2; index++)
            {
                this.DrawThickLine(XBrushes.Black, diamond.Right, diamond.MidHeight, rect.Right, diamond.MidHeight + (rect.Right - diamond.Right), barWidth, g,
                    0 - (barWidth * (index * 2)), barWidth * (index * 2));
            }

            offset = barWidth * 2;
            for(var index = 0; index < 2; index++)
            {
                this.DrawThickLine(XBrushes.Black, diamond.Right, diamond.MidHeight + (2 * offset), 
                    diamond.Right - (pattern.Bottom - (diamond.MidHeight + (2 * offset))), pattern.Bottom, barWidth, g,
                    (barWidth * (index * 2)), barWidth * (index * 2));
            }

            // right-side verticals...
            for (var index = -3; index < 4; index++)
            {
                var vIndex = index;
                if (vIndex < 0)
                    vIndex = 0 - vIndex;

                this.DrawThickLine(XBrushes.Black, diamond.Left, diamond.MidHeight, diamond.Left, pattern.Bottom - (barWidth * (vIndex * 2)), barWidth, g, 
                    (barWidth * (index * 2)), (barWidth * (vIndex * 2)));
            }

            // outer diamond...
            this.DrawDiamond(XBrushes.Black, diamond, barWidth, g);

            // inner diamond...
            var innerDiamondSize = diamondSize - (barWidth * 8);
            var innerDiamond = new XRectangleF(rect.MidWidth - (innerDiamondSize / 2), rect.MidHeight - (innerDiamondSize / 2), innerDiamondSize, innerDiamondSize);
            this.DrawDiamond(XBrushes.Black, innerDiamond, barWidth, g);

            // bottom chevrons...
            var band = bands.Last();
            this.DrawThickLine(XBrushes.Black, band.MidWidth + (barWidth / 2), band.Top + (barWidth * 2.5f), 
                band.MidWidth + (barWidth / 2) - (band.Bottom - (band.Top + (barWidth * 2.5f))), band.Bottom, barWidth, g);
            this.DrawThickLine(XBrushes.Black, band.MidWidth - (barWidth / 2), band.Top + (barWidth * 2.5f),
                band.MidWidth - (barWidth / 2) + (band.Bottom - (band.Top + (barWidth * 2.5f))), band.Bottom, barWidth, g);

            // top chevrons...
            band = bands.First();
            this.DrawThickLine(XBrushes.Black, band.MidWidth + (barWidth / 2), band.Top + (barWidth * 2.5f), 
                band.MidWidth + (barWidth / 2) - (band.Bottom - (band.Top + (barWidth * 2.5f))), band.Bottom, barWidth, g);
            this.DrawThickLine(XBrushes.Black, band.MidWidth - (barWidth / 2), band.Top + (barWidth * 2.5f),
                band.MidWidth - (barWidth / 2) + (band.Bottom - (band.Top + (barWidth * 2.5f))), band.Bottom, barWidth, g);

            // band around the outside...
            this.DrawThickBox(XBrushes.Black, rect, barWidth, g);

            // boxes in the corners...
            var box1 = new XRectangleF(rect.Left, rect.Top, bandHeight, bandHeight);
            g.FillRectangle(XBrushes.White, box1.Inflate(barWidth));
            this.DrawThickBox(XBrushes.Black, box1, barWidth, g);

            var box2 = new XRectangleF(rect.Right - bandHeight, rect.Top, bandHeight, bandHeight);
            g.FillRectangle(XBrushes.White, box2.Inflate(barWidth));
            this.DrawThickBox(XBrushes.Black, box2, barWidth, g);

            var box3 = new XRectangleF(rect.Left, rect.Bottom - bandHeight, bandHeight, bandHeight);
            g.FillRectangle(XBrushes.White, box3.Inflate(barWidth));
            this.DrawThickBox(XBrushes.Black, box3, barWidth, g);

            var box4 = new XRectangleF(rect.Right - bandHeight, rect.Bottom - bandHeight, bandHeight, bandHeight);
            g.FillRectangle(XBrushes.White, box4.Inflate(barWidth));
            this.DrawThickBox(XBrushes.Black, box4, barWidth, g);
        }

        private void DrawThickLine(XBrush brush, float x1, float y1, float x2, float y2, float barWidth, IGraphics g, float nudgeX = 0, float nudgeY = 0)
        {
            var midBarWidth = barWidth / 2;

            var points = new List<PointF>();

            if (x1 >= x2 || y1 >= y2)
            {
                points.Add(new PointF(x1 - midBarWidth + nudgeX, y1 - midBarWidth + nudgeY));
                points.Add(new PointF(x1 + midBarWidth + nudgeX, y1 + midBarWidth + nudgeY));
                points.Add(new PointF(x2 + midBarWidth + nudgeX, y2 + midBarWidth + nudgeY));
                points.Add(new PointF(x2 - midBarWidth + nudgeX, y2 - midBarWidth + nudgeY));
                points.Add(new PointF(x1 - midBarWidth + nudgeX, y1 - midBarWidth + nudgeY));
            }
            else
            {
                points.Add(new PointF(x1 - midBarWidth + nudgeX, y1 + midBarWidth + nudgeY));
                points.Add(new PointF(x1 + midBarWidth + nudgeX, y1 - midBarWidth + nudgeY));
                points.Add(new PointF(x2 + midBarWidth + nudgeX, y2 - midBarWidth + nudgeY));
                points.Add(new PointF(x2 - midBarWidth + nudgeX, y2 + midBarWidth + nudgeY));
                points.Add(new PointF(x1 - midBarWidth + nudgeX, y1 + midBarWidth + nudgeY));
            }

            var theG = ((GdiGraphics)g).Graphics;

            using (var theBrush = brush.ToBrush())
                theG.FillPolygon(theBrush, points.ToArray());
        }

        private void DrawDiamond(XBrush brush, XRectangleF rect, float barWidth, IGraphics g)
        {
            var adjust = barWidth / 2;

            this.DrawThickLine(XBrushes.Black, rect.Left, rect.MidHeight, rect.Left + (rect.Width / 2) + adjust, rect.Top - adjust, barWidth, g);
            this.DrawThickLine(XBrushes.Black, rect.MidWidth , rect.Bottom, rect.Right + adjust, rect.MidHeight - adjust, barWidth, g);
            this.DrawThickLine(XBrushes.Black, rect.Left - adjust, rect.MidHeight - adjust, rect.MidWidth + adjust, rect.Bottom + adjust, barWidth, g);
            this.DrawThickLine(XBrushes.Black, rect.MidWidth, rect.Top, rect.Right, rect.MidHeight, barWidth, g);
        }

        private void DrawThickBox(XBrush brush, XRectangleF rect, float barWidth, IGraphics g)
        {
            g.FillRectangle(XBrushes.Black, new XRectangleF(rect.Left, rect.Top, rect.Width, barWidth));
            g.FillRectangle(XBrushes.Black, new XRectangleF(rect.Left, rect.Bottom - barWidth, rect.Width, barWidth));
            g.FillRectangle(XBrushes.Black, new XRectangleF(rect.Left, rect.Top, barWidth, rect.Height));
            g.FillRectangle(XBrushes.Black, new XRectangleF(rect.Right - barWidth, rect.Top, barWidth, rect.Height));
        }
    }
}