using Mbrit.Vegas.Lens.Graph;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    internal static class ICardRenderExtender
    {
        internal static void Render(this ICardRenderer renderer, Graphics g) => renderer.Render(new GdiGraphics(g));

        internal static string Export(this ICardRenderer renderer)
        {
            var widthMm = 500;
            var heightMm = 500;

            var path = Path.GetTempFileName() + ".png";

            using (var bitmap = new Bitmap((int)widthMm, (int)heightMm, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(bitmap))
                    renderer.Render(g);

                bitmap.Save(path, ImageFormat.Png);
            }

            return path;
        }
    }
}