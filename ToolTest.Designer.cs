namespace ZYCControl
{
    partial class ToolTest
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
            this.ultraToolLayout1 = new ZYCControl.UltraToolLayout();
            this.SuspendLayout();
            // 
            // ultraToolLayout1
            // 
            this.ultraToolLayout1.BackColor = System.Drawing.Color.Transparent;
            this.ultraToolLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraToolLayout1.Location = new System.Drawing.Point(0, 0);
            this.ultraToolLayout1.Name = "ultraToolLayout1";
            this.ultraToolLayout1.Size = new System.Drawing.Size(284, 261);
            this.ultraToolLayout1.TabIndex = 0;
            // 
            // ToolTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ultraToolLayout1);
            this.Name = "ToolTest";
            this.Text = "ToolTest";
            this.ResumeLayout(false);

        }

        #endregion

        public UltraToolLayout ultraToolLayout1;
    }
}