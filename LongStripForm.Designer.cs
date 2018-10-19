namespace ZYCControl
{
    partial class LongStripForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.longStrip1 = new ZYCControl.LongStrip();
            this.rulerBarH = new ZYCControl.RulerBar();
            this.rulerBarV = new ZYCControl.RulerBar();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rulerBarH
            // 
            this.rulerBarH.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarH.Location = new System.Drawing.Point(30, 330);
            this.rulerBarH.Name = "rulerBarH";
            this.rulerBarH.Size = new System.Drawing.Size(509, 30);
            this.rulerBarH.TabIndex = 2;
            // 
            // longStrip1
            // 
            this.longStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.longStrip1.JudgeLine0 = 0F;
            this.longStrip1.JudgeLine0Enable = false;
            this.longStrip1.JudgeLine1 = 0F;
            this.longStrip1.JudgeLine1Enable = false;
            this.longStrip1.JudgeLine2 = 0F;
            this.longStrip1.JudgeLine2Enable = false;
            this.longStrip1.Location = new System.Drawing.Point(30, 0);
            this.longStrip1.Name = "longStrip1";
            this.longStrip1.Size = new System.Drawing.Size(508, 330);
            this.longStrip1.TabIndex = 0;
            // 
            // rulerBarV
            // 
            this.rulerBarV.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarV.Location = new System.Drawing.Point(0, 0);
            this.rulerBarV.Name = "rulerBarV";
            this.rulerBarV.Size = new System.Drawing.Size(30, 330);
            this.rulerBarV.TabIndex = 1;
            // 
            // LongStripForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 360);
            this.Controls.Add(this.rulerBarH);
            this.Controls.Add(this.rulerBarV);
            this.Controls.Add(this.longStrip1);
            this.DoubleBuffered = true;
            this.Name = "LongStripForm";
            this.Text = "LongStripForm";
            this.SizeChanged += new System.EventHandler(this.LongStripForm_SizeChanged);
            this.ResumeLayout(false);

        }
        
        #endregion

        public LongStrip longStrip1;
        private System.Windows.Forms.Timer timer1;
        private RulerBar rulerBarV;
        private RulerBar rulerBarH;
    }
}