namespace ZYCControl
{
    partial class UltraFTest
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
            this.ultravisionFieldRB = new ZYCControl.UltravisionFieldRB();
            this.SuspendLayout();
            // 
            // ultravisionFieldRB
            // 
            this.ultravisionFieldRB.BackColor = System.Drawing.Color.YellowGreen;
            this.ultravisionFieldRB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultravisionFieldRB.Location = new System.Drawing.Point(0, 0);
            this.ultravisionFieldRB.Name = "ultravisionFieldRB";
            this.ultravisionFieldRB.Size = new System.Drawing.Size(451, 365);
            this.ultravisionFieldRB.TabIndex = 0;
            // 
            // UltraFTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 365);
            this.Controls.Add(this.ultravisionFieldRB);
            this.Name = "UltraFTest";
            this.Text = "UltraFTest";
            this.ResumeLayout(false);

        }

        #endregion

        public UltravisionFieldRB ultravisionFieldRB;
    }
}