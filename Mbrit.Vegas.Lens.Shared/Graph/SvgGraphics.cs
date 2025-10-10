using Mbrit.Vegas.Lens.Gdi;
using Mbrit.Vegas.Lens.Graph;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace Mbrit.Vegas.Lens
{
    public class SvgGraphics : IGraphics
    {
        private readonly XmlDocument _doc;
        private readonly XmlElement _svg;
        private readonly XmlNamespaceManager _nsMgr;
        public XRectangleF ClientRect => new XRectangleF(0, 0, 0, 0);

        public SvgGraphics(float width = 1024, float height = 768)
        {
            _doc = new XmlDocument();

            // Create root <svg> element
            _svg = _doc.CreateElement("svg", "http://www.w3.org/2000/svg");
            _svg.SetAttribute("width", width.ToString(CultureInfo.InvariantCulture));
            _svg.SetAttribute("height", height.ToString(CultureInfo.InvariantCulture));
            _svg.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
            _doc.AppendChild(_svg);

            _nsMgr = new XmlNamespaceManager(_doc.NameTable);
            _nsMgr.AddNamespace("svg", "http://www.w3.org/2000/svg");
        }

        public XmlDocument GetDocument() => _doc;

        public string GetSvgString()
        {
            using var stringWriter = new System.IO.StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
            _doc.Save(xmlWriter);
            return stringWriter.ToString();
        }

        public override string ToString() => this.GetSvgString();

        public void DrawLine(XPen pen, float x1, float y1, float x2, float y2)
        {
            var line = _doc.CreateElement("line", _svg.NamespaceURI);
            line.SetAttribute("x1", x1.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("y1", y1.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("x2", x2.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("y2", y2.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("stroke", ColorToHex(pen.Color));
            line.SetAttribute("stroke-width", pen.Width.ToString(CultureInfo.InvariantCulture));
            _svg.AppendChild(line);
        }

        public void FillRectangle(XBrush brush, XRectangleF rect)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        public void DrawRectangle(XPen pen, XRectangleF rect)
        {
            var rectElem = _doc.CreateElement("rect", _svg.NamespaceURI);
            rectElem.SetAttribute("x", rect.X.ToString(CultureInfo.InvariantCulture));
            rectElem.SetAttribute("y", rect.Y.ToString(CultureInfo.InvariantCulture));
            rectElem.SetAttribute("width", rect.Width.ToString(CultureInfo.InvariantCulture));
            rectElem.SetAttribute("height", rect.Height.ToString(CultureInfo.InvariantCulture));
            rectElem.SetAttribute("stroke", ColorToHex(pen.Color));
            rectElem.SetAttribute("fill", "none");
            rectElem.SetAttribute("stroke-width", pen.Width.ToString(CultureInfo.InvariantCulture));
            _svg.AppendChild(rectElem);
        }

        public void DrawString(string buf, XFont font, XBrush brush, float x, float y, XStringFormat yAxisFormat)
        {
            var text = _doc.CreateElement("text", _svg.NamespaceURI);
            text.SetAttribute("x", x.ToString(CultureInfo.InvariantCulture));
            text.SetAttribute("y", y.ToString(CultureInfo.InvariantCulture));
            text.SetAttribute("fill", BrushColor(brush));
            text.SetAttribute("font-size", font.Size.ToString(CultureInfo.InvariantCulture));
            text.SetAttribute("font-family", font.FontFamily.Name);
            text.InnerText = buf;
            _svg.AppendChild(text);
        }

        public void FillEllipse(XBrush brush, float x, float y, int width, int height)
        {
            var ellipse = _doc.CreateElement("ellipse", _svg.NamespaceURI);
            ellipse.SetAttribute("cx", (x + width / 2f).ToString(CultureInfo.InvariantCulture));
            ellipse.SetAttribute("cy", (y + height / 2f).ToString(CultureInfo.InvariantCulture));
            ellipse.SetAttribute("rx", (width / 2f).ToString(CultureInfo.InvariantCulture));
            ellipse.SetAttribute("ry", (height / 2f).ToString(CultureInfo.InvariantCulture));
            ellipse.SetAttribute("fill", BrushColor(brush));
            _svg.AppendChild(ellipse);
        }

        public void FillPolygon(XBrush brush, XPointF[] points)
        {
            var polygon = _doc.CreateElement("polygon", _svg.NamespaceURI);
            var pointStr = string.Join(" ", Array.ConvertAll(points, p => $"{p.X.ToString(CultureInfo.InvariantCulture)},{p.Y.ToString(CultureInfo.InvariantCulture)}"));
            polygon.SetAttribute("points", pointStr);
            polygon.SetAttribute("fill", BrushColor(brush));
            _svg.AppendChild(polygon);
        }

        public void DrawArrow(XColor color, XPointF start, XPointF end)
        {
            // Define marker only once
            if (_doc.SelectSingleNode("//svg:defs", _nsMgr) == null)
            {
                var defs = _doc.CreateElement("defs", _svg.NamespaceURI);
                var marker = _doc.CreateElement("marker", _svg.NamespaceURI);
                marker.SetAttribute("id", "arrow");
                marker.SetAttribute("markerWidth", "10");
                marker.SetAttribute("markerHeight", "10");
                marker.SetAttribute("refX", "5");
                marker.SetAttribute("refY", "5");
                marker.SetAttribute("orient", "auto");
                marker.SetAttribute("markerUnits", "strokeWidth");

                var path = _doc.CreateElement("path", _svg.NamespaceURI);
                path.SetAttribute("d", "M0,0 L10,5 L0,10 z");
                path.SetAttribute("fill", ColorToHex(color));
                marker.AppendChild(path);
                defs.AppendChild(marker);
                _svg.AppendChild(defs);
            }

            var line = _doc.CreateElement("line", _svg.NamespaceURI);
            line.SetAttribute("x1", start.X.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("y1", start.Y.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("x2", end.X.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("y2", end.Y.ToString(CultureInfo.InvariantCulture));
            line.SetAttribute("stroke", ColorToHex(color));
            line.SetAttribute("stroke-width", "2");
            line.SetAttribute("marker-end", "url(#arrow)");
            _svg.AppendChild(line);
        }

        public void DrawPoint(XPen pen, float x, float y)
        {
            var circle = _doc.CreateElement("circle", _svg.NamespaceURI);
            circle.SetAttribute("cx", x.ToString(CultureInfo.InvariantCulture));
            circle.SetAttribute("cy", y.ToString(CultureInfo.InvariantCulture));
            circle.SetAttribute("r", "1");
            circle.SetAttribute("fill", ColorToHex(pen.Color));
            _svg.AppendChild(circle);
        }

        private static string ColorToHex(XColor color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private static string BrushColor(XBrush brush)
        {
            return brush is XBrush sb ? ColorToHex(sb.Color) : "#000";
        }

        public IDisposable RotateAround(float x, float y, float angle)
        {
            throw new NotImplementedException();
        }

        public void DrawStringTight(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
            throw new NotImplementedException();
        }

        public void DrawString(string label, XFont font, XBrush brush, XRectangleF rect, XStringFormat format)
        {
            throw new NotImplementedException();
        }

        public XSizeF MeasureString(string buf, XFont font) => throw new NotImplementedException("This operation has not been implemented.");
    }
}