using BootFX.Common;
using Mbrit.Vegas.Utility;
using System.Diagnostics;
using System.Globalization;
using PSP = PdfSharpCore.Pdf;
using PSD = PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Mbrit.Vegas.Lens;
using Mbrit.Vegas.Lens.Gdi;

namespace Mbrit.Vegas.Cards
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ICardRenderer GetRenderer(int scale)
        {
            return new NotepadRenderer(scale);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.GetRenderer(1).Render(e.Graphics);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            this.GetRenderer(2).Render(e.Graphics);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = this.GetRenderer(1).Export();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = @"c:\temp\cards--" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".pdf";

            PSP.PdfDocument doc = null;
            PSP.PdfPage page = null;

            // width...
            var trimsMm = 6f;

            var widthMm = 3.5f * CardRendererBase.InchesToMm;
            var heightMm = 2.5f * CardRendererBase.InchesToMm;

            var suits = new List<Suit>()
            {
                Suit.Club,
                Suit.Spade,
                Suit.Diamond,
                Suit.Heart,
            };

            var image = GetCardBackImage();
            using (image)
            {
                using (var theImage = image.ToPdfXImage())
                {
                    using (var pdf = PdfGraphics.Create(widthMm, heightMm, trimsMm, out doc))
                    {
                        foreach (var suit in suits)
                        {
                            for (var rank = 1; rank <= 13; rank++)
                            {
                                page = pdf.AddPage();

                                var renderer = new CardRenderer(suit, rank);

                                renderer.Render(pdf);
                                pdf.AddTrimMarksToPage();

                                // then the background...
                                page = pdf.AddPage();

                                this.RenderBackground(theImage, pdf);
                                pdf.AddTrimMarksToPage();
                            }
                        }
                    }
                }
            }

            doc.Save(path);
            this.OpenPdf(path);
        }

        private Image GetCardBackImage()
        {
            Image image = null;
            using (var stream = ResourceHelper.GetResourceStream(this.GetType().Assembly, "Mbrit.Vegas.Cards.Resources.TvwmBack.png"))
                image = Image.FromStream(stream);
            return image;
        }

        private Image GetHoodieFrontImage()
        {
            Image image = null;
            using (var stream = ResourceHelper.GetResourceStream(this.GetType().Assembly, "Mbrit.Vegas.Cards.Resources.TvwmFront.png"))
                image = Image.FromStream(stream);
            return image;
        }

        private void OpenPdf(string path) => Process.Start(@"C:\Program Files\Glyph & Cog\XpdfReader-win64\xpdf.exe", path);

        private void RenderBackground(PSD.XImage image, PdfGraphics pdf) => pdf.DrawImageInBox(image, pdf.BleedRect);

        private void button3_Click(object sender, EventArgs e)
        {
            PSP.PdfDocument doc = null;
            var image = GetCardBackImage();
            using (image)
            {
                using (var theImage = image.ToPdfXImage())
                {
                    using (var pdf = PdfGraphics.Create(250, 140, 3, out doc))
                    {
                        pdf.AddPage();
                        pdf.DrawImageInBox(theImage, pdf.BleedRect);
                        pdf.AddTrimMarksToPage();
                    }
                }
            }

            var path = @"c:\temp\back--" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".pdf";
            doc.Save(path);
            this.OpenPdf(path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PSP.PdfDocument doc = null;
            var image = GetHoodieFrontImage();
            using (image)
            {
                using (var theImage = image.ToPdfXImage())
                {
                    using (var pdf = PdfGraphics.Create(100, 33, 3, out doc))
                    {
                        pdf.AddPage();
                        pdf.DrawImageInBox2(theImage, pdf.BleedRect);
                        pdf.AddTrimMarksToPage();
                    }
                }
            }

            var path = @"c:\temp\front--" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".pdf";
            doc.Save(path);
            this.OpenPdf(path);
        }
    }
}
