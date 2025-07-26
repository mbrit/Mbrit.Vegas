using BootFX.Common;
using Mbrit.Vegas.Simulator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Formats.Asn1.AsnWriter;

namespace Mbrit.Vegas.Lens.Graph
{
    internal class GraphGenerator
    {
        private RectangleF PlotArea { get; }
        private RectangleF LegendArea1 { get; }
        private RectangleF LegendArea2 { get; }
        private float MidY { get; }
        private float ScaleY { get; }
        public IWinLoseDrawRoundsBucket Rounds { get; }
        public List<float> XTicks { get; }
        private float Unit { get; }
        public IEnumerable<float> WalkProfits { get; }
        public IEnumerable<WinLoseDrawType> WalkVectors { get; }
        public float WalkHouseEdge { get; }

        //public float NormalGameHouseEdge { get; }
        //public float WalkGameHouseEdge { get; }
        public List<HouseEdgeIllustration> HouseEdgeIllustrations { get; }
        public int BoxHandsMax { get; }
        public int BoxHandsOptimal { get; }
        public bool ShowBoxHands { get; }
        public bool ShowWedge { get; }

        private const float YAxisWidth = 40f;
        private const float RightMargin = 40f;
        private const float LegendHeight = 20f;
        private const float TickHeight = 2.5f;
        private const int MaxInvestments = 15;

        internal GraphGenerator(Rectangle rect, IWinLoseDrawRoundsBucket rounds,
            IEnumerable<WinLoseDrawType> walkVectors, IEnumerable<float> walkProfits, float walkHouseEdge,
            float unit, IEnumerable<HouseEdgeIllustration> houseEdges, 
            int boxHandsMax, int boxHandsOptimal, bool showBoxHands, bool showWedge)
        {
            this.LegendArea1 = new RectangleF(rect.Left + YAxisWidth, rect.Top, rect.Width - (YAxisWidth * 2) - RightMargin, LegendHeight);

            // y axis...
            this.PlotArea = new RectangleF(rect.Left + YAxisWidth, LegendArea1.Bottom, rect.Width - (YAxisWidth * 2) - RightMargin, rect.Bottom - LegendArea1.Bottom);

            this.MidY = (this.PlotArea.Height / 2) + this.PlotArea.Top;

            var maxProfit = unit * MaxInvestments;
            var maxLoss = 0 - (unit * MaxInvestments);

            var diff = maxProfit + (0 - maxLoss);

            this.ScaleY = this.PlotArea.Height / diff;

            this.Rounds = rounds;

            // make the ticks...
            int numXTicks = this.Rounds[0].Count;
            var xTicks = new List<float>();

            var per = this.PlotArea.Width / numXTicks;
            var x = this.PlotArea.Left + per;
            for (var index = 0; index < numXTicks; index++)
            {
                xTicks.Add(x);
                x += per;
            }
            this.XTicks = xTicks;

            // game...
            this.Unit = unit;
            this.WalkProfits = new List<float>(walkProfits);
            this.WalkVectors = new List<WinLoseDrawType>(walkVectors);
            this.WalkHouseEdge = walkHouseEdge;
            this.HouseEdgeIllustrations = new List<HouseEdgeIllustration>(houseEdges);

            this.BoxHandsMax = boxHandsMax;
            this.BoxHandsOptimal = boxHandsOptimal;
            this.ShowBoxHands = showBoxHands;
            this.ShowWedge = showWedge;
        }

        public float NormalGameHouseEdge => (float)this.Rounds.HouseEdge;

