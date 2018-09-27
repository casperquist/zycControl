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
            this.longStrip1 = new ZYCControl.LongStrip();
            this.SuspendLayout();
            // 
            // longStrip1
            // 
            this.longStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.longStrip1.JudgeLine0 = 0F;
            this.longStrip1.JudgeLine0Enable = false;
            this.longStrip1.JudgeLine1 = 0F;
            this.longStrip1.JudgeLine1Enable = false;
            this.longStrip1.JudgeLine2 = 0F;
            this.longStrip1.JudgeLine2Enable = false;
            this.longStrip1.Location = new System.Drawing.Point(0, 0);
            this.longStrip1.Name = "longStrip1";
            this.longStrip1.Size = new System.Drawing.Size(284, 261);
            this.longStrip1.TabIndex = 0;
            // 
            // LongStripForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.longStrip1);
            this.DoubleBuffered = true;
            this.Name = "LongStripForm";
            this.Text = "LongStripForm";
            this.ResumeLayout(false);

        }
        
        #endregion

        public LongStrip longStrip1;
    }
}