using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mbrit.Vegas.Cards.Objects
{
    public partial class BoxForm : Form
    {
        public BoxForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            new BoxRenderer().Render(this.panel1.ClientRectangle, e.Graphics);
        }
    }
}
