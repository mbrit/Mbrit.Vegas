namespace Mbrit.Vegas.Lens
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new DoubleBufferedPanel();
            button1 = new Button();
            buttonUnbiased1 = new Button();
            buttonBiased1 = new Button();
            listMode = new ComboBox();
            buttonBiasedN = new Button();
            buttonUnbiasedN = new Button();
            listHouseEdges = new ComboBox();
            checkShowBoxHands = new CheckBox();
            checkShowWeekday = new CheckBox();
            checkShowWeekend = new CheckBox();
            checkShowWedge = new CheckBox();
            checkShowTilt = new CheckBox();
            checkShowGame = new CheckBox();
            buttonBiased5 = new Button();
            buttonBiased20 = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.Location = new Point(38, 83);
            panel1.Name = "panel1";
            panel1.Size = new Size(836, 498);
            panel1.TabIndex = 0;
            panel1.Click += panel1_Click;
            panel1.Paint += panel1_Paint;
            panel1.Resize += panel1_Resize;
            // 
            // button1
            // 
            button1.Location = new Point(38, 12);
            button1.Name = "button1";
            button1.Size = new Size(124, 23);
            button1.TabIndex = 3;
            button1.Text = "&Biased Exemplar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // buttonUnbiased1
            // 
            buttonUnbiased1.Location = new Point(274, 12);
            buttonUnbiased1.Name = "buttonUnbiased1";
            buttonUnbiased1.Size = new Size(100, 23);
            buttonUnbiased1.TabIndex = 4;
            buttonUnbiased1.UseVisualStyleBackColor = true;
            buttonUnbiased1.Text = "Unbiased 1";
            buttonUnbiased1.Click += button2_Click;
            // 
            // buttonBiased1
            // 
            buttonBiased1.Location = new Point(168, 12);
            buttonBiased1.Name = "buttonBiased1";
            buttonBiased1.Size = new Size(100, 23);
            buttonBiased1.TabIndex = 5;
            buttonBiased1.UseVisualStyleBackColor = true;
            buttonBiased1.Text = "Biased 15";
            buttonBiased1.Click += button3_Click;
            // 
            // listMode
            // 
            listMode.DropDownStyle = ComboBoxStyle.DropDownList;
            listMode.FormattingEnabled = true;
            listMode.Location = new Point(654, 12);
            listMode.Name = "listMode";
            listMode.Size = new Size(174, 23);
            listMode.TabIndex = 10;
            listMode.SelectedIndexChanged += listMode_SelectedIndexChanged;
            // 
            // buttonBiasedN
            // 
            buttonBiasedN.Location = new Point(168, 41);
            buttonBiasedN.Name = "buttonBiasedN";
            buttonBiasedN.Size = new Size(100, 23);
            buttonBiasedN.TabIndex = 12;
            buttonBiasedN.Text = "&Biased";
            buttonBiasedN.UseVisualStyleBackColor = true;
            buttonBiasedN.Click += buttonBiasedN_Click;
            // 
            // buttonUnbiasedN
            // 
            buttonUnbiasedN.Location = new Point(531, 40);
            buttonUnbiasedN.Name = "buttonUnbiasedN";
            buttonUnbiasedN.Size = new Size(100, 23);
            buttonUnbiasedN.TabIndex = 11;
            buttonUnbiasedN.Text = "&Unbiased";
            buttonUnbiasedN.UseVisualStyleBackColor = true;
            buttonUnbiasedN.Click += buttonUnbiasedN_Click;
            // 
            // listHouseEdges
            // 
            listHouseEdges.DropDownStyle = ComboBoxStyle.DropDownList;
            listHouseEdges.FormattingEnabled = true;
            listHouseEdges.Location = new Point(654, 40);
            listHouseEdges.Name = "listHouseEdges";
            listHouseEdges.Size = new Size(174, 23);
            listHouseEdges.TabIndex = 13;
            listHouseEdges.SelectedIndexChanged += listHouseEdges_SelectedIndexChanged;
            // 
            // checkShowBoxHands
            // 
            checkShowBoxHands.AutoSize = true;
            checkShowBoxHands.Location = new Point(894, 121);
            checkShowBoxHands.Name = "checkShowBoxHands";
            checkShowBoxHands.Size = new Size(115, 19);
            checkShowBoxHands.TabIndex = 14;
            checkShowBoxHands.Text = "Show Box Hands";
            checkShowBoxHands.UseVisualStyleBackColor = true;
            checkShowBoxHands.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkShowWeekday
            // 
            checkShowWeekday.AutoSize = true;
            checkShowWeekday.Checked = true;
            checkShowWeekday.CheckState = CheckState.Checked;
            checkShowWeekday.Location = new Point(894, 44);
            checkShowWeekday.Name = "checkShowWeekday";
            checkShowWeekday.Size = new Size(106, 19);
            checkShowWeekday.TabIndex = 15;
            checkShowWeekday.Text = "Show Weekday";
            checkShowWeekday.UseVisualStyleBackColor = true;
            checkShowWeekday.CheckedChanged += checkShowWeekday_CheckedChanged;
            // 
            // checkShowWeekend
            // 
            checkShowWeekend.AutoSize = true;
            checkShowWeekend.Checked = true;
            checkShowWeekend.CheckState = CheckState.Checked;
            checkShowWeekend.Location = new Point(894, 63);
            checkShowWeekend.Name = "checkShowWeekend";
            checkShowWeekend.Size = new Size(107, 19);
            checkShowWeekend.TabIndex = 16;
            checkShowWeekend.Text = "Show Weekend";
            checkShowWeekend.UseVisualStyleBackColor = true;
            checkShowWeekend.CheckedChanged += checkShowWeekend_CheckedChanged;
            // 
            // checkShowWedge
            // 
            checkShowWedge.AutoSize = true;
            checkShowWedge.Checked = true;
            checkShowWedge.CheckState = CheckState.Checked;
            checkShowWedge.Location = new Point(894, 83);
            checkShowWedge.Name = "checkShowWedge";
            checkShowWedge.Size = new Size(95, 19);
            checkShowWedge.TabIndex = 17;
            checkShowWedge.Text = "Show Wedge";
            checkShowWedge.UseVisualStyleBackColor = true;
            checkShowWedge.CheckedChanged += checkShowWedge_CheckedChanged;
            // 
            // checkShowTilt
            // 
            checkShowTilt.AutoSize = true;
            checkShowTilt.Location = new Point(894, 158);
            checkShowTilt.Name = "checkShowTilt";
            checkShowTilt.Size = new Size(74, 19);
            checkShowTilt.TabIndex = 18;
            checkShowTilt.Text = "Show Tilt";
            checkShowTilt.UseVisualStyleBackColor = true;
            checkShowTilt.CheckedChanged += checkBox1_CheckedChanged_1;
            // 
            // checkShowGame
            // 
            checkShowGame.AutoSize = true;
            checkShowGame.Checked = true;
            checkShowGame.CheckState = CheckState.Checked;
            checkShowGame.Location = new Point(894, 12);
            checkShowGame.Name = "checkShowGame";
            checkShowGame.Size = new Size(89, 19);
            checkShowGame.TabIndex = 19;
            checkShowGame.Text = "Show Game";
            checkShowGame.UseVisualStyleBackColor = true;
            checkShowGame.CheckedChanged += checkShowGame_CheckedChanged;
            // 
            // button2
            // 
            buttonBiased5.Location = new Point(274, 41);
            buttonBiased5.Name = "buttonBiased5";
            buttonBiased5.Size = new Size(100, 23);
            buttonBiased5.TabIndex = 20;
            buttonBiased5.Text = "&Biased 5";
            buttonBiased5.UseVisualStyleBackColor = true;
            buttonBiased5.Click += button2_Click_1;
            // 
            // button3
            // 
            buttonBiased20.Location = new Point(380, 41);
            buttonBiased20.Name = "buttonBiased20";
            buttonBiased20.Size = new Size(100, 23);
            buttonBiased20.TabIndex = 21;
            buttonBiased20.Text = "&Biased 20";
            buttonBiased20.UseVisualStyleBackColor = true;
            buttonBiased20.Click += button3_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1071, 616);
            Controls.Add(buttonBiased20);
            Controls.Add(buttonBiased5);
            Controls.Add(checkShowGame);
            Controls.Add(checkShowTilt);
            Controls.Add(checkShowWedge);
            Controls.Add(checkShowWeekend);
            Controls.Add(checkShowWeekday);
            Controls.Add(checkShowBoxHands);
            Controls.Add(listHouseEdges);
            Controls.Add(buttonBiasedN);
            Controls.Add(buttonUnbiasedN);
            Controls.Add(listMode);
            Controls.Add(buttonBiased1);
            Controls.Add(buttonUnbiased1);
            Controls.Add(button1);
            Controls.Add(panel1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DoubleBufferedPanel panel1;
        private Button button1;
        private Button buttonUnbiased1;
        private Button buttonBiased1;
        private ComboBox listMode;
        private Button buttonBiasedN;
        private Button buttonUnbiasedN;
        private ComboBox listHouseEdges;
        private CheckBox checkShowBoxHands;
        private CheckBox checkShowWeekday;
        private CheckBox checkShowWeekend;
        private CheckBox checkShowWedge;
        private CheckBox checkShowTilt;
        private CheckBox checkShowGame;
        private Button buttonBiased5;
        private Button buttonBiased20;
    }
}
