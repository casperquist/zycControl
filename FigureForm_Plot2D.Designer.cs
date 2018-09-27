namespace ZYCControl
{
    partial class FigureForm_Plot2D
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
            this.figurePlot2D1 = new ZYCControl.FigurePlot2D();
            this.SuspendLayout();
            // 
            // figurePlot2D1
            // 
            this.figurePlot2D1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figurePlot2D1.Location = new System.Drawing.Point(0, 0);
            this.figurePlot2D1.Name = "figurePlot2D1";
            this.figurePlot2D1.Size = new System.Drawing.Size(284, 261);
            this.figurePlot2D1.TabIndex = 0;
            // 
            // FigureForm_Plot2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.figurePlot2D1);
            this.Name = "FigureForm_Plot2D";
            this.Text = "FigureForm_Plot2D";
            this.ResumeLayout(false);

        }

        #endregion

        private FigurePlot2D figurePlot2D1;
    }
}