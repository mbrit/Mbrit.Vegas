using BootFX.Common;
using Mbrit.Vegas.Cards.Objects;
using Mbrit.Vegas.Lens;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using Mbrit.Vegas.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    internal class CardRenderer : CardRendererBase
    {
        public string Rank { get; }
        public Suit Suit { get; }

        internal CardRenderer(Suit suit, int rank, float scale = 1f)
            : base(scale)
        {
            this.Suit = suit;

            if (rank == 1)
                this.Rank = "A";
            else if (rank == 11)
                this.Rank = "J";
            else if (rank == 12)
                this.Rank = "Q";
            else if (rank == 13)
                this.Rank = "K";
            else
                this.Rank = rank.ToString();
        }

        protected override void DoRender(XRectangleF rect, CardStyle style, IGraphics g)
        {
            // inner...
            var edgeX = this.ScaleF(2.25f);
            var edgeY = this.ScaleF(9);
            var inner = new XRectangleF(rect.Left + edgeX, rect.Top + edgeY, rect.Width - (2 * edgeX), rect.Height - (2 * edgeY));

            // score...
            var padding = this.ScaleF(1.5f);
            var scoreHeight = (rect.Bottom - inner.Bottom - (2 * padding));
            var scoreWidth = scoreHeight * 3;

            var score1 = new XRectangleF(rect.Right - scoreWidth - padding, rect.Top + padding, scoreWidth, scoreHeight);
            this.RenderScore(score1, false, style, g);

            var score2 = new XRectangleF(rect.Left + padding, rect.Bottom - scoreHeight - padding, scoreWidth, scoreHeight);
            this.RenderScore(score2, true, style, g);

            // grid...
            var groups = this.DrawGrid(inner, style, g).ToList();

            var endX = groups[2].Rectangle.Left;
            var midX = groups[1].Rectangle.Left;
            var farX = groups[2].Rectangle.Right;

            // labels...
            var smallPadding = padding / 2;
            var labelHeight = score1.Height - (2 * smallPadding);
            var location = new XRectangleF(inner.Left, this.AlignVertical(score1, labelHeight), endX - inner.Left, labelHeight);
            this.RenderLabel(location, "Start Location", style, g);

            var date = new XRectangleF(midX, this.AlignVertical(score2, labelHeight), endX - midX - (padding / 2), labelHeight);
            this.RenderLabel(date, "Date", style, g);

            var profit = new XRectangleF(endX + padding - (padding / 2), date.Top, farX - (endX + padding) + (padding / 2), date.Height);
            this.RenderLabel(profit, "Profit/Loss", style, g);

            /*
            g.DrawLine(XPens.Green, inner.Left, rect.Top, inner.Left, rect.Bottom);
            g.DrawLine(XPens.Green, inner.Right, rect.Top, inner.Right, rect.Bottom);
            g.DrawLine(XPens.Green, inner.Left, inner.Top, inner.Right, inner.Top);
            g.DrawLine(XPens.Green, inner.Left, inner.Bottom, inner.Right, inner.Bottom);

            g.DrawLine(XPens.Red, rect.Left, rect.Top, rect.Right, rect.Bottom);
            g.DrawLine(XPens.Red, rect.Left, rect.Bottom, rect.Right, rect.Top);

            g.DrawLine(XPens.Red, inner.Left, inner.Top, inner.Right, inner.Bottom);
            g.DrawLine(XPens.Red, inner.Left, inner.Bottom, inner.Right, inner.Top);
            */
        }

        private float AlignVertical(XRectangleF rect, float height) => rect.MidHeight - (height / 2);

        private IEnumerable<CardGroup> DrawGrid(XRectangleF rect, CardStyle style, IGraphics g)
        {
            /*
            var footerHeight = this.ScaleF(1);

            var headerPartHeight = footerHeight;
            var headerHeight = headerPartHeight * 2;
            var header = new XRectangleF(rect.Left, rect.Top, rect.Width, headerHeight);
            g.DrawLine(XPens.Black, header.Left, header.Bottom, header.Right, header.Bottom);

            var header1 = new XRectangleF(header.Left, header.Top, header.Width, headerPartHeight);
            var header2 = new XRectangleF(header.Left, header1.Bottom, header.Width, headerPartHeight);

            var footer = new XRectangleF(rect.Left, rect.Bottom - footerHeight, rect.Width, footerHeight);
            g.DrawLine(XPens.Black, footer.Left, footer.Top, footer.Right, footer.Top);

            var body = new XRectangleF(header.Left, header.Bottom, rect.Width, footer.Top - header.Bottom);
            */

            var body = rect;

            // create a left and right...
            const int numGroups = 3;
            var groupWidth = body.Width / numGroups;

            // define the columns...
            var columns = new List<CardColumn>();
            columns.Add(new CardColumn("Hand")
            {
                HasSeparator = false
            });
            columns.Add(new CardColumn("Wager"));
            columns.Add(new CardColumn("Win/Lose"));
            columns.Add(new CardColumn("Game"));
            columns.Add(new CardColumn("ER"));

            var widths = new List<float>() { 0.125f, 0.2f, 0.2f, 0.2f, 0.2f };

            if (widths.Count != columns.Count)
                throw new InvalidOperationException("Width/columns mismatch.");

            var total = widths.Sum();
            var adjust = 1 / total;

            if (adjust != 1)
            {
                for (var index = 0; index < widths.Count; index++)
                    widths[index] *= adjust;
            }

            for (var index = 0; index < columns.Count; index++)
                columns[index].Width = groupWidth * widths[index];

            // build up the groups...
            var x = body.Left;
            var groups = new List<CardGroup>();
            var hand = 1;
            const int handsPerGroup = 9;
            for(var index = 0; index < numGroups; index++)
            {
                var groupRect = new XRectangleF(x, body.Top, groupWidth, body.Height);
                var endHand = hand + handsPerGroup;
                var group = new CardGroup(groupRect, hand, endHand - 1);
                groups.Add(group);

                x += groupWidth;
                hand = endHand;
            }

            var ends = new List<float>();
            for(var index = 0; index < groups.Count; index++)
            {
                var group = groups[index];

                var end = this.RenderGroup(group, columns, style, g);
                ends.Add(end);

                g.DrawLine(style.GridPen, group.Rectangle.Left, group.Rectangle.Top, group.Rectangle.Left, group.Rectangle.Bottom);
            }

            g.DrawLine(style.GridPen, body.Left, body.Top, body.Right, body.Top);

            var midX = groups[groups.Count - 2].Rectangle.Right;
            g.DrawLine(style.GridPen, body.Left, body.Bottom, midX, body.Bottom);

            var finalY = ends.Last();
            g.DrawLine(style.GridPen, midX, finalY, body.Right, finalY);

            g.DrawLine(style.GridPen, body.Right, body.Top, body.Right, finalY);

            var instructions = new XRectangleF(midX, finalY, body.Right - midX, body.Bottom - finalY);
            this.RenderInstructions(instructions, columns, style, g);

            return groups;
        }

        private void RenderInstructions(XRectangleF instructions, List<CardColumn> columns, CardStyle style, IGraphics g)
        {
            var midXs = new List<float>();
            var x = instructions.Left;
            foreach (var column in columns)
            {
                midXs.Add(x + (column.Width / 2));
                x += column.Width;
            }

            this.RenderInstruction(columns[1].Label, midXs[1], instructions, 0, style, g);
            this.RenderInstruction(columns[2].Label, midXs[2], instructions, 1, style, g);
            this.RenderInstruction(columns[3].Label, midXs[3], instructions, 0, style, g);
            this.RenderInstruction(columns[4].Label, midXs[4], instructions, 1, style, g);
        }

        private void RenderInstruction(string label, float x, XRectangleF rect, int row, CardStyle style, IGraphics g)
        {
            const int numRows = 3;
            var rowHeight = rect.Height / numRows;

            var y = rect.Top + ((row + 1) * rowHeight);
              
            g.DrawLine(style.LightGridPen, x, rect.Top - (rowHeight / 2), x, y);

            var font = style.HandFont;
            var size = g.MeasureString(label, font);
            g.DrawString(label, font, style.LabelBrush, x - (size.Width / 2), y + (rowHeight * .1f));
        }

        private float RenderGroup(CardGroup group, IEnumerable<CardColumn> columns, CardStyle style, IGraphics g)
        {
            var rowHeight = group.Rectangle.Height / group.NumRows;

            var y = group.Rectangle.Top;
            var hand = group.StartHand;
            for(var index = 0; index < group.NumRows; index++)
            {
                XBrush brush = null;
                XBrush labelBrush = null;
                if (index % 2 == 0)
                {
                    brush = style.Row1Brush;
                    labelBrush = style.DarkLabelBrush;
                }
                else
                {
                    brush = style.Row2Brush;
                    labelBrush = style.LabelBrush;
                }

                var rect = new XRectangleF(group.Rectangle.Left, y, group.Rectangle.Width, rowHeight);
                g.FillRectangle(brush, rect);

                // hand number...
                var numberRect = rect.Inflate(.75f, .5f);
                g.DrawString(hand.ToString(), style.HandFont, labelBrush, numberRect.Left, numberRect.Top);

                y += rowHeight;

                hand++;
                if (hand > MaxHands)
                    break;
            }

            var x = group.Rectangle.Left;
            foreach (var column in columns)
            {
                x += column.Width;

                if(column.HasSeparator)
                    g.DrawLine(style.LightGridPen, x, group.Rectangle.Top, x, y);
            }

            return y;
        }

        private void RenderScore(XRectangleF score, bool invert, CardStyle style, IGraphics g)
        {
            XBrush brush = null;
            if (this.Suit == Suit.Diamond || this.Suit == Suit.Heart)
                brush = style.RedRankBrush;
            else if (this.Suit == Suit.Club || this.Suit == Suit.Spade)
                brush = style.BlackRankBrush;
            else
                throw new NotSupportedException($"Cannot handle '{this.Suit}'.");

            var adjust = 0;

            var format = new XStringFormat()
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XStringAlignment.Center
            };

            var nudge = (score.Height / 6);

            if (!(invert))
            {
                var x = score.Right - (score.Height / 2) - adjust;
                var y = score.Top;

                var rankRect = new XRectangleF(x, y - score.Height / 2, score.Height, score.Height);
                var suitRect = new XRectangleF(x, y - (score.Height / 2) + score.Height + nudge, score.Height, score.Height);

                using (g.RotateAround(x, y, 90))
                {
                    g.DrawStringTight(this.Rank.ToString(), style.RankFont, brush, rankRect, format);
                    g.DrawStringTight(GetSuitSymbol(this.Suit), style.SuitFont, brush, suitRect, format);
                }
            }
            else
            {
                var x = score.Left + (score.Height / 2) + adjust;
                var y = score.Bottom;

                var rankRect = new XRectangleF(x, y - score.Height / 2, score.Height, score.Height);
                var suitRect = new XRectangleF(x, y - (score.Height / 2) + score.Height + nudge, score.Height, score.Height);

                using (g.RotateAround(x, y, 270))
                {
                    g.DrawStringTight(this.Rank.ToString(), style.RankFont, brush, rankRect, format);
                    g.DrawStringTight(GetSuitSymbol(this.Suit), style.SuitFont, brush, suitRect, format);
                }
            }
        }
    }
}
