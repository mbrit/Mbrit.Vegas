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
            panel1 = new Panel();
            button1 = new Button();
            buttonUnbiased1 = new Button();
            buttonBiased1 = new Button();
            labelNumLosses = new Label();
            labelNumWins = new Label();
            listMode = new ComboBox();
            buttonBiasedN = new Button();
            buttonUnbiasedN = new Button();
            listHouseEdges = new ComboBox();
            checkShowBoxHands = new CheckBox();
            checkShowWeekday = new CheckBox();
            checkShowWeekend = new CheckBox();
            checkShowWedge = new CheckBox();
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
            buttonUnbiased1.Text = "&Unbiased";
            buttonUnbiased1.UseVisualStyleBackColor = true;
            buttonUnbiased1.Click += button2_Click;
            // 
            // buttonBiased1
            // 
            buttonBiased1.Location = new Point(168, 12);
            buttonBiased1.Name = "buttonBiased1";
            buttonBiased1.Size = new Size(100, 23);
            buttonBiased1.TabIndex = 5;
            buttonBiased1.Text = "&Biased";
            buttonBiased1.UseVisualStyleBackColor = true;
            buttonBiased1.Click += button3_Click;
            // 
            // labelNumLosses
            // 
            labelNumLosses.AutoSize = true;
            labelNumLosses.BackColor = Color.Transparent;
            labelNumLosses.Font = new Font("Consolas", 9F);
            labelNumLosses.Location = new Point(896, 120);
            labelNumLosses.Name = "labelNumLosses";
            labelNumLosses.Size = new Size(105, 14);
            labelNumLosses.TabIndex = 7;
            labelNumLosses.Text = "labelNumLosses";
            // 
            // labelNumWins
            // 
            labelNumWins.AutoSize = true;
            labelNumWins.Font = new Font("Consolas", 9F);
            labelNumWins.Location = new Point(896, 107);
            labelNumWins.Name = "labelNumWins";
            labelNumWins.Size = new Size(91, 14);
            labelNumWins.TabIndex = 6;
            labelNumWins.Text = "labelNumWins";
            // 
            // listMode
            // 
            listMode.DropDownStyle = ComboBoxStyle.DropDownList;
            listMode.FormattingEnabled = true;
            listMode.Location = new Point(561, 13);
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
            buttonUnbiasedN.Location = new Point(274, 41);
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
            listHouseEdges.Location = new Point(561, 41);
            listHouseEdges.Name = "listHouseEdges";
            listHouseEdges.Size = new Size(174, 23);
            listHouseEdges.TabIndex = 13;
            listHouseEdges.SelectedIndexChanged += listHouseEdges_SelectedIndexChanged;
            // 
            // checkShowBoxHands
            // 
            checkShowBoxHands.AutoSize = true;
            checkShowBoxHands.Location = new Point(772, 17);
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
            checkShowWeekday.Location = new Point(896, 17);
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
            checkShowWeekend.Location = new Point(896, 36);
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
            checkShowWedge.Location = new Point(896, 56);
            checkShowWedge.Name = "checkShowWedge";
            checkShowWedge.Size = new Size(95, 19);
            checkShowWedge.TabIndex = 17;
            checkShowWedge.Text = "Show Wedge";
            checkShowWedge.UseVisualStyleBackColor = true;
            checkShowWedge.CheckedChanged += checkShowWedge_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1071, 616);
            Controls.Add(checkShowWedge);
            Controls.Add(checkShowWeekend);
            Controls.Add(checkShowWeekday);
            Controls.Add(checkShowBoxHands);
            Controls.Add(listHouseEdges);
            Controls.Add(buttonBiasedN);
            Controls.Add(buttonUnbiasedN);
            Controls.Add(listMode);
            Controls.Add(labelNumLosses);
            Controls.Add(labelNumWins);
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

        private Panel panel1;
        private Button button1;
        private Button buttonUnbiased1;
        private Button buttonBiased1;
        private Label labelNumLosses;
        private Label labelNumWins;
        private ComboBox listMode;
        private Button buttonBiasedN;
        private Button buttonUnbiasedN;
        private ComboBox listHouseEdges;
        private CheckBox checkShowBoxHands;
        private CheckBox checkShowWeekday;
        private CheckBox checkShowWeekend;
        private CheckBox checkShowWedge;
    }
}
