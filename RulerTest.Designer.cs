namespace ZYCControl
{
    partial class RulerTest
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
            this.rulerBar1 = new ZYCControl.RulerBar();
            this.SuspendLayout();
            // 
            // rulerBar1
            // 
            this.rulerBar1.BackColor = System.Drawing.Color.Transparent;
            this.rulerBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rulerBar1.Location = new System.Drawing.Point(0, 192);
            this.rulerBar1.Name = "rulerBar1";
            this.rulerBar1.Size = new System.Drawing.Size(284, 69);
            this.rulerBar1.TabIndex = 0;
            this.rulerBar1.SizeChanged += new System.EventHandler(this.rulerBar1_SizeChanged);
            // 
            // RulerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.rulerBar1);
            this.Name = "RulerTest";
            this.Text = "RulerTest";
            this.SizeChanged += new System.EventHandler(this.RulerTest_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private RulerBar rulerBar1;
    }
}