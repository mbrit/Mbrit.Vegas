using Mbrit.Vegas.Cards.Objects;
using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    internal class BackRenderer2 : CardRendererBase
    {
        internal enum StripeDirection
        {
            ToRight = 0,  // Top-left to bottom-right (45 degrees)
            ToLeft = 1,   // Top-right to bottom-left (-45 degrees)
            None = 2      // No stripes (solid white)
        }

        public class StripedRegion
        {
            public RectangleF Bounds { get; set; }
            public StripeDirection Direction { get; set; }
            public bool ClipToDiamond { get; set; } = false;

            public StripedRegion(RectangleF bounds, StripeDirection direction)
            {
                Bounds = bounds;
                Direction = direction;
            }
        }

        internal BackRenderer2(float scale)
            : base(scale)
        {
        }

        protected override void DoRender(XRectangleF rect, CardStyle style, IGraphics g)
        {
            this.DrawCardBack(((GdiGraphics)g).Graphics, rect.ToRectangleF(), 10);
        }

        private void DrawCardBack(Graphics g, RectangleF cardBounds, float stripeWidth, float borderWidth = 10)
        {
            // Clear background
            g.FillRectangle(Brushes.White, cardBounds);

            // Draw black border
            using (Pen borderPen = new Pen(Color.Black, borderWidth))
            {
                g.DrawRectangle(borderPen, cardBounds.X, cardBounds.Y, cardBounds.Width, cardBounds.Height);
            }

            // Calculate inner bounds (inside the border)
            RectangleF innerBounds = new RectangleF(
                cardBounds.X + borderWidth,
                cardBounds.Y + borderWidth,
                cardBounds.Width - borderWidth * 2,
                cardBounds.Height - borderWidth * 2
            );

            // Define regions for the card design
            var regions = CreateCardRegions(innerBounds);

            // Draw each region with its stripes
            foreach (var region in regions)
            {
                if (region.Direction != StripeDirection.None)
                {
                    DrawStripedRegion(g, region.Bounds, stripeWidth, region.Direction);
                }
                // White regions are already white from the background
            }

            // Draw the central diamond (white)
            DrawCentralDiamond(g, innerBounds);

            // Draw thin borders between regions (optional, for definition)
            DrawRegionBorders(g, regions);
        }

        private static List<StripedRegion> CreateCardRegions(RectangleF innerBounds)
        {
            var regions = new List<StripedRegion>();

            float cornerSize = innerBounds.Width * 0.15f; // Corner squares are 15% of width
            float centerBandWidth = innerBounds.Width * 0.3f; // Central vertical band
            float centerBandHeight = innerBounds.Height * 0.3f; // Central horizontal band

            float leftX = innerBounds.Left;
            float rightX = innerBounds.Right - cornerSize;
            float topY = innerBounds.Top;
            float bottomY = innerBounds.Bottom - cornerSize;
            float centerX = innerBounds.Left + (innerBounds.Width - centerBandWidth) / 2;
            float centerY = innerBounds.Top + (innerBounds.Height - centerBandHeight) / 2;

            // Top-left corner (white)
            regions.Add(new StripedRegion(
                new RectangleF(leftX, topY, cornerSize, cornerSize),
                StripeDirection.None
            ));

            // Top-right corner (white)
            regions.Add(new StripedRegion(
                new RectangleF(rightX, topY, cornerSize, cornerSize),
                StripeDirection.None
            ));

            // Bottom-left corner (white)
            regions.Add(new StripedRegion(
                new RectangleF(leftX, bottomY, cornerSize, cornerSize),
                StripeDirection.None
            ));

            // Bottom-right corner (white)
            regions.Add(new StripedRegion(
                new RectangleF(rightX, bottomY, cornerSize, cornerSize),
                StripeDirection.None
            ));

            // Top strip (between corners)
            regions.Add(new StripedRegion(
                new RectangleF(leftX + cornerSize, topY, innerBounds.Width - cornerSize * 2, cornerSize),
                StripeDirection.ToRight
            ));

            // Bottom strip (between corners)
            regions.Add(new StripedRegion(
                new RectangleF(leftX + cornerSize, bottomY, innerBounds.Width - cornerSize * 2, cornerSize),
                StripeDirection.ToRight
            ));

            // Left strip (between corners)
            regions.Add(new StripedRegion(
                new RectangleF(leftX, topY + cornerSize, cornerSize, innerBounds.Height - cornerSize * 2),
                StripeDirection.ToLeft
            ));

            // Right strip (between corners)
            regions.Add(new StripedRegion(
                new RectangleF(rightX, topY + cornerSize, cornerSize, innerBounds.Height - cornerSize * 2),
                StripeDirection.ToLeft
            ));

            // Central regions (creating the X pattern around the diamond)
            // Top-left quadrant
            regions.Add(new StripedRegion(
                new RectangleF(leftX + cornerSize, topY + cornerSize,
                                centerX - leftX - cornerSize, centerY - topY - cornerSize),
                StripeDirection.ToRight
            ));

            // Top-right quadrant
            regions.Add(new StripedRegion(
                new RectangleF(centerX + centerBandWidth, topY + cornerSize,
                                rightX - centerX - centerBandWidth, centerY - topY - cornerSize),
                StripeDirection.ToLeft
            ));

            // Bottom-left quadrant
            regions.Add(new StripedRegion(
                new RectangleF(leftX + cornerSize, centerY + centerBandHeight,
                                centerX - leftX - cornerSize, bottomY - centerY - centerBandHeight),
                StripeDirection.ToLeft
            ));

            // Bottom-right quadrant
            regions.Add(new StripedRegion(
                new RectangleF(centerX + centerBandWidth, centerY + centerBandHeight,
                                rightX - centerX - centerBandWidth, bottomY - centerY - centerBandHeight),
                StripeDirection.ToRight
            ));

            // Center cross bands around diamond
            // Top center
            regions.Add(new StripedRegion(
                new RectangleF(centerX, topY + cornerSize, centerBandWidth, centerY - topY - cornerSize),
                StripeDirection.ToLeft
            ));

            // Bottom center
            regions.Add(new StripedRegion(
                new RectangleF(centerX, centerY + centerBandHeight, centerBandWidth,
                                bottomY - centerY - centerBandHeight),
                StripeDirection.ToRight
            ));

            // Left center
            regions.Add(new StripedRegion(
                new RectangleF(leftX + cornerSize, centerY, centerX - leftX - cornerSize, centerBandHeight),
                StripeDirection.ToRight
            ));

            // Right center
            regions.Add(new StripedRegion(
                new RectangleF(centerX + centerBandWidth, centerY,
                                rightX - centerX - centerBandWidth, centerBandHeight),
                StripeDirection.ToLeft
            ));

            return regions;
        }

        private static void DrawStripedRegion(Graphics g, RectangleF bounds, float stripeWidth, StripeDirection direction)
        {
            // Save current clip
            var oldClip = g.Clip;

            // Clip to the region bounds
            g.SetClip(bounds);

            // Get the stripe polygons
            var stripePolygons = CalculateStripePolygons(bounds, stripeWidth, direction);

            // Draw black stripes (white is already the background)
            using (Brush blackBrush = new SolidBrush(Color.Black))
            {
                foreach (var polygon in stripePolygons)
                {
                    g.FillPolygon(blackBrush, polygon);
                }
            }

            // Restore clip
            g.Clip = oldClip;
        }

        private static void DrawCentralDiamond(Graphics g, RectangleF innerBounds)
        {
            float diamondSize = Math.Min(innerBounds.Width, innerBounds.Height) * 0.25f;
            float centerX = innerBounds.Left + innerBounds.Width / 2;
            float centerY = innerBounds.Top + innerBounds.Height / 2;

            PointF[] diamond = new PointF[]
            {
        new PointF(centerX, centerY - diamondSize / 2), // Top
        new PointF(centerX + diamondSize / 2, centerY), // Right
        new PointF(centerX, centerY + diamondSize / 2), // Bottom
        new PointF(centerX - diamondSize / 2, centerY)  // Left
            };

            // Fill white diamond
            g.FillPolygon(Brushes.White, diamond);

            // Draw black border around diamond
            using (Pen pen = new Pen(Color.Black, 3))
            {
                g.DrawPolygon(pen, diamond);
            }
        }

        private static void DrawRegionBorders(Graphics g, List<StripedRegion> regions)
        {
            using (Pen pen = new Pen(Color.Black, 1))
            {
                foreach (var region in regions)
                {
                    g.DrawRectangle(pen, region.Bounds.X, region.Bounds.Y,
                                    region.Bounds.Width, region.Bounds.Height);
                }
            }
        }

        // Reuse the stripe calculation from before
        public static List<PointF[]> CalculateStripePolygons(RectangleF bounds, float stripeWidth, StripeDirection direction)
        {
            var polygons = new List<PointF[]>();
            float stripeSpacing = stripeWidth * (float)Math.Sqrt(2);
            float diagonalWidth = bounds.Width + bounds.Height;
            int numStripes = (int)Math.Ceiling(diagonalWidth / stripeSpacing) + 1;

            for (int i = 0; i < numStripes; i += 2) // Skip every other stripe (only black ones)
            {
                float xOffset1 = i * stripeSpacing;
                float xOffset2 = (i + 1) * stripeSpacing;
                List<PointF> polygon = new List<PointF>();

                AddIntersectionPoints(bounds, xOffset1, polygon, true, direction);
                AddIntersectionPoints(bounds, xOffset2, polygon, false, direction);

                if (polygon.Count >= 3)
                {
                    polygons.Add(polygon.ToArray());
                }
            }
            return polygons;
        }

        private static void AddIntersectionPoints(RectangleF bounds, float offset,
                                                    List<PointF> polygon, bool forward,
                                                    StripeDirection direction)
        {
            List<PointF> points = new List<PointF>();

            if (direction == StripeDirection.ToRight)
            {
                // Top edge
                float xAtTop = bounds.Left + offset;
                if (xAtTop >= bounds.Left && xAtTop <= bounds.Right)
                    points.Add(new PointF(xAtTop, bounds.Top));

                // Right edge
                float yAtRight = bounds.Top + (bounds.Right - bounds.Left) - offset;
                if (yAtRight >= bounds.Top && yAtRight <= bounds.Bottom)
                    points.Add(new PointF(bounds.Right, yAtRight));

                // Bottom edge
                float xAtBottom = bounds.Left + offset + bounds.Height;
                if (xAtBottom >= bounds.Left && xAtBottom <= bounds.Right)
                    points.Add(new PointF(xAtBottom, bounds.Bottom));

                // Left edge
                float yAtLeft = bounds.Top + offset;
                if (yAtLeft >= bounds.Top && yAtLeft <= bounds.Bottom)
                    points.Add(new PointF(bounds.Left, yAtLeft));
            }
            else if (direction == StripeDirection.ToLeft)
            {
                // Top edge
                float xAtTop = bounds.Right - offset;
                if (xAtTop >= bounds.Left && xAtTop <= bounds.Right)
                    points.Add(new PointF(xAtTop, bounds.Top));

                // Left edge
                float yAtLeft = bounds.Top + (bounds.Right - bounds.Left) - offset;
                if (yAtLeft >= bounds.Top && yAtLeft <= bounds.Bottom)
                    points.Add(new PointF(bounds.Left, yAtLeft));

                // Bottom edge
                float xAtBottom = bounds.Right - offset - bounds.Height;
                if (xAtBottom >= bounds.Left && xAtBottom <= bounds.Right)
                    points.Add(new PointF(xAtBottom, bounds.Bottom));

                // Right edge
                float yAtRight = bounds.Top + offset;
                if (yAtRight >= bounds.Top && yAtRight <= bounds.Bottom)
                    points.Add(new PointF(bounds.Right, yAtRight));
            }

            if (forward)
                polygon.AddRange(points);
            else
            {
                points.Reverse();
                polygon.AddRange(points);
            }
        }
    }
}

