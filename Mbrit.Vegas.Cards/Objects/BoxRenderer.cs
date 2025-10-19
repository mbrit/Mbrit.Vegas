using BootFX.Common;
using Mbrit.Vegas.Lens.Graph;
using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Mbrit.Vegas.Cards
{
    internal class BoxRenderer
    {
        public void Render(RectangleF rect, Graphics g)
        {
            var y = rect.Top + rect.Height / 2;
            g.DrawLine(Pens.Black, rect.Left, y, rect.Right, y);

            var right = (float)(rect.Left + (rect.Right * .6f));
            var height = rect.Height * .66f;

            var nudge = rect.Height * .025f;

            var bottomRight = new PointF(right, y + (height / 2) + nudge);
            var topRight = new PointF(right, y - (height / 2) + nudge);
            g.DrawLine(Pens.Black, topRight, bottomRight);

            g.DrawLine(Pens.LightGray, bottomRight, new PointF(rect.Left, y));
            g.DrawLine(Pens.LightGray, topRight, new PointF(rect.Left, y));

            g.DrawLine(Pens.LightGray, bottomRight, new PointF(rect.Right, y));
            g.DrawLine(Pens.LightGray, topRight, new PointF(rect.Right, y));

            //var distX = right - rect.Left;
            //var distY = bottomRight.Y - y;
            //var step = distX / distY;

            var vanishingLeft = new PointF(rect.Left, y);
            var vanishingRight = new PointF(rect.Right, y);

            var width = rect.Width * .25f;

            //var bottomLeft = new PointF(bottomRight.X - width, bottomRight.Y - (width / step));
            //var topLeft = new PointF(topRight.X - width, topRight.Y + (width / step));
            var bottomLeft = this.ProjectPoint(bottomRight, vanishingLeft, width);
            var topLeft = this.ProjectPoint(topRight, vanishingLeft, width);

            g.DrawLine(Pens.Black, bottomLeft, topLeft);
            g.DrawLine(Pens.Black, topLeft, topRight);
            g.DrawLine(Pens.Black, bottomLeft, bottomRight);

            var depth = 0 - (rect.Width * .075f);
            var lowerBottomRight = this.ProjectPoint(bottomRight, vanishingRight, depth);
            var lowerTopRight = this.ProjectPoint(topRight, vanishingRight, depth);

            g.DrawLine(Pens.Black, lowerBottomRight, lowerTopRight);
            g.DrawLine(Pens.Black, topRight, lowerTopRight);
            g.DrawLine(Pens.Black, bottomRight, lowerBottomRight);

            var front = new List<PointF>() { topLeft, topRight, bottomLeft };

            using (var image = this.GetFrontImage())
                g.DrawImage(image, front.ToArray());
        }

        private PointF ProjectPoint(PointF start, PointF vanishing, float width)
        {
            var distX = start.X - vanishing.X;
            var distY = start.Y - vanishing.Y;
            var step = distX / distY;

            return new PointF(start.X - width, start.Y - (width / step));
        }

        private Assembly ResourceAssembly => this.GetType().Assembly;

        public Image GetFrontImage()
        {
            using (var stream = ResourceHelper.GetResourceStream(this.ResourceAssembly, "Mbrit.Vegas.Cards.Resources.TvwmBack.png")) 
                return Image.FromStream(stream);
        }
    }
}
