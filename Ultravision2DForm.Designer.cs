namespace ZYCControl
{
    partial class Ultravision2DForm
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
            this.u2d = new ZYCControl.Ultravision2D();
            this.SuspendLayout();
            // 
            // u2d
            // 
            this.u2d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.u2d.Location = new System.Drawing.Point(0, 0);
            this.u2d.Name = "u2d";
            this.u2d.Size = new System.Drawing.Size(797, 448);
            this.u2d.TabIndex = 0;
            // 
            // Ultravision2DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 448);
            this.Controls.Add(this.u2d);
            this.Name = "Ultravision2DForm";
            this.Text = "Ultravision2DForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Ultravision2D u2d;
    }
}