        private void MarkRectangle(RectangleF rect, Graphics g)
        {
            using(var pen = new Pen(Color.FromArgb(0xff, 0, 0xff)))
            {
                g.DrawRectangle(pen, rect);
                g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Bottom);
                g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Top);
            }
        }

        private bool HasWalkGame => this.WalkProfits.Any();

        private Font GetFont() => new Font("Tahoma", 8); 

        internal void Render(Graphics theG)
        {
            theG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //var asList = this.Vectors.ToList();
            var walkProfitsAsList = this.WalkProfits.ToList();

            //this.MarkRectangle(this.LegendArea1, g);
            //this.MarkRectangle(this.PlotArea, g);

            using (var font = this.GetFont())
            {
                var heEnds = new List<PointF>();
                var wedgeEnd = PointF.Empty;

                var max = 1;
                if (this.ShowWedge)
                    max = 2;

                for (var loop = 0; loop < max; loop++)
                {
                    IGraphics g = null;
                    if (loop == 0 && max != 1)
                        g = new NullGraphics();
                    else if (loop == 1 || max == 1)
                        g = new GdiGraphics(theG);
                    else
                        throw new NotSupportedException($"Cannot handle '{loop}'.");

                    var yAxisFormat = new StringFormat()
                    {
                        Alignment = StringAlignment.Far
                    };

                    // y axis...
                    for (var index = 0 - MaxInvestments; index < MaxInvestments; index++)
                    {
                        var amount = index * this.Unit;

                        var y = this.CalcY(amount);
                        g.DrawLine(Pens.Black, this.PlotArea.Left - TickHeight, y, this.PlotArea.Left + TickHeight, y);

                        if (index % 2 == 0)
                            g.DrawString(amount.ToString(), font, Brushes.Gray, this.PlotArea.Left - TickHeight, y - 8, yAxisFormat);
                    }

                    // x axis...
                    g.DrawLine(Pens.Black, this.PlotArea.Left, this.PlotArea.Top, this.PlotArea.Left, this.PlotArea.Bottom);

                    g.DrawLine(Pens.Black, this.PlotArea.Left, this.MidY, this.XTicks.Max(), this.MidY);

                    var tickIndex = 0;
                    using (var gridPen = new Pen(Color.FromArgb(0xe0, 0xe0, 0xe0), 1))
                    {
                        foreach (var tick in this.XTicks)
                        {
                            if (tickIndex > 0 && tickIndex % 10 == 0)
                                g.DrawLine(gridPen, tick, this.PlotArea.Top, tick, this.PlotArea.Bottom);

                            g.DrawLine(Pens.Black, tick, this.MidY - TickHeight, tick, this.MidY + TickHeight);

                            if (tickIndex % 2 == 0)
                                g.DrawString((tickIndex + 1).ToString(), font, Brushes.Gray, tick, this.MidY + TickHeight);

                            tickIndex++;
                        }
                    }

                    // wedge...
                    if (loop == 1 && this.ShowWedge)
                    {
                        var wedge = new List<PointF>();
                        wedge.Add(new PointF(this.PlotArea.X, this.MidY));
                        wedge.Add(wedgeEnd);

                        var heEnd = heEnds.First();
                        wedge.Add(heEnd);

                        using(var brush = new SolidBrush(Color.FromArgb(128, 0x7f, 0xff, 0xd4)))
                            g.FillPolygon(brush, wedge.ToArray());

                        g.DrawArrow(Color.Red, heEnd, wedgeEnd);
                    }

                    tickIndex = 0;
                    foreach (var tick in this.XTicks)
                    {
                        if (tickIndex % 2 == 0)
                            g.DrawString((tickIndex + 1).ToString(), font, Brushes.Gray, tick, this.MidY + TickHeight);

                        tickIndex++;
                    }

                    // bounding...
                    if (this.ShowBoxHands || this.HasWalkGame)
                    {
                        var c = Color.FromArgb(0x80, 0x90, 0xa0);

                        this.DrawBoundingBox(this.BoxHandsOptimal, 6, c, 2, g);
                        this.DrawBoundingBox(this.BoxHandsMax, 6, Color.Black, 2, g);

                        this.DrawBoundingBox(this.BoxHandsOptimal, 12, c, 2, g);
                        this.DrawBoundingBox(this.BoxHandsMax, 12, Color.Black, 2, g);
                    }

                    // slope...
                    foreach (var houseEdge in this.HouseEdgeIllustrations)
                    {
                        var slopes = new List<float>();

                        var slopeBankroll = 0f;
                        for (var index = 1; index < this.XTicks.Count; index++)
                        {
                            slopes.Add(slopeBankroll);
                            slopeBankroll -= this.Unit * houseEdge.Amount;
                        }

                        var end = this.PlotLine(slopes, houseEdge.Colour, 1, houseEdge.Dash, this.FormatPercentage(houseEdge.Amount), g);
                        heEnds.Add(end);
                    }

                    // plot the game...
                    if (this.Rounds.Count > 0)
                    {
                        var profits = new float[this.XTicks.Count, this.Rounds.Count];
                        var winCounts = new int[this.XTicks.Count];

                        for (var i = 0; i < this.Rounds.Count; i++)
                        {
                            var round = this.Rounds[i];

                            var bankroll = 12 * this.Unit;
                            var initial = bankroll;

                            for (var j = 0; j < this.XTicks.Count; j++)
                            {
                                bankroll -= this.Unit;

                                var result = round.GetResult(j);

                                if (result == WinLoseDrawType.Win)
                                {
                                    bankroll += (this.Unit + this.Unit);
                                    winCounts[j]++;
                                }
                                else if (result == WinLoseDrawType.Lose)
                                {
                                    // no-op...
                                }
                                else
                                    throw new NotSupportedException($"Cannot handle '{result}'.");

                                var profit = bankroll - initial;
                                profits[j, i] = profit;
                            }
                        }

                        var vectors = new List<WinLoseDrawType>();
                        var threshhold = (int)(((float)this.Rounds.Count / 2f) * 0.95f);
                        foreach (var winCount in winCounts)
                        {
                            if (this.Rounds.Count > 1)
                            {
                                if (winCount >= threshhold)
                                    vectors.Add(WinLoseDrawType.Win);
                                else
                                    vectors.Add(WinLoseDrawType.Lose);
                            }
                            else
                                vectors.Add(this.Rounds[0].GetResult(vectors.Count()));
                        }

                        var smoothed = new List<float>();
                        for (var j = 0; j < this.XTicks.Count; j++)
                        {
                            var forPoint = new List<float>();
                            for (var i = 0; i < this.Rounds.Count; i++)
                                forPoint.Add(profits[j, i]);

                            smoothed.Add(forPoint.Average());
                        }

                        this.PlotLine(smoothed, Color.Blue, 2, DashStyle.Solid, g);

                        var angle = 0f;
                        var trend = this.GetTrendLine(smoothed, ref angle);
                        wedgeEnd = this.PlotLine(trend, Color.Black, 2, DashStyle.Dash, "#" + this.Rounds.Count + " @ " + this.FormatPercentage(this.NormalGameHouseEdge), g);

                        this.RenderLegend(vectors, this.LegendArea1, g);
                    }

                    // walk game profits...
                    if (this.HasWalkGame)
                    {
                        this.PlotLine(this.WalkProfits, Color.Green, 2, DashStyle.Solid, "Walk Game " + this.FormatPercentage(this.WalkHouseEdge), g);

                        var legendY = this.CalcY(6 * this.Unit);
                        var legendRect = new RectangleF(this.PlotArea.Left, legendY, this.PlotArea.Width, this.MidY - legendY);
                        this.RenderLegend(this.WalkVectors, legendRect, g);
                    }
                }
            }
        }

        private void RenderLegend(IEnumerable<WinLoseDrawType> vectors, RectangleF rect, IGraphics g)
        {
            using (var font = this.GetFont())
            {
                var asList = vectors.ToList();

                for (var index = 0; index < asList.Count; index++)
                {
                    var result = asList[index];

                    const int size = 6;
                    var x = this.CalcX(index);

                    var y = rect.Top + size;
                    Brush brush = null;
                    if (result == WinLoseDrawType.Win)
                        brush = Brushes.Green;
                    else if (result == WinLoseDrawType.Lose)
                    {
                        brush = Brushes.Red;
                        y += size;
                    }
                    else
                        throw new NotSupportedException($"Cannot handle '{result}'.");

                    g.FillEllipse(brush, x - size, y - size, size, size);
                }
            }
        }

        private string FormatPercentage(float value) => (value * 100) + "%";

        private void DrawBoundingBox(int value, int highY, Color colour, int width, IGraphics g)
        {
            using(var pen = new Pen(colour, width)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
            })
            {
                var fromY = this.CalcY(this.Unit * highY);
                var toY = this.CalcY(0 - (this.Unit * 12));

                var x = this.XTicks[value];

                var rect = new RectangleF(this.PlotArea.Left, fromY, x - this.PlotArea.Left, toY - fromY);
                g.DrawRectangle(pen, rect);
            }
        }

        private PointF PlotLine(IEnumerable<float> values, Color colour, int width, DashStyle dash, IGraphics g) =>
            this.PlotLine(values, colour, width, dash, string.Empty, g);

        private PointF PlotLine(IEnumerable<float> values, Color colour, int width, DashStyle dash, string label, IGraphics g)
        {
            using (var pen = new Pen(colour, width)
            {
                DashStyle = dash
            })
            {
                var lastProfitX = this.PlotArea.Left;
                var lastProfitY = this.MidY;

                var index = 0;
                foreach (var value in values)
                {
                    var profitX = this.XTicks[index];
                    var profitY = this.CalcY(value);

                    g.DrawLine(pen, lastProfitX, lastProfitY, profitX, profitY);

                    lastProfitX = profitX;
                    lastProfitY = profitY;

                    index++;
                }

                var pt = new PointF(lastProfitX, lastProfitY);

                if (!(string.IsNullOrEmpty(label)) && g != null)
                {
                    using (var font = this.GetFont())
                    {
                        using (var brush = new SolidBrush(colour))
                        {
                            g.DrawString(label, font, brush, pt.X, pt.Y - 6);
                        }
                    }
                }

                return pt;
            }
        }

        private float CalcX(int index) => this.XTicks[index];

        private float CalcY(float value)
        {
            if (value < 0)
                return this.MidY + ((0 - value) * this.ScaleY);
            else
                return this.MidY - (value * this.ScaleY);
        }

        private IEnumerable<float> GetTrendLine(IEnumerable<float> values, ref float angleDeg)
        {
            angleDeg = 0;

            var y = values.ToArray();
            int n = y.Length;
            if (n == 0)
                throw new ArgumentException("Cannot calculate trend line on empty list.");

            var x = Enumerable.Range(0, n).Select(i => (float)i).ToArray();

            float numerator = 0f;
            float denominator = 0f;

            for (int i = 0; i < n; i++)
            {
                numerator += x[i] * y[i];
                denominator += x[i] * x[i];
            }

            float slope = denominator == 0 ? 0 : numerator / denominator;

            angleDeg = (float)(Math.Atan(slope) * (180.0 / Math.PI));

            // Return y = mx (intercept is 0)
            var asList = x.Select(xi => slope * xi).ToList();
            asList.RemoveAt(asList.Count - 1);
            return asList;
        }
    }
}
