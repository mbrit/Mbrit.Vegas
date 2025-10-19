namespace Mbrit.Vegas.Cards.Objects
{
    partial class BoxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Location = new Point(35, 31);
            panel1.Name = "panel1";
            panel1.Size = new Size(843, 642);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // BoxForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(906, 708);
            Controls.Add(panel1);
            Name = "BoxForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BoxForm";
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
    }
}