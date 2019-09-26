namespace ZYCControl
{
    partial class LongStripTest
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
            this.longStrip_Ruler1 = new ZYCControl.LongStrip_Ruler();
            this.SuspendLayout();
            // 
            // longStrip_Ruler1
            // 
            this.longStrip_Ruler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.longStrip_Ruler1.Location = new System.Drawing.Point(0, 0);
            this.longStrip_Ruler1.Name = "longStrip_Ruler1";
            this.longStrip_Ruler1.Size = new System.Drawing.Size(348, 322);
            this.longStrip_Ruler1.TabIndex = 0;
            // 
            // LongStripTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 322);
            this.Controls.Add(this.longStrip_Ruler1);
            this.Name = "LongStripTest";
            this.Text = "LongStripTest";
            this.ResumeLayout(false);

        }

        #endregion

        public LongStrip_Ruler longStrip_Ruler1;
    }
